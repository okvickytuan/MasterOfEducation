using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIEffectManager : MonoBehaviour {

	internal static UIEffectManager _instance;

	private PhotonView _view;
	private RectTransform canvas;

	private RectTransform playerNameUI;

	void Awake() {
		_instance = this;
	}

	// Use this for initialization
	void Start () {
		_view = GetComponent<PhotonView> ();
		canvas = GameObject.Find ("Canvas").GetComponent<RectTransform>();

		foreach (RectTransform tf in canvas) {
			if (tf.name == "PlayerName") {
				playerNameUI = tf;
			}
		}

		_view.RPC ("PunShowPlayerName", PhotonTargets.All, PUNManager._instance.PlayerIndex, PlayerInfo.username);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	[PunRPC]
	private void PunShowPlayerName(int playerIndex, string playerName) {
		int playerUI = GetPlayerUI (playerIndex + 1);
		foreach (RectTransform tf in playerNameUI) {
			if (tf.name == ("" + playerUI)) {
				tf.FindChild("Text").GetComponent<Text>().text = playerName;
			}
		}
	}

	internal void ShowPlayerTurnEffect(int playerTurn) {
		_view.RPC ("PunShowPlayerTurnEffect", PhotonTargets.All, playerTurn);
	}

	[PunRPC]
	private void PunShowPlayerTurnEffect(int playerTurn) {
		int playerUI = GetPlayerUI (playerTurn);

		for (int i=0; i<4; i++) {
			playerNameUI.FindChild("border_" + (i+1)).gameObject.SetActive((i+1) == playerUI);
		}
	}

	private int GetPlayerUI(int player) {
		int result;
		result = player - PUNManager._instance.PlayerIndex;
		if (result < 1) {
			result = 4 + result;
		}
		return result;
	}

}

