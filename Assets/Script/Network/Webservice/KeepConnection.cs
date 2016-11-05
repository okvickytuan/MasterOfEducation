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

			WWW w = new WWW (GameConfig.KEEPCONNECTION_URL, form);
			while (!w.isDone) {
				yield return new WaitForEndOfFrame();
			}
			yield return new WaitForSeconds(5);
		}
	}

}
