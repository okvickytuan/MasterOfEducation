using UnityEngine;
using System.Collections;

public class Animal : MonoBehaviour {

	private PhotonView _view;
	private int _playerIndex;

	private bool _isInCage = true;
	private Transform _way;

	private int _stepPerArea = 11;

	private event del_no_param_no_return moveCompleteEvt = null;

	private int[] _destination = new int[2];

	internal bool IsInCage {
		get { return _isInCage; }
	}

	private int CurrentArea {
		get {
			return IsInCage ? 0 : int.Parse(transform.parent.parent.name);
		}
	}

	private int CurrentAreaIndex {
		get {
			return IsInCage ? 0 : int.Parse(transform.parent.name);
		}
	}

	// Use this for initialization
	void Start () {
		_view = GetComponent<PhotonView> ();
		_way = GameObject.Find ("Way").transform;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	internal void AddMoveEvent(del_no_param_no_return moveEvt) {
		moveCompleteEvt = moveEvt;
	}

	internal void GoToCage(int playerIndex) {
		GetComponent<PhotonView>().RPC ("PunGoToCage", PhotonTargets.All, playerIndex);
	}

	//Về chuồng
	[PunRPC]
	void PunGoToCage(int playerIndex) {
		this._playerIndex = playerIndex;
		this._isInCage = true;

		transform.parent = GameObject.Find ("Player " + (playerIndex+1)).transform.FindChild("animalSlot");
		transform.localPosition = Vector3.zero;
		Vector3 euler = transform.eulerAngles;
		euler.y = Random.Range (0, 360);
		transform.eulerAngles = euler;
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
				if (moveCompleteEvt != null) {
					moveCompleteEvt ();
				}
				return;
			}
			//Nếu nước đi là nước xuất phát và chưa phải vị trí cuối cùng thì không dc đi
			if (tmpArea[0] == _destination[0] && tmpArea[1] == _destination[1] &&
			    (tmpArea [0] != nextArea || tmpArea [1] != nextAreaIndex)) {
				if (moveCompleteEvt != null) {
					moveCompleteEvt ();
				}
				return;
			}
		} while (tmpArea[0] != nextArea || tmpArea[1] != nextAreaIndex);

		GameController._instance.debug.text = area + "-" + areaIndex + " -> " + step + " -> " + nextArea + "-" + nextAreaIndex;

		//Di chuyển từng bước 1 đến đích
		StartCoroutine(StepByStepTo(nextArea, nextAreaIndex));
	}

	//Xuất quân
	internal void GoToRoad() {
		_isInCage = false;
		_destination = new int[2] { this._playerIndex + 1, 1 };
		_view.RPC ("JumpTo", PhotonTargets.All, this._playerIndex + 1, 1);
		if (moveCompleteEvt != null) {
			moveCompleteEvt();
		}
	}

	private IEnumerator StepByStepTo(int area, int areaIndex) {
		//Di chuyển từng bước 1 cho đến khi tới đích
		while (CurrentArea != area || CurrentAreaIndex != areaIndex) {
			/*int nextArea = (CurrentAreaIndex + 1) <= _stepPerArea ? 
				CurrentArea : ((CurrentArea + 1) > 4 ? 1 : (CurrentArea + 1));
			int nextAreaIndex = (CurrentAreaIndex + 1) <= _stepPerArea ? 
				(CurrentAreaIndex + 1) : (CurrentAreaIndex + 1 - _stepPerArea);*/
			int[] nextStep = GetNextArea(CurrentArea, CurrentAreaIndex);
			//JumpTo(nextArea, nextAreaIndex);
			_view.RPC ("JumpTo", PhotonTargets.All, nextStep[0], nextStep[1]);
			yield return new WaitForSeconds(1.0f);
		}
		if (CurrentArea == _destination [0] && CurrentAreaIndex == _destination [1]) {
			GameController._instance.ShowWin();
		} else if (moveCompleteEvt != null) {
			moveCompleteEvt();
		}
	}

	//Nhảy tới ô nào đó
	[PunRPC]
	private void JumpTo(int area, int areaIndex) {
		Transform des = _way.FindChild ("" + area).FindChild ("" + areaIndex);

		//Nếu có thú thì đá
		Animal animal = GetAnimalInArea (area, areaIndex);
		if (animal != null) {
			Kick(animal);
		}
		transform.parent = des;
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;
	}

	//Đá thú về chuồng
	private void Kick(Animal player) {
		player.GoToCage (player._playerIndex);
	}

	//Lấy nước đi kế tiếp
	private int[] GetNextArea(int area, int areaIndex) {
		int nextArea = (areaIndex + 1) <= _stepPerArea ? 
			area : ((area + 1) > 4 ? 1 : (area + 1));
		int nextAreaIndex = (areaIndex + 1) <= _stepPerArea ? 
			(areaIndex + 1) : (areaIndex + 1 - _stepPerArea);
		return new int[] { nextArea, nextAreaIndex };
	}

	//Lấy animal ở nước đi nào đó, nếu không có thú trả về null
	private Animal GetAnimalInArea(int area, int areaIndex) {
		Transform des = _way.FindChild ("" + area).FindChild ("" + areaIndex);
		if (des.childCount > 0)
			return des.GetChild(0).GetComponent<Animal>();
		return null;
	}

}
