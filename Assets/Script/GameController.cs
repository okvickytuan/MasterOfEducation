using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public GameObject[] _animal = new GameObject[4];
	public UnityEngine.UI.Text debug;

	private PhotonView _view;

	private int playerReady = 0;
	private int playerTurn = 1;

	// Use this for initialization
	void Start () {
		_view = GetComponent<PhotonView> ();
		CreateAnimal ();
		CreatePlayer ();
		_view.RPC ("ReadyPlayer", PhotonTargets.All);
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

	[PunRPC]
	void ReadyPlayer() {
		playerReady++;

	}

	void ShowQuestion() {
		_view.RPC ("PunShowQuestion", PhotonTargets.All);
	}

	[PunRPC]
	void PunShowQuestion() {
		QuestionManager._instance.ResponseQuestion ();
		QuestionManager._instance.ShowQuestionTable ();
	}

	int RollDice() {
		return Random.Range (1, 7);
	}

}
