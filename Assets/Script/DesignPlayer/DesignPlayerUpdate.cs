using UnityEngine;
using System.Collections;

public class DesignPlayerUpdate : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	public void UpdatePlayerClosest() {
		PlayerClosest._curBody = PlayerAnimation._instance.CurBody;
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
		PlayerClosest._curWeaponColor = PlayerAnimation._instance.CurWeaponColor;
	}

	public void GoToLobby() {
		UpdatePlayerClosest ();
		Application.LoadLevel("Lobby");
	}

}
