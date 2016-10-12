using UnityEngine;
using System.Collections;

public class KeepConnection : MonoBehaviour {

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (gameObject);
		StartCoroutine (SendRequest());
		//SendRequest ();
	}

	IEnumerator SendRequest() {
		while (true) {
			WWWForm form = new WWWForm ();
			form.AddField ("Id", PlayerInfo.id);

			WWW w = new WWW ("http://localhost/MasterOfEducation/Connecting/KeepConnection.php", form);
			while (!w.isDone) {
			
			}
			yield return new WaitForSeconds(5);
		}
	}

}
