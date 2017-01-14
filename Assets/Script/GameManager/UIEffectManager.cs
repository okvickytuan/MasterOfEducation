using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIEffectManager : MonoBehaviour {

	internal static UIEffectManager _instance;

	public GameObject _winLose;

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

	internal void HidePlayerTurnEffect() {
		_view.RPC ("PunHidePlayerTurnEffect", PhotonTargets.All);
	}

	[PunRPC]
	internal void PunShowPlayerTurnEffect(int playerTurn) {
		int playerUI = GetPlayerUI (playerTurn);

		for (int i=0; i<4; i++) {
			playerNameUI.FindChild("border_" + (i+1)).gameObject.SetActive((i+1) == playerUI);
		}

		if (playerTurn == PUNManager._instance.PlayerIndex + 1) {
			GameController._instance.PlayerAnimal.ShowArrow();
		} else {
			GameController._instance.PlayerAnimal.HideArrow();
		}
	}

	[PunRPC]
	private void PunHidePlayerTurnEffect() {
		for (int i=0; i<4; i++) {
			playerNameUI.FindChild("border_" + (i+1)).gameObject.SetActive(false);
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

	internal void ShowWinLose(bool isWin) {
		_winLose.SetActive (true);
		_winLose.transform.FindChild (isWin ? "Victory" : "Defeat").gameObject.SetActive (true);

		SoundManager._instance.PlayEffect (isWin ? SoundConfig.VICTORY_PATH : SoundConfig.DEFEAT_PATH, 1.0f);
		StartCoroutine ("CoShowContinueButton");
	}

	private IEnumerator CoShowContinueButton() {
		yield return new WaitForSeconds (1.5f);
		_winLose.transform.FindChild ("Continue").gameObject.SetActive (true);
		/*float timer = 0;
		float time = 1.0f;
		Image button = _winLose.transform.FindChild ("Continue").GetComponent<UnityEngine.UI.Image> ();

		while (timer >= time) {
			float offset = Time.deltaTime / time * 255.0f;
			Color tmp = button.color;
			tmp.a = tmp.a + (offset > 255.0f ? 255.0f : offset);
			button.color = tmp;
			
			timer += Time.deltaTime;
			yield return new WaitForFixedUpdate();
		}*/
	}

}

