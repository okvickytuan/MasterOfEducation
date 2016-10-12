using UnityEngine;
using System.Collections;

public class Animal : MonoBehaviour {

	private PhotonView _view;
	private int _playerIndex;

	// Use this for initialization
	void Start () {
		_view = GetComponent<PhotonView> ();
	}
	
	// Update is called once per frame
	void Update () {
	
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

}
