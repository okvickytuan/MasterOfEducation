using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	internal static GameController _instance;

	public GameObject[] _animal = new GameObject[4];
	public UnityEngine.UI.Text debug;
	public UnityEngine.UI.Text countToStart;

	private PhotonView _view;

	private int playerReady = 0;		//So nguoi choi da load xong, san sang...
	private int _playerTurn = 1;		//Luot cua nguoi choi hien tai
	private int timeToStart = 3;		//Thoi gian bat dau tran dau

	private double[] firstTotalAnswerTime = new double[4];	//Tong thoi gian tra loi cau hoi danh quyen di chuyen dau tien
	private int numberOfFirstQuestion = 3;					//So cau hoi de danh quyen di chuyen dau tien

	internal PhotonView PunView {
		get { return _view; }
	}

	internal int PlayerTurn {
		get { return _playerTurn; }
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

	//Tao cac animal
	void CreateAnimal() {
		int index = PUNManager._instance.PlayerIndex;
		GameObject go = PhotonNetwork.Instantiate (_animal [index].name, Vector3.zero, Quaternion.identity, 0) as GameObject;
		Animal animal = go.GetComponent<Animal>();
		animal.GitAnimalParent(index);
	}

	//Tao cac nguoi choi
	void CreatePlayer() {
		GameObject player = PhotonNetwork.Instantiate ("Player", Vector3.zero, Quaternion.identity, 0) as GameObject;
		player.GetComponent<PlayerNetwork> ().GitParent (PUNManager._instance.PlayerIndex);
	}

	//Voi moi nguoi choi vao game, kiem tra tat ca nguoi choi da load xong scene chua
	[PunRPC]
	void ReadyPlayer() {
		playerReady++;
		//Neu tat ca da load xong, moi nguoi choi bat dau dem thoi gian de start game
		if (playerReady >= PhotonNetwork.room.playerCount) {
			playerReady = 0;
			InvokeRepeating ("CountToStart", 0, 1);
		}
	}

	//Dem thoi gian de start game
	void CountToStart() {
		countToStart.transform.localScale = Vector3.one;
		if (timeToStart > 0) {
			countToStart.text = timeToStart.ToString ();
			iTween.ScaleTo (countToStart.gameObject, iTween.Hash ("position", Vector3.zero, "time", 1f, "easetype", "linear"));
		} else {
			debug.text = "player: " + (PUNManager._instance.PlayerIndex+1);
			_view.RPC("StartGame", PhotonTargets.All);
			countToStart.text = "START !!!";
			Invoke("HideCountText", 1);
			iTween.ScaleTo (countToStart.gameObject, iTween.Hash ("position", Vector3.zero, "time", 1f, "easetype", "linear"));
			CancelInvoke("CountToStart");
		}
		timeToStart--;
	}

	//An thoi gian sau khi dem xong
	void HideCountText() {
		countToStart.text = "";
	}

	//Bat dau game sau khi dem xong - Nguoi choi 1 lay cau hoi va gui cho tat ca nguoi choi trong phong
	[PunRPC]
	void StartGame() {
		playerReady++;

		if (playerReady == PhotonNetwork.room.playerCount) {
			StartCoroutine("ShowFirstQuestions");
		}
	}

	[PunRPC]
	void PunSetTurn(int turn) {
		_playerTurn = turn;
	}

	/*[PunRPC]
	void PunGetTurn() {
		if (_playerTurn == 0) {
			_playerTurn = PUNManager._instance.PlayerIndex+1;
			debug.text = "Turn: " + _playerTurn.ToString();
			QuestionManager._instance.HideQuestionTable();
		}
	}*/

	//Hien thị những câu hỏi đầu tiên để dành quyền đi trước
	private IEnumerator ShowFirstQuestions() {
		do {
			QuestionManager._instance.AddAnswerEvent (AnswerFirstQuestionRight, AnswerFirstQuestionWrong);
			if (_playerTurn == PUNManager._instance.PlayerIndex + 1) {
				QuestionManager._instance.ResponseQuestion (true);
			}
			numberOfFirstQuestion--;
			yield return new WaitForSeconds(10);
		} while(numberOfFirstQuestion > 0);
		CheckToSetFirstPlayerTurn ();
	}

	private void AnswerFirstQuestionRight() {
		double deltaTime = PhotonNetwork.time - QuestionManager._instance.QuestionAppearTime;
		_view.RPC ("AddFirstTotalAnswerTime", PhotonTargets.All, PUNManager._instance.PlayerIndex, deltaTime);
		debug.text = "Time: " + firstTotalAnswerTime [PUNManager._instance.PlayerIndex];
		//QuestionManager._instance.HideQuestionTable ();
	}

	private void AnswerFirstQuestionWrong() {
		double deltaTime = 5.0f;
		_view.RPC ("AddFirstTotalAnswerTime", PhotonTargets.All, PUNManager._instance.PlayerIndex, deltaTime);
		debug.text = "Time: " + firstTotalAnswerTime [PUNManager._instance.PlayerIndex];
	}

	internal void NotAnswerFirstQuestion() {
		_view.RPC ("AddFirstTotalAnswerTime", PhotonTargets.All, PUNManager._instance.PlayerIndex, 5.0);
	}

	[PunRPC]
	private void AddFirstTotalAnswerTime(int playerIndex, double time) {
		firstTotalAnswerTime [playerIndex] += time;
	}

	//Kiem tra thoi gian ai tra loi nhanh nhất sẽ dành quyền ưu tiên đi trước
	private void CheckToSetFirstPlayerTurn() {
		double minTime = firstTotalAnswerTime [0];
		int playerIndex = 0;
		for (int i=1; i<firstTotalAnswerTime.Length; i++) {
			if (firstTotalAnswerTime[i] < minTime) {
				minTime = firstTotalAnswerTime[i];
				playerIndex = i;
			}
		}
		_view.RPC ("PunSetTurn", PhotonTargets.All, playerIndex + 1);
	}

	int RollDice() {
		return Random.Range (1, 7);
	}

}
