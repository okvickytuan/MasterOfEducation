using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Question {

	private int id;
	private string question;
	private string answerA;
	private string answerB;
	private string answerC;
	private string answerD;
	private float time;

	internal int Id {
		set { id = value; }
		get { return id; }
	}

	internal string Ques {
		set { question = value; }
	}

	internal string AnswerA {
		set { answerA = value; }
	}

	internal string AnswerB {
		set { answerB = value; }
	}

	internal string AnswerC {
		set { answerC = value; }
	}

	internal string AnswerD {
		set { answerD = value; }
	}

	internal float Time {
		get { return time; }
		set { time = value; }
	}

	internal void addToTable(RectTransform table) {
		table.FindChild("Question").FindChild("Text").GetComponent<Text>().text = this.question;
		table.FindChild("Answer_A").GetComponentInChildren<Text>().text = "A." + this.answerA;
		table.FindChild("Answer_B").GetComponentInChildren<Text>().text = "B." + this.answerB;
		table.FindChild("Answer_C").GetComponentInChildren<Text>().text = "C." + this.answerC;
		table.FindChild("Answer_D").GetComponentInChildren<Text>().text = "D." + this.answerD;
	}

}
