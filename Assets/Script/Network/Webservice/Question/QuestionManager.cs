using UnityEngine;
using System.Collections;
using SimpleJSON;
using UnityEngine.UI;

public class QuestionManager : MonoBehaviour {

	internal static QuestionManager _instance;

	public RectTransform questionTable;

	private PhotonView _view;
	private Question question = null;

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

	internal void ResponseQuestion(bool showOthers = false) {
		
		WWW w = new WWW (GameConfig.QUESTION_URL);
		while (!w.isDone) {
			
		}
		if (w.text != "") {
			JSONNode node = JSON.Parse (w.text);
			if (node["api"] != null && (API)int.Parse(node["api"]) == API.DatabaseCannotConnect) {
				Debug.LogError("Failed to connect to database");
			} else {
				_view.RPC("AddQuestion", PhotonTargets.All, node, showOthers);
			}
		}
	}

	[PunRPC]
	void AddQuestion(JSONNode node, bool showOthers) {
		if (node != null && node ["api"] != null) {
			question = new Question();
			question.Id = node["qid"].AsInt;
			question.Ques = node["question_Vi"];
			question.AnswerA = node["answer_A_Vi"];
			question.AnswerB = node["answer_B_Vi"];
			question.AnswerC = node["answer_C_Vi"];
			question.AnswerD = node["answer_D_Vi"];
			question.addToTable(questionTable);

			ShowQuestionTable();
		}
	}

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

			} else if ((API)int.Parse(node["api"]) == API.AnswerWrong) {
				Debug.Log("Answer Wrong");
			}
		}
	}

	internal void ShowQuestionTable() {
		iTween.MoveTo (questionTable.gameObject, new Vector3 (questionTable.position.x, 150, 0), 2f);
	}

	internal void HideQuestionTable() {
		//_view.RPC ("PunHideQuestionTable", PhotonTargets.All);
		iTween.MoveTo (questionTable.gameObject, new Vector3 (questionTable.position.x, -100, 0), 2f);
		question = null;
	}

	[PunRPC]
	void PunHideQuestionTable() {

	}

}
