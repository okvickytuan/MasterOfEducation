using UnityEngine;
using System.Collections;

public class Animal : MonoBehaviour {

	private PhotonView _view;
	private int _playerIndex;

	private bool _isInCage = true;
	private Transform _way;

	private int _stepPerArea = 11;

	private event del_no_param_no_return moveCompleteEvt = null;

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

	internal void GitAnimalParent(int playerIndex) {
		GetComponent<PhotonView>().RPC ("GitParent", PhotonTargets.All, playerIndex);
	}

	[PunRPC]
	void GitParent(int playerIndex) {
		this._playerIndex = playerIndex;
		transform.parent = GameObject.Find ("Player " + (playerIndex+1)).transform.FindChild("animalSlot");
		transform.localPosition = Vector3.zero;
		Vector3 euler = transform.eulerAngles;
		euler.y = Random.Range (0, 360);
		transform.eulerAngles = euler;
	}

	internal void Move(int step) {
		if (_isInCage) {
			return;
		}
		int areaIndex = CurrentAreaIndex;
		int area = CurrentArea;

		int nextArea = (areaIndex + step) <= _stepPerArea ? area : ((area + 1) > 4 ? 1 : (area + 1));
		int nextAreaIndex = (areaIndex + step) <= _stepPerArea ? (areaIndex + step) : (areaIndex + step - _stepPerArea);
		
		StartCoroutine(StepByStepTo(nextArea, nextAreaIndex));
	}

	internal void GoToRoad() {
		_isInCage = false;
		//transform.parent = _way.FindChild ("" + (this._playerIndex+1)).FindChild ("1");
		//transform.localPosition = Vector3.zero;
		//transform.localRotation = Quaternion.identity;
		_view.RPC ("JumpTo", PhotonTargets.All, this._playerIndex + 1, 1);
		if (moveCompleteEvt != null) {
			moveCompleteEvt();
		}
	}

	private IEnumerator StepByStepTo(int area, int areaIndex) {

		while (CurrentArea != area || CurrentAreaIndex != areaIndex) {
			int nextArea = (CurrentAreaIndex + 1) <= _stepPerArea ? 
				CurrentArea : ((CurrentArea + 1) > 4 ? 1 : (CurrentArea + 1));
			int nextAreaIndex = (CurrentAreaIndex + 1) <= _stepPerArea ? 
				(CurrentAreaIndex + 1) : (CurrentAreaIndex + 1 - _stepPerArea);
			//JumpTo(nextArea, nextAreaIndex);
			_view.RPC ("JumpTo", PhotonTargets.All, nextArea, nextAreaIndex);
			yield return new WaitForSeconds(1.0f);
		}
		if (moveCompleteEvt != null) {
			moveCompleteEvt();
		}
	}

	[PunRPC]
	private void JumpTo(int area, int areaIndex) {
		GameController._instance.debug.text = area + " - " + areaIndex;
		transform.parent = _way.FindChild ("" + area).FindChild ("" + areaIndex);
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;
	}

}
