using UnityEngine;
using System.Collections;
using SimpleJSON;
using UnityEngine.UI;

public class DiceManager : MonoBehaviour {

	internal static DiceManager _instance;

	public Dice diceButton;
	public Dice diceRoll;

	internal bool _canRoll = false;
	internal bool _isRolling = false;

	private PhotonView _view;
	private int _currentDot = 0;

	private event del_one_integer_param _rollDoneEvt = null;

	internal int CurrentDot {
		get { return _currentDot; }
	}

	void Awake() {
		DiceManager._instance = this;
	}

	// Use this for initialization
	void Start () {
		_view = GetComponent<PhotonView> ();
	}

	internal void AddRollDoneEvent(del_one_integer_param rollDoneEvent) {
		_rollDoneEvt = rollDoneEvent;
	}

	public void RollDice() {
		CancelInvoke ("RollDice");
		if (_canRoll == true && _isRolling == false) {
			StartCoroutine ("CoRollDice");
		}
	}

	private IEnumerator CoRollDice() {
		BeginRoll (PhotonTargets.All);
		WWW w = new WWW (GameConfig.ROLL_DICE_URL);
		while (!w.isDone) {
			yield return new WaitForEndOfFrame();
		}
		yield return new WaitForSeconds (1.0f);
		diceRoll.StopRoll ();
		diceButton.gameObject.SetActive (true);
		if (w.text != "" && _canRoll) {
			JSONNode node = JSON.Parse (w.text);
			if (node["dot"] != null && node["dot"].AsInt >= 1 && node["dot"].AsInt <= 6) {
				int dot = node["dot"].AsInt;
				_currentDot = dot;
				diceButton.SetDot(dot);
				_canRoll = false;

				if (_rollDoneEvt != null) {
					_rollDoneEvt(dot);
				}

			}
		}
		_isRolling = false;
	}

	[PunRPC]
	internal void BeginRollAnim() {
		_isRolling = true;
		diceRoll.Roll ();
		diceButton.gameObject.SetActive (false);
	}

	[PunRPC]
	internal void StopRollAnim(int dot) {
		diceRoll.StopRoll ();
		diceButton.gameObject.SetActive (true);
		_currentDot = dot;
		diceButton.SetDot(dot);
		_canRoll = false;
		_isRolling = false;

		SoundManager._instance.PlayEffect (SoundConfig.ROLL_DONE_PATH, 1.0f);
	}

	internal void AutoRoll() {
		Invoke ("RollDice", 5);
	}

	internal void StopRoll(int dot, PhotonTargets target) {
		_view.RPC ("StopRollAnim", target, dot);
	}

	internal void BeginRoll(PhotonTargets target) {
		_view.RPC ("BeginRollAnim", target);
	}

}
