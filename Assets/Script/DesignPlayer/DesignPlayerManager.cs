using UnityEngine;
using System.Collections;
using SimpleJSON;

public class DesignPlayerManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	public void ChangeClosest() {
		/*PlayerClosest._curBody = PlayerAnimation._instance.CurBody;
		PlayerClosest._curHair = PlayerAnimation._instance.CurHair;
		PlayerClosest._curBeard = PlayerAnimation._instance.CurBeard;
		PlayerClosest._curHat = PlayerAnimation._instance.CurHat;
		PlayerClosest._curBacket = PlayerAnimation._instance.CurBacket;
		PlayerClosest._curSkin = PlayerAnimation._instance.CurSkin;
		PlayerClosest._curFace = PlayerAnimation._instance.CurFace;
		PlayerClosest._curWeapon = PlayerAnimation._instance.CurWeapon;

		PlayerClosest._curBeardColor = PlayerAnimation._instance.CurBeardColor;
		PlayerClosest._curHairColor = PlayerAnimation._instance.CurHairColor;
		PlayerClosest._curHatColor = PlayerAnimation._instance.CurHatColor;
		PlayerClosest._curWeaponColor = PlayerAnimation._instance.CurWeaponColor;*/

		StartCoroutine ("CoChangeClosest");
	}

	IEnumerator CoChangeClosest() {
		WWWForm form = new WWWForm ();
		form.AddField ("Id", PlayerInfo.id);
		form.AddField ("Body", (int)PlayerAnimation._instance.CurBody);
		form.AddField ("Hair", (int)PlayerAnimation._instance.CurHair);
		form.AddField ("Face", (int)PlayerAnimation._instance.CurFace);
		form.AddField ("Beard", (int)PlayerAnimation._instance.CurBeard);
		form.AddField ("Hat", (int)PlayerAnimation._instance.CurHat);
		form.AddField ("Backet", (int)PlayerAnimation._instance.CurBacket);
		form.AddField ("Skin", (int)PlayerAnimation._instance.CurSkin);
		form.AddField ("Weapon", (int)PlayerAnimation._instance.CurWeapon);

		form.AddField ("HairColor", (int)PlayerAnimation._instance.CurHairColor);
		form.AddField ("BeardColor", (int)PlayerAnimation._instance.CurBeardColor);
		form.AddField ("HatColor", (int)PlayerAnimation._instance.CurHatColor);
		form.AddField ("WeaponColor", (int)PlayerAnimation._instance.CurWeaponColor);
		
		WWW w = new WWW (GameConfig.CHANGE_CLOSEST_URL, form);
		while (!w.isDone) {
			yield return new WaitForEndOfFrame();
		}
		if (w.text != "") {
			JSONNode node = JSON.Parse (w.text);
			if (node["api"] != null) {
				if ((API)int.Parse(node["api"]) == API.ChangeClosestSuccess) {
					Debug.Log("Changed Closest");
					PlayerClosest.ChangeClosest();
					Application.LoadLevel("Lobby");
				} else {
					Debug.Log("Change Closest Failed");
				}
			} else {
				Debug.Log("Change Closest Failed");
			}
		}
	}

}
