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

	private void CoRegister() {
		WWWForm form = new WWWForm ();
		form.AddField ("Username", usernameField.text);
		form.AddField ("Password", passwordField.text);
		
		WWW w = new WWW ("http://192.168.1.74/MasterOfEducation/Login/Register.php", form);
		while (!w.isDone) {
			
		}
		if ((API)int.Parse (w.text) == API.UsernameHasAlreadyExist) {
			Debug.Log ("Username has already exist");
		} else {
			Debug.Log ("Register successful");
		}
	}

	private void CoLogin() {
		WWWForm form = new WWWForm ();
		form.AddField ("Username", usernameField.text);
		form.AddField ("Password", passwordField.text);
		
		WWW w = new WWW ("http://192.168.1.74/MasterOfEducation/Login/Login.php", form);
		while (!w.isDone) {
			
		}

		string jsonStr = w.text;
		Debug.Log (jsonStr);
		JSONNode node = JSON.Parse (jsonStr);
		if (jsonStr != "" && (API)int.Parse (node["api"]) == API.LoginSuccess) {
			PlayerInfo.id = node["id"];
			Debug.Log ("Login Success");
			Application.LoadLevel("DesignCharacter");
		} else {
			Debug.Log ("Login failed");
		}
	}

}
