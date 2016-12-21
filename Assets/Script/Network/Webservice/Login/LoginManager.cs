using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using SimpleJSON;
using System.Net;
using System.Net.Sockets;

public class LoginManager : MonoBehaviour {

	public InputField usernameField;
	public InputField passwordField;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Login() {
		StartCoroutine("CoLogin");
	}

	public void Register() {
		StartCoroutine("CoRegister");
	}

	private IEnumerator CoRegister() {
		WWWForm form = new WWWForm ();
		form.AddField ("Username", usernameField.text);
		form.AddField ("Password", passwordField.text);
		
		WWW w = new WWW (GameConfig.REGISTER_URL, form);
		while (!w.isDone) {
			yield return new WaitForEndOfFrame();
		}
		if ((API)int.Parse (w.text) == API.UsernameHasAlreadyExist) {
			Debug.Log ("Username has already exist");
		} else if ((API)int.Parse (w.text) == API.RegisterFailed) {
			Debug.Log ("Register failed");
		} else {
			Debug.Log ("Register successful");
		}
	}

	private IEnumerator CoLogin() {
		WWWForm form = new WWWForm ();
		form.AddField ("Username", usernameField.text);
		form.AddField ("Password", passwordField.text);
		
		WWW w = new WWW (GameConfig.LOGIN_URL, form);
		while (!w.isDone) {
			yield return new WaitForEndOfFrame();
		}

		string jsonStr = w.text;
		Debug.Log (jsonStr);
		JSONNode node = JSON.Parse (jsonStr);
		if (jsonStr != "" && (API)int.Parse (node["api"]) == API.LoginSuccess) {
			PlayerInfo.id = node["id"];
			Debug.Log ("Login Success");
			if (node["closest"] == null) {
				Application.LoadLevel("DesignCharacter");
			} else {
				PlayerClosest.ChangeClosest(node["closest"]);
				Application.LoadLevel("Lobby");
			}
		} else {
			Debug.Log ("Login failed");
		}
	}

}
