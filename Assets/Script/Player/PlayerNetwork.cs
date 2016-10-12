using UnityEngine;
using System.Collections;

public class PlayerNetwork : MonoBehaviour {

	private int _playerIndex;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	internal void GitParent(int playerIndex) {
		GetComponent<PhotonView> ().RPC ("RPCGitParent", PhotonTargets.All, playerIndex);
	}

	internal void ChangeClosest(string closest) {
		//PlayerAnimation anim = GetComponent<PlayerAnimation> ();

	}

	[PunRPC]
	void RPCGitParent(int playerIndex) {
		this._playerIndex = playerIndex;
		transform.parent = GameObject.Find ("Player " + (playerIndex + 1)).transform.FindChild ("playerSlot");
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;
	}

}
