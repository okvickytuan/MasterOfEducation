using UnityEngine;
using System.Collections;

public class Animal : MonoBehaviour {

	private PhotonView _view;
	private int _playerIndex;
	private int _slot;

	// Use this for initialization
	void Start () {
		_view = GetComponent<PhotonView> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	internal void GitAnimalParent(int playerIndex, int slot) {
		GetComponent<PhotonView>().RPC ("GitParent", PhotonTargets.All, playerIndex, slot);
	}

	[PunRPC]
	void GitParent(int playerIndex, int slot) {
		this._playerIndex = playerIndex;
		this._slot = slot;
		transform.parent = GameObject.Find ("Player " + (playerIndex+1)).transform.FindChild("slot_" + slot);
		transform.localPosition = Vector3.zero;
	}

}
