using UnityEngine;
using System.Collections;
using SimpleJSON;
using UnityEngine.UI;

public class QuestionManager : MonoBehaviour {

	public RectTransform questionTable;

	private Question question = null;

	// Use this for initialization
	void Start () {
		ResponseQuestion ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	internal void ResponseQuestion() {
		
		WWW w = new WWW ("http://192.168.1.74/MasterOfEducation/Question/Question.php");
		while (!w.isDone) {
			
		}
		if (w.text != "") {
			JSONNode node = JSON.Parse (w.text);
			if (node["api"] != null && (API)int.Parse(node["api"]) == API.DatabaseCannotConnect) {
				Debug.LogError("Failed to connect to database");
			} else {
				question = new Question();
				question.Id = node["qid"].AsInt;
				question.Ques = node["question_Vi"];
				question.AnswerA = node["answer_A_Vi"];
				question.AnswerB = node["answer_B_Vi"];
				question.AnswerC = node["answer_C_Vi"];
				question.AnswerD = node["answer_D_Vi"];
				question.addToTable(questionTable);
			}
		}
	}

	public void RequestAnswer(string answer) {
		WWWForm form = new WWWForm ();
		form.AddField ("qid", question.Id);
		form.AddField ("answer", answer);

		WWW w = new WWW ("http://192.168.1.74/MasterOfEducation/Question/CheckAnswer.php", form);
		while (!w.isDone) {
			
		}
		Debug.Log (w.text);
		if (w.text != "") {
			JSONNode node = JSON.Parse (w.text);
			if ((API)int.Parse(node["api"]) == API.DatabaseCannotConnect) {
				Debug.LogError("Failed to connect to database");
			} else if ((API)int.Parse(node["api"]) == API.AnswerCorrect) {
				Debug.Log("Answer Correct");
				HideQuestionTable();
			} else if ((API)int.Parse(node["api"]) == API.AnswerWrong) {
				Debug.Log("Answer Wrong");
			}
		}
	}

	internal void ShowQuestionTable() {
		iTween.MoveTo (questionTable.gameObject, new Vector3 (questionTable.position.x, 150, 0), 2f);
	}

	internal void HideQuestionTable() {
		iTween.MoveTo (questionTable.gameObject, new Vector3 (questionTable.position.x, -100, 0), 2f);
	}

}
