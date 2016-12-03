using UnityEngine;
using System.Collections;
using SimpleJSON;
using UnityEngine.UI;

public class DiceManager : MonoBehaviour {

	public Dice diceButton;
	public Dice diceRoll;

	private PhotonView _view;

	void Awake() {

	}

	// Use this for initialization
	void Start () {
		_view = GetComponent<PhotonView> ();
	}

	public void RollDice() {
		StartCoroutine("CoRollDice");
	}

	private IEnumerator CoRollDice() {
		diceRoll.Roll ();
		diceButton.gameObject.SetActive (false);
		WWW w = new WWW (GameConfig.ROLL_DICE_URL);
		while (!w.isDone) {
			yield return new WaitForEndOfFrame();
		}
		yield return new WaitForSeconds (1.5f);
		diceRoll.StopRoll ();
		diceButton.gameObject.SetActive (true);
		if (w.text != "") {
			JSONNode node = JSON.Parse (w.text);
			if (node["dot"] != null && node["dot"].AsInt >= 1 && node["dot"].AsInt <= 6) {
				int dot = node["dot"].AsInt;
				diceButton.SetDot(dot);
			}
		}
	}

}
