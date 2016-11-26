using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	internal static GameController _instance;

	public GameObject[] _animal = new GameObject[4];
	public UnityEngine.UI.Text debug;
	public UnityEngine.UI.Text countToStart;

	private PhotonView _view;

	private GameState gameState = GameState.ChooseFirstPlayerTurn;	//Trạng thái game hiện tại (Giành quyền đi đầu tiên)

	private int playerReady = 0;		//So nguoi choi da load xong, san sang...
	private int _playerTurn = 1;		//Luot cua nguoi choi hien tai
	private int timeToStart = 3;		//Thoi gian bat dau tran dau

	private double[] firstTotalAnswerTime = new double[4];	//Tong thoi gian tra loi cau hoi danh quyen di chuyen dau tien
	private double[] currentAnswerTime = new double[4];		//Thoi gian tra loi cau hoi tien tai
	private bool[] currentAnswerResult = new bool[4];		//Ket qua cau tra loi hien tai (Dung-True .... Sai-False)
	private int numberOfFirstQuestion = 3;					//So cau hoi de danh quyen di chuyen dau tien
	private int firstPlayerTurn = 0;						//Người chơi giành lượt đi đầu tiên


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
			//StartCoroutine("ShowFirstQuestions");
			ShowFirstQuestions();
		}
	}

	[PunRPC]
	void PunSetTurn(int turn) {
		_playerTurn = turn;
	}

	//Hien thị những câu hỏi đầu tiên để dành quyền đi trước
	private void ShowFirstQuestions() {
		QuestionManager._instance.AddAnswerEvent (AnswerFirstQuestionRight, AnswerFirstQuestionWrong, NotAnswerFirstQuestion);
		/*do {
			if (_playerTurn == PUNManager._instance.PlayerIndex + 1) {
				QuestionManager._instance.ResponseQuestion (true);
			}
			_view.RPC("DecreaseFirstQuestionShowed", PhotonTargets.All);
			yield return new WaitForSeconds(7);
		} while(numberOfFirstQuestion > 0);
		CheckToSetFirstPlayerTurn ();*/
		if (_playerTurn == PUNManager._instance.PlayerIndex + 1) {
			QuestionManager._instance.ResponseQuestion (true);
			_view.RPC("DecreaseFirstQuestionShowed", PhotonTargets.All);
		}
	}

	//Khi tra loi dung loat cau hoi dau tien
	private void AnswerFirstQuestionRight() {
		double deltaTime = PhotonNetwork.time - QuestionManager._instance.QuestionAppearTime;
		_view.RPC ("AddFirstTotalAnswerTime", PhotonTargets.All, PUNManager._instance.PlayerIndex, deltaTime);
		_view.RPC ("ChangeCurrentAnswer", PhotonTargets.All, PUNManager._instance.PlayerIndex, deltaTime, true);
		debug.text = "Time: " + firstTotalAnswerTime [PUNManager._instance.PlayerIndex];
	}

	//Khi tra loi sai loat cau hoi dau tien
	private void AnswerFirstQuestionWrong() {
		double deltaTime = PhotonNetwork.time - QuestionManager._instance.QuestionAppearTime;
		_view.RPC ("AddFirstTotalAnswerTime", PhotonTargets.All, PUNManager._instance.PlayerIndex, deltaTime);
		_view.RPC ("ChangeCurrentAnswer", PhotonTargets.All, PUNManager._instance.PlayerIndex, deltaTime, false);
		debug.text = "Time: " + firstTotalAnswerTime [PUNManager._instance.PlayerIndex];
	}

	//Khong tra loi loat cau hoi dau tien
	internal void NotAnswerFirstQuestion(double questionTime) {
		_view.RPC ("AddFirstTotalAnswerTime", PhotonTargets.All, PUNManager._instance.PlayerIndex, questionTime);
		_view.RPC ("ChangeCurrentAnswer", PhotonTargets.All, PUNManager._instance.PlayerIndex, questionTime, false);
	}

	//Thay đổi tổng thời gian trả lời câu hỏi của người chơi
	[PunRPC]
	private void AddFirstTotalAnswerTime(int playerIndex, double time) {
		firstTotalAnswerTime [playerIndex] += time;
		debug.text = "Time: " + firstTotalAnswerTime [PUNManager._instance.PlayerIndex];
	}

	[PunRPC]
	private void ChangeCurrentAnswer(int playerIndex, double time, bool result) {
		currentAnswerTime [playerIndex] = time;
		currentAnswerResult [playerIndex] = result;
	}

	[PunRPC]
	private void DecreaseFirstQuestionShowed() {
		numberOfFirstQuestion--;
	}

	//Hiển thị kết quả câu hỏi hiện tại cho các người chơi
	internal void ShowQuestionResult() {
		_view.RPC ("PunShowQuestionResult", PhotonTargets.All);
	}

	[PunRPC]
	private void PunShowQuestionResult() {
		if (gameState == GameState.ChooseFirstPlayerTurn) {
			//Tìm người trlời đúng và nhanh nhất in ra
			double minTime = 100.0;
			int playerChosen = 0;
			for (int i=0; i<currentAnswerResult.Length; i++) {
				if (currentAnswerResult[i] == true && currentAnswerTime[i] < minTime) {
					playerChosen = i+1;
					minTime = currentAnswerTime[i];
				}
			}
			debug.text = "Người trl đúng: " + (playerChosen == 0 ? "Khong co" : playerChosen.ToString());
			if (playerChosen == 0) {	//Không ai trả lời đúng
				if (numberOfFirstQuestion > 0) {	//Vẫn còn câu hỏi
					//Câu hỏi tiếp theo
					ShowFirstQuestions();
				} else {
					//Disconnect all
					print("Disconnect all player in room");
				}
			} else {
				_playerTurn = playerChosen;
				debug.text = "Player first is: Player " + _playerTurn;
			}
		}
		ResetFirstQuestionResult ();
	}

	//Reset câu trả lời của các người chơi
	[PunRPC]
	private void ResetFirstQuestionResult() {
		for (int i=0; i<currentAnswerResult.Length; i++) {
			currentAnswerResult[i] = false;
		}
	}

	//Kiem tra thoi gian ai tra loi nhanh nhất sẽ dành quyền ưu tiên đi trước
	private void CheckToSetFirstPlayerTurn() {
		/*double minTime = firstTotalAnswerTime [0];
		int playerIndex = 0;
		for (int i=1; i<firstTotalAnswerTime.Length; i++) {
			if (firstTotalAnswerTime[i] < minTime) {
				minTime = firstTotalAnswerTime[i];
				playerIndex = i;
			}
		}
		_view.RPC ("PunSetTurn", PhotonTargets.All, playerIndex + 1);*/
	}

	int RollDice() {
		return Random.Range (1, 7);
	}

}
