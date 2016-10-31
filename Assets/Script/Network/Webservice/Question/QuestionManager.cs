using UnityEngine;
using System.Collections;
using SimpleJSON;
using UnityEngine.UI;

public delegate void del_no_param();

public class QuestionManager : MonoBehaviour {

	internal static QuestionManager _instance;

	public RectTransform questionTable;		//Bang cau hoi
	public RectTransform questionShow;		//Object vi tri xuat hien cau hoi
	public RectTransform questionHide;		//Object vi tri an cau hoi

	private PhotonView _view;
	private Question question = null;		//Cau hoi hien tai

	private event del_no_param answerRightEvt = null;		//Event khi tra loi dung
	private event del_no_param answerWrongEvt = null;		//Event khi tra loi sai

	private float questionAppearTime = 0f;		//Thoi gian cau hoi xuat hien
	internal float QuestionAppearTime {
		get { return questionAppearTime; }
	}

	void Awake() {
		_instance = this;
	}

	// Use this for initialization
	void Start () {
		_view = GetComponent<PhotonView> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//Lay cau hoi tu webservice
	internal void ResponseQuestion(bool showOthers = false) {
		
		WWW w = new WWW (GameConfig.QUESTION_URL);
		while (!w.isDone) {
			
		}
		if (w.text != "") {
			JSONNode node = JSON.Parse (w.text);
			if (node["api"] != null && (API)int.Parse(node["api"]) == API.DatabaseCannotConnect) {
				Debug.LogError("Failed to connect to database");
			} else {
				_view.RPC("AddQuestion", PhotonTargets.All, w.text, showOthers);
			}
		}
	}

	//Luu cau hoi lay duoc - Add cau hoi vao bang cau hoi - Show bang cau hoi
	[PunRPC]
	void AddQuestion(string text, bool showOthers) {
		JSONNode node = JSON.Parse (text);
		GameController._instance.debug.text = node["api"];
		question = new Question();
		question.Id = node["qid"].AsInt;
		question.Ques = node["question_Vi"];
		question.AnswerA = node["answer_A_Vi"];
		question.AnswerB = node["answer_B_Vi"];
		question.AnswerC = node["answer_C_Vi"];
		question.AnswerD = node["answer_D_Vi"];
		question.addToTable(questionTable);

		GameController._instance.debug.text += " " + showOthers;
		if (showOthers == false) {
			GameController._instance.debug.text += " " + GameController._instance.PlayerTurn;
			if (PUNManager._instance.PlayerIndex+1 == GameController._instance.PlayerTurn) {
				ShowQuestionTable();
				Invoke("ShowOtherQuestionTable", 3);
			}
		} else {
			_view.RPC("ShowQuestionTable", PhotonTargets.All);
		}
	}

	//Gui request check cau tra loi
	public void RequestAnswer(string answer) {
		WWWForm form = new WWWForm ();
		form.AddField ("qid", question.Id);
		form.AddField ("answer", answer);

		WWW w = new WWW (GameConfig.CHECK_ANSWER_URL, form);
		while (!w.isDone) {
			
		}
		Debug.Log (w.text);
		if (w.text != "") {
			JSONNode node = JSON.Parse (w.text);
			if ((API)int.Parse(node["api"]) == API.DatabaseCannotConnect) {
				Debug.LogError("Failed to connect to database");
			} else if ((API)int.Parse(node["api"]) == API.AnswerCorrect) {
				Debug.Log("Answer Correct");
				if (answerRightEvt != null) {
					answerRightEvt();
				}
			} else if ((API)int.Parse(node["api"]) == API.AnswerWrong) {
				Debug.Log("Answer Wrong");
				if (answerWrongEvt != null) {
					answerWrongEvt();
				}
			}
		}
	}

	//Hien thi bang cau hoi - Lay thoi gian hien tai
	[PunRPC]
	public void ShowQuestionTable() {
		questionAppearTime = PhotonNetwork.time;
		iTween.MoveTo (questionTable.gameObject, new Vector3 (questionTable.position.x, questionShow.position.y, 0), 1.0f);
	}

	//Hien thi bang cau hoi cho nhung thanh vien con lai trong phong
	internal void ShowOtherQuestionTable() {
		_view.RPC ("ShowQuestionTable", PhotonTargets.Others);
	}

	internal void PunHideQuestionTable(PhotonTargets target) {
		_view.RPC ("HideQuestionTable", target);
	}

	//An bang cau hoi
	[PunRPC]
	internal void HideQuestionTable() {
		//_view.RPC ("PunHideQuestionTable", PhotonTargets.All);
		iTween.MoveTo (questionTable.gameObject, new Vector3 (questionTable.position.x, questionHide.position.y, 0), 1f);
		question = null;
	}

	//Add su kien khi tra loi Dung - Sai
	internal void AddAnswerEvent(del_no_param rightEvt, del_no_param wrongEvt) {
		answerRightEvt = rightEvt;
		answerWrongEvt = wrongEvt;
	}

}
