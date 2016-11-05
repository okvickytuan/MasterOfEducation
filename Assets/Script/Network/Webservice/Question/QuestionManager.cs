using UnityEngine;
using System.Collections;
using SimpleJSON;
using UnityEngine.UI;

public delegate void del_no_param_no_return();

public class QuestionManager : MonoBehaviour {

	internal static QuestionManager _instance;

	public RectTransform questionTable;		//Bang cau hoi
	public RectTransform questionShow;		//Object vi tri xuat hien cau hoi
	public RectTransform questionHide;		//Object vi tri an cau hoi

	private PhotonView _view;
	private Question question = null;		//Cau hoi hien tai

	private bool isAnswered = false;		//Da tra loi cau hoi chua
	private event del_no_param_no_return answerRightEvt = null;		//Event khi tra loi dung
	private event del_no_param_no_return answerWrongEvt = null;		//Event khi tra loi sai

	private double questionAppearTime = 0f;		//Thoi gian cau hoi xuat hien
	internal double QuestionAppearTime {
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
		StartCoroutine (CoResponseQuestion (showOthers));
	}

	private IEnumerator CoResponseQuestion(bool showOthers) {
		
		WWW w = new WWW (GameConfig.QUESTION_URL);
		while (!w.isDone) {
			yield return new WaitForEndOfFrame();
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
		isAnswered = false;

		JSONNode node = JSON.Parse (text);
		question = new Question();
		question.Id = node["qid"].AsInt;
		question.Ques = node["question_Vi"];
		question.AnswerA = node["answer_A_Vi"];
		question.AnswerB = node["answer_B_Vi"];
		question.AnswerC = node["answer_C_Vi"];
		question.AnswerD = node["answer_D_Vi"];
		question.Time = node["time"].AsFloat;
		question.addToTable(questionTable);

		if (showOthers == false) {
			if (PUNManager._instance.PlayerIndex+1 == GameController._instance.PlayerTurn) {
				ShowQuestionTable();
				Invoke("ShowOtherQuestionTable", question.Time/2.0f);
			}
		} else {
			_view.RPC("ShowQuestionTable", PhotonTargets.All);
		}
	}

	//Gui request check cau tra loi
	public void RequestAnswer(string answer) {
		StartCoroutine (CoRequestAnswer (answer));
	}

	private IEnumerator CoRequestAnswer(string answer) {
		if (isAnswered == false) {		//Neu chua tra loi thi moi request cau tra loi
			WWWForm form = new WWWForm ();
			form.AddField ("qid", question.Id);
			form.AddField ("answer", answer);
		
			WWW w = new WWW (GameConfig.CHECK_ANSWER_URL, form);
			while (!w.isDone) {
				yield return new WaitForEndOfFrame ();
			}
			Debug.Log (w.text);
			if (w.text != "") {
				JSONNode node = JSON.Parse (w.text);
				if ((API)int.Parse (node ["api"]) == API.DatabaseCannotConnect) {
					Debug.LogError ("Failed to connect to database");
				} else if ((API)int.Parse (node ["api"]) == API.AnswerCorrect) {		//Tra loi dung
					Debug.Log ("Answer Correct");
					isAnswered = true;
					if (answerRightEvt != null) {
						answerRightEvt ();
					}
				} else if ((API)int.Parse (node ["api"]) == API.AnswerWrong) {		//Tra loi sai
					Debug.Log ("Answer Wrong");
					isAnswered = true;
					if (answerWrongEvt != null) {
						answerWrongEvt ();
					}
				}
			}
		}
	}

	//Hien thi bang cau hoi - Lay thoi gian hien tai
	[PunRPC]
	public void ShowQuestionTable() {
		questionAppearTime = PhotonNetwork.time;
		iTween.MoveTo (questionTable.gameObject, new Vector3 (questionTable.position.x, questionShow.position.y, 0), 1.0f);
		WaitToHideQuestionTable ();
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
		iTween.MoveTo (questionTable.gameObject, new Vector3 (questionTable.position.x, questionHide.position.y, 0), 1f);
		question = null;
	}

	//Add su kien khi tra loi Dung - Sai
	internal void AddAnswerEvent(del_no_param_no_return rightEvt, del_no_param_no_return wrongEvt) {
		answerRightEvt = rightEvt;
		answerWrongEvt = wrongEvt;
	}

	internal void WaitToHideQuestionTable() {
		if (question != null) {
			StartCoroutine("CoWaitToHideQuestionTable");
		}
	}

	private IEnumerator CoWaitToHideQuestionTable() {
		while(PhotonNetwork.time - QuestionManager._instance.QuestionAppearTime < question.Time) {
			yield return new WaitForFixedUpdate();
		}
		if (question != null) {
			HideQuestionTable ();
		}
		if (isAnswered == false) {
			GameController._instance.NotAnswerFirstQuestion();
		}
	}

}
