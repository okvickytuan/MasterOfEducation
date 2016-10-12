using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public GameObject[] _animal = new GameObject[4];

	private PhotonView _view;

	// Use this for initialization
	void Start () {
		_view = GetComponent<PhotonView> ();
		CreateAnimal ();
		CreatePlayer ();
	}
	
	void CreateAnimal() {
		int index = PUNManager._instance.PlayerIndex;
		GameObject go = PhotonNetwork.Instantiate (_animal [index].name, Vector3.zero, Quaternion.identity, 0) as GameObject;
		Animal animal = go.GetComponent<Animal>();
		animal.GitAnimalParent(index);
	}

	void CreatePlayer() {
		GameObject player = PhotonNetwork.Instantiate ("Player", Vector3.zero, Quaternion.identity, 0) as GameObject;
		player.GetComponent<PlayerNetwork> ().GitParent (PUNManager._instance.PlayerIndex);
	}

}
