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

	private double[] currentAnswerTime = new double[4];		//Thoi gian tra loi cau hoi tien tai
	private bool[] currentAnswerResult = new bool[4];		//Ket qua cau tra loi hien tai (Dung-True .... Sai-False)


	//............CÁC THUỘC TÍNH CHO CÂU HỎI DÀNH QUYỀN ĐI TRƯỚC.....................

	private double[] firstTotalAnswerTime = new double[4];	//Tong thoi gian tra loi cau hoi danh quyen di chuyen dau tien
	private int numberOfFirstQuestion = 3;					//So cau hoi de danh quyen di chuyen dau tien
	private int firstPlayerTurn = 0;						//Người chơi giành lượt đi đầu tiên

	//............CÁC THUỘC TÍNH CHO CÂU HỎI LUÂN PHIÊN.....................
	//private int[] playersMoves = new int[4] { 1, 1, 1, 1 };		//Lượt mỗi người chơi
	private int[] playersRolls = new int[4] { 2, 2, 2, 2 };		//Lượt đổ xí ngầu mỗi người chơi (ban đầu có 2 lượt)

	private Animal _playerAnimal;

	internal PhotonView PunView {
		get { return _view; }
	}

	internal int PlayerTurn {
		get { return _playerTurn; }
	}

	internal bool[] CurrentAnswerResult {
		get { return currentAnswerResult; }
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

	void Update() {
		if (Input.GetKeyDown(KeyCode.G)) {
			this.ShowWin();
		}
	}

	//Tao cac animal
	void CreateAnimal() {
		int index = PUNManager._instance.PlayerIndex;
		GameObject go = PhotonNetwork.Instantiate (_animal [index].name, Vector3.zero, Quaternion.identity, 0) as GameObject;
		_playerAnimal = go.GetComponent<Animal>();
		_playerAnimal.GoToCage(index);
	}

	//Tao cac nguoi choi
	void CreatePlayer() {
		GameObject player = PhotonNetwork.Instantiate ("Player", Vector3.zero, Quaternion.identity, 0) as GameObject;
		player.GetComponent<PlayerNetwork> ().GitParent (PUNManager._instance.PlayerIndex);
		player.GetComponent<PlayerAnimation> ().ChangeClosest (
			PlayerClosest._curBody, PlayerClosest._curHair, PlayerClosest._curBeard,
			PlayerClosest._curHat, PlayerClosest._curBacket, PlayerClosest._curSkin,
			PlayerClosest._curFace, PlayerClosest._curWeapon, PlayerClosest._curBeardColor,
			PlayerClosest._curHairColor, PlayerClosest._curHatColor, PlayerClosest._curWeaponColor);

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

	//.............................................................................................................
	//............................TRẢ LỜI CÂU HỎI ĐẦU TIÊN DÀNH QUYỀN ĐI TRƯỚC.....................................
	//.............................................................................................................

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
		
		ResetCurrentAnswerTime ();
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
		//debug.text = "Time: " + firstTotalAnswerTime [PUNManager._instance.PlayerIndex];
		//debug.text = "" + currentAnswerTime[PUNManager._instance.PlayerIndex];

	}

	//Khi tra loi sai loat cau hoi dau tien
	private void AnswerFirstQuestionWrong() {
		double deltaTime = PhotonNetwork.time - QuestionManager._instance.QuestionAppearTime;
		_view.RPC ("AddFirstTotalAnswerTime", PhotonTargets.All, PUNManager._instance.PlayerIndex, deltaTime);
		_view.RPC ("ChangeCurrentAnswer", PhotonTargets.All, PUNManager._instance.PlayerIndex, deltaTime, false);
		//debug.text = "Time: " + firstTotalAnswerTime [PUNManager._instance.PlayerIndex];
		//debug.text = "" + currentAnswerTime[PUNManager._instance.PlayerIndex];
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
		//debug.text = "Time: " + firstTotalAnswerTime [PUNManager._instance.PlayerIndex];
	}

	[PunRPC]
	private void ChangeCurrentAnswer(int playerIndex, double time, bool result) {
		currentAnswerTime [playerIndex] = time;
		currentAnswerResult [playerIndex] = result;
		//debug.text = "Players time: ";
		//debug.text += "\n" + time;
		for (int i=0; i<currentAnswerTime.Length; i++) {
			//debug.text += "\n" + currentAnswerTime[i];
		}
	}

	[PunRPC]
	private void DecreaseFirstQuestionShowed() {
		numberOfFirstQuestion--;
	}

	//Hiển thị kết quả câu hỏi hiện tại cho các người chơi
	internal void ShowQuestionResult() {
		if (_playerTurn == PUNManager._instance.PlayerIndex + 1) {
			_view.RPC ("PunShowQuestionResult", PhotonTargets.All);
		}

	}

	[PunRPC]
	private void PunShowQuestionResult() {
		switch (gameState) {
		case GameState.ChooseFirstPlayerTurn:
			ShowFirstQuestionResult();
			break;
		case GameState.Ingame:
			ShowIngameQuestionResult();
			break;
		case GameState.Finish:

			break;
		}

	}

	//Kiem tra thoi gian ai tra loi nhanh nhất sẽ dành quyền ưu tiên đi trước
	private void ShowFirstQuestionResult() {
		//Tìm người trlời đúng và nhanh nhất in ra
		double minTime = 100.0;
		int playerChosen = 0;
		//debug.text = "Danh sach nguoi choi";
		for (int i=0; i<currentAnswerResult.Length; i++) {
			//debug.text += "\n" + currentAnswerTime[i];
			if (currentAnswerResult[i] == true && currentAnswerTime[i] < minTime) {
				playerChosen = i+1;
				minTime = currentAnswerTime[i];
			}
		}
		//debug.text = "Người trl đúng: " + (playerChosen == 0 ? "Khong co" : playerChosen.ToString());
		if (playerChosen == 0) {	//Không ai trả lời đúng
			if (numberOfFirstQuestion >= 0) {	//Vẫn còn câu hỏi
				//Câu hỏi tiếp theo
				ShowFirstQuestions();
			} else {							//Hết câu hỏi
				bool noOneAnswer = true;
				for (int i=1; i<firstTotalAnswerTime.Length; i++) {
					if (firstTotalAnswerTime[0] != firstTotalAnswerTime[i]) {
						noOneAnswer = false;
						break;
					}
				}
				if (noOneAnswer == true) {		//Disconnect all
					GameController._instance.debug.text = "Disconnect all player in room";
					PhotonNetwork.Disconnect();
				} else {
					for (int i=0; i<currentAnswerResult.Length; i++) {
						if (currentAnswerTime[i] == minTime) {
							playerChosen = i+1;
							minTime = currentAnswerTime[i];
						}
					}
					_playerTurn = playerChosen;
					firstPlayerTurn = _playerTurn;
					gameState = GameState.Ingame;
					//ShowQuestion();
					RollDice();
					debug.text = "Player first is: Player " + _playerTurn;
				}
			}
		} else {
			_playerTurn = playerChosen;
			firstPlayerTurn = _playerTurn;
			gameState = GameState.Ingame;
			//ShowQuestion();
			RollDice();
			debug.text = "Player first is: Player " + _playerTurn;
		}
		//ResetPlayersAnswerResult ();
	}

	//Reset câu trả lời của các người chơi
	[PunRPC]
	private void ResetPlayersAnswerResult() {
		for (int i=0; i<currentAnswerResult.Length; i++) {
			currentAnswerResult[i] = false;
		}
	}

	private void ResetCurrentAnswerTime() {
		for (int i=0; i<currentAnswerTime.Length; i++) {
			currentAnswerTime[i] = 0.0;
		}
		//debug.text += "\nresettttt";
	}



	//.............................................................................................................
	//...........................INGAME: THAY PHIÊN NHAU ĐỔ XÍ NGẦU ĐỂ DI CHUYỂN...................................
	//.............................................................................................................
	
	private void ShowQuestion() {
		QuestionManager._instance.AddAnswerEvent (AnswerRight, AnswerWrong, NotAnswer);
		
		ResetCurrentAnswerTime ();
		ResetPlayersAnswerResult ();
		if (_playerTurn == PUNManager._instance.PlayerIndex + 1) {
			QuestionManager._instance.ResponseQuestion ();
			//_view.RPC ("DecreasePlayerMove", PhotonTargets.All, PUNManager._instance.PlayerIndex);
		}
	}

	private void AnswerRight() {
		double deltaTime = PhotonNetwork.time - QuestionManager._instance.QuestionAppearTime;
		_view.RPC ("ChangeCurrentAnswer", PhotonTargets.All, PUNManager._instance.PlayerIndex, deltaTime, true);
	}

	private void AnswerWrong() {
		double deltaTime = PhotonNetwork.time - QuestionManager._instance.QuestionAppearTime;
		_view.RPC ("ChangeCurrentAnswer", PhotonTargets.All, PUNManager._instance.PlayerIndex, deltaTime, false);
	}

	private void NotAnswer(double questionTime) {
		if (_playerTurn == PUNManager._instance.PlayerIndex+1)
			_view.RPC ("ChangeCurrentAnswer", PhotonTargets.All, PUNManager._instance.PlayerIndex, questionTime, false);
	}

	[PunRPC]
	private void DecreasePlayerMove(int playerIndex) {
		//playersMoves [playerIndex] -= 1;
	}

	[PunRPC]
	private void DecreasePlayerRoll(int playerIndex) {
		playersRolls [playerIndex] -= 1;
	}

	[PunRPC]
	private void ResetPlayerRoll() {
		for (int i=0; i<playersRolls.Length; i++) {
			playersRolls [i] = 1;
		}
	}

	//Hiện kết quả
	private void ShowIngameQuestionResult() {
		/*
		//Tìm người trlời đúng và nhanh nhất in ra
		double minTime = 100.0;
		int playerChosen = 0;
		//debug.text = "Danh sach nguoi choi";
		for (int i=0; i<currentAnswerResult.Length; i++) {
			//debug.text += "\n" + currentAnswerTime[i];
			if (currentAnswerResult[i] == true && currentAnswerTime[i] < minTime) {
				playerChosen = i+1;
				minTime = currentAnswerTime[i];
			}
		}

		if (playerChosen == 0) {	//Không ai trả lời đúng
			ShowQuestion();
		} else {
			debug.text = "Player right: Player " + playerChosen;
			RollDice(playerChosen);
		}*/

		//Nếu trả lời đúng - Được đổ xí ngầu
		if (currentAnswerResult [_playerTurn - 1] == true) {
			//debug.text = "Roll";
			RollDice();
		} else {	//Nếu trả lời sai - Lượt chơi cho người tiếp theo
			//debug.text = "Next turn";
			NextTurn();
		}

	}

	//Đổ xí ngầu
	private void RollDice() {
		UIEffectManager._instance.ShowPlayerTurnEffect (_playerTurn);

		_playerAnimal.AddMoveEvent (CheckToRollDice);
		if (_playerTurn == PUNManager._instance.PlayerIndex+1) {
			DiceManager._instance._canRoll = true;
			DiceManager._instance.AddRollDoneEvent(RollDone);
			DiceManager._instance.AutoRoll();
		}
	}

	//Đổ xong xí ngầu
	private void RollDone(int dot) {
		//Stop animation đổ xí ngầu cho các người chơi khác
		//Đồng thời gửi cho các người chơi còn lại số xí ngầu mình đổ được
		DiceManager._instance.StopRoll (dot, PhotonTargets.Others);

		//Nếu là lượt đầu tiên đổ hoặc đổ được 1->5 thì giảm số lượt được đổ đi 1
		if (playersRolls [_playerTurn-1] > 1 || dot != 6) {
			_view.RPC("DecreasePlayerRoll", PhotonTargets.All, PUNManager._instance.PlayerIndex);
		}
		//debug.text = "" + playersRolls [_playerTurn - 1];

		//Nếu thú đang ở trong chuồng
		if (_playerAnimal.IsInCage) {
			//Nếu đổ được 1 hoặc 6 thì xuất chuồng
			if (dot == 6 || dot == 1) {
				_playerAnimal.GoToRoad();
			} else {		//Nếu không thì kiểm tra xem còn lượt đổ hay không
				CheckToRollDice();
			}
		} else {			//Nếu thú không ở trong chuồng thì di chuyển nó
			_playerAnimal.Move(dot);
		}
	}

	//Chuyển lượt chơi sang người kế tiếp
	[PunRPC]
	private void NextTurn() {
		//Reset lượt đổ xí ngầu của người chơi cũ
		if (playersRolls [_playerTurn - 1] == 0) {
			playersRolls [_playerTurn - 1] = 1;
		}
		//Xoay vòng lượt đến người chơi tiếp theo...
		_playerTurn = _playerTurn == 4 ? 1 : _playerTurn+1;

		ShowQuestion();
	}

	//Kiểm tra lượt đổ xí ngầu
	private void CheckToRollDice() {
		//Nếu còn lượt đổ thì tiếp tục đổ
		if (playersRolls [_playerTurn-1] > 0) {
			RollDice();
		} else {	//Nếu hết lượt đổ thì đến lượt người chơi tiếp theo
			_view.RPC ("NextTurn", PhotonTargets.All);
		}
	}

	internal void ShowWin() {
		_view.RPC ("PunShowWin", PhotonTargets.All);
	}

	[PunRPC]
	private void PunShowWin() {
		debug.text = "PLAYER WIN: " + _playerTurn;

		if (PUNManager._instance.PlayerIndex == 0) {
			PhotonNetwork.Disconnect ();
			Application.LoadLevel ("Lobby");
		}
	}



}
