using UnityEngine;
using System.Collections;

public class Animal : MonoBehaviour {

	protected PhotonView _view;
	protected int _playerIndex = -1;

	protected bool _isInCage = true;
	protected Transform _way;

	protected int _stepPerArea = 11;

	protected event del_no_param_no_return moveCompleteEvt = null;

	protected int[] _destination = new int[2];

	internal bool IsInCage {
		get { return _isInCage; }
	}

	protected int CurrentArea {
		get {
			return IsInCage ? 0 : int.Parse(transform.parent.parent.name);
		}
	}

	protected int CurrentAreaIndex {
		get {
			return IsInCage ? 0 : int.Parse(transform.parent.name);
		}
	}

	// Use this for initialization
	protected virtual void Start () {
		_view = GetComponent<PhotonView> ();
		_way = GameObject.Find ("Way").transform;
	}

	internal void AddMoveEvent(del_no_param_no_return moveEvt) {
		moveCompleteEvt = moveEvt;
	}

	protected virtual void MoveComplete() {
		if (moveCompleteEvt != null) {
			moveCompleteEvt ();
		}
	}

	internal void GoToCage(int playerIndex) {
		GetComponent<PhotonView>().RPC ("PunGoToCage", PhotonTargets.All, playerIndex);
	}

	//Ve chuong
	[PunRPC]
	protected void PunGoToCage(int playerIndex) {
		_playerIndex = playerIndex;
		this._isInCage = true;

		transform.parent = GameObject.Find ("Player " + (_playerIndex+1)).transform.FindChild("animalSlot");
		transform.localPosition = Vector3.zero;
		Vector3 euler = transform.eulerAngles;
		euler.y = Random.Range (0, 360);
		transform.eulerAngles = euler;

		GameEffectManager._instance.CreateEffect ("Prefabs/Particle/visionBuff", transform.parent.position, 2.0f);
	}

	//Di chuyển các animal
	internal void Move(int step) {
		if (_isInCage) {
			return;
		}
		//Vị trí hiện tại
		int areaIndex = CurrentAreaIndex;
		int area = CurrentArea;

		//Vị trí nó muốn tới
		int nextArea = (areaIndex + step) <= _stepPerArea ? area : ((area + 1) > 4 ? 1 : (area + 1));
		int nextAreaIndex = (areaIndex + step) <= _stepPerArea ? (areaIndex + step) : (areaIndex + step - _stepPerArea);

		//Kiểm tra đường đi trước khi di chuyển
		int[] tmpArea = new int[] { area, areaIndex };
		do {
			//Lấy nước đi tiếp theo
			tmpArea = GetNextArea (tmpArea [0], tmpArea [1]);
			//Lấy con thú ở nước đi đó. Nếu ko có sẽ = null
			Animal animal = GetAnimalInArea (tmpArea [0], tmpArea [1]);
			//Nếu có con thú và chưa phải vị trí cuối cùng thì không được đi
			if (animal != null && (tmpArea [0] != nextArea || tmpArea [1] != nextAreaIndex)) {
				MoveComplete();
				return;
			}
			//Nếu nước đi là nước xuất phát và chưa phải vị trí cuối cùng thì không dc đi
			if (tmpArea[0] == _destination[0] && tmpArea[1] == _destination[1] &&
			    (tmpArea [0] != nextArea || tmpArea [1] != nextAreaIndex)) {
				MoveComplete();
				return;
			}
		} while (tmpArea[0] != nextArea || tmpArea[1] != nextAreaIndex);

		//GameController._instance.debug.text = area + "-" + areaIndex + " -> " + step + " -> " + nextArea + "-" + nextAreaIndex;

		//Di chuyển từng bước 1 đến đích
		StartCoroutine(StepByStepTo(nextArea, nextAreaIndex));
	}

	//Xuất quân
	internal virtual void GoToRoad() {

	}

	protected IEnumerator StepByStepTo(int area, int areaIndex) {
		//GameEffectManager._instance.TurnLight (false);

		//Di chuyển từng bước 1 cho đến khi tới đích
		while (CurrentArea != area || CurrentAreaIndex != areaIndex) {
			int[] nextStep = GetNextArea(CurrentArea, CurrentAreaIndex);
			//JumpTo(nextArea, nextAreaIndex);
			_view.RPC ("JumpTo", PhotonTargets.All, nextStep[0], nextStep[1]);
			yield return new WaitForSeconds(0.6f);
		}
		if (CurrentArea == _destination [0] && CurrentAreaIndex == _destination [1]) {
			GameController._instance.ShowWin();
		} /*else if (moveCompleteEvt != null) {
			moveCompleteEvt();
		}*/
		yield return new WaitForSeconds (2.0f);
		//GameEffectManager._instance.TurnLight (true);
		if (moveCompleteEvt != null && !GameController._instance.IsDoneGame) {
			moveCompleteEvt();
		}
	}

	//Nhảy tới ô nào đó
	[PunRPC]
	protected virtual void JumpTo(int area, int areaIndex) {
		/*Transform des = _way.FindChild ("" + area).FindChild ("" + areaIndex);

		//Nếu có thú thì đá
		Animal animal = GetAnimalInArea (area, areaIndex);
		if (animal != null) {
			Kick(animal);
		}
		transform.parent = des;
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;*/
	}

	//Đá thú về chuồng
	protected IEnumerator Kick(Animal player) {
		//Lightning effect
		Vector3 lightningPos = new Vector3 (player.transform.position.x, player.transform.position.y + 8.5f,
		                                   player.transform.position.z);
		GameEffectManager._instance.CreateEffect ("Prefabs/Particle/lightning", lightningPos, 2.0f);
		SoundManager._instance.PlayEffect (SoundConfig.LIGHTNING_PATH, 1.0f);
		yield return new WaitForSeconds (2.0f);
		player.GoToCage (player._playerIndex);
	}

	//Lấy nước đi kế tiếp
	protected int[] GetNextArea(int area, int areaIndex) {
		int nextArea = (areaIndex + 1) <= _stepPerArea ? 
			area : ((area + 1) > 4 ? 1 : (area + 1));
		int nextAreaIndex = (areaIndex + 1) <= _stepPerArea ? 
			(areaIndex + 1) : (areaIndex + 1 - _stepPerArea);
		return new int[] { nextArea, nextAreaIndex };
	}

	//Lấy animal ở nước đi nào đó, nếu không có thú trả về null
	protected Animal GetAnimalInArea(int area, int areaIndex) {
		Transform des = _way.FindChild ("" + area).FindChild ("" + areaIndex);
		if (des.childCount > 1)
			return des.GetComponentInChildren<Animal> ();
		return null;
	}

	internal void ShowArrow() {
		GetComponent<PhotonView> ().RPC ("PunShowArrow", PhotonTargets.All);
	}

	internal void HideArrow() {
		GetComponent<PhotonView> ().RPC ("PunHideArrow", PhotonTargets.All);
	}

	[PunRPC]
	private void PunShowArrow() {
		Transform arrow = transform.FindChild ("arrow");
		arrow.GetComponent<ParticleSystem> ().Play ();
	}

	[PunRPC]
	private void PunHideArrow() {
		Transform arrow = transform.FindChild ("arrow");
		arrow.GetComponent<ParticleSystem> ().Stop ();
	}

}
