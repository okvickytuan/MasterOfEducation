using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	internal static GameController _instance;

	public GameObject[] _animal = new GameObject[4];
	public UnityEngine.UI.Text debug;
	public UnityEngine.UI.Text countToStart;

	private PhotonView _view;

	private int playerReady = 0;
	private int playerTurn = 0;
	private int timeToStart = 4;

	internal PhotonView PunView {
		get { return _view; }
	}

	void Awake() {
		_instance = this;
	}

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
		if (playerReady >= PhotonNetwork.room.playerCount) {
			playerReady = 0;
			CountToStart();
		}
	}

	void CountToStart() {
		InvokeRepeating ("PunCountToStart", 0, 1);
	}

	void PunCountToStart() {

		countToStart.transform.localScale = Vector3.one;
		if (timeToStart > 0) {
			countToStart.text = timeToStart.ToString ();
			iTween.ScaleTo (countToStart.gameObject, iTween.Hash ("position", Vector3.zero, "time", 1f, "easetype", "linear"));
		} else {
			countToStart.text = "Start !!!";
			iTween.ScaleTo (countToStart.gameObject, iTween.Hash ("position", Vector3.zero, "time", 1f, "easetype", "linear"));
			playerReady++;
			if (playerReady >= PhotonNetwork.room.playerCount) {
				ResponseQuestion();
			}
			CancelInvoke("PunCountToStart");
		}
		timeToStart--;
	}

	void ResponseQuestion() {
		debug.text = "START !!!";
		QuestionManager._instance.ResponseQuestion (true);
	}

	void StartGame() {

	}

	[PunRPC]
	void PunStartGame() {

	}

	internal void GetTurn() {
		_view.RPC ("PunGetTurn", PhotonTargets.All);
	}

	[PunRPC]
	void PunGetTurn() {
		if (playerTurn == 0) {
			playerTurn = PUNManager._instance.PlayerIndex+1;
			debug.text = "Turn: " + playerTurn.ToString();
			QuestionManager._instance.HideQuestionTable();
		}
	}

	int RollDice() {
		return Random.Range (1, 7);
	}

}
