using UnityEngine;
using System.Collections;
using System;

public class PlayerAnimation : MonoBehaviour {

	public static PlayerAnimation _instance;

	public Transform _headSlot;
	public Transform _weaponSlot;

	private BODY_TYPE _curBody = BODY_TYPE.FAT;
	private HAIR_TYPE _curHair = HAIR_TYPE.NONE;
	private BEARD_TYPE _curBeard = BEARD_TYPE.NONE;
	private HAT_TYPE _curHat = HAT_TYPE.NONE;
	private BACKET_TYPE _curBacket = BACKET_TYPE.NONE;
	private SKIN_TYPE _curSkin = SKIN_TYPE.SKIN_1;
	private SKIN_TYPE _curFace = SKIN_TYPE.SKIN_1;

	private WEAPON_TYPE _curWeapon = WEAPON_TYPE.AXE_1;
	private string _curWeaponStr = "NPC_Tools_Axe_001";

	private COLOR_TYPE _curBeardColor = COLOR_TYPE.RED;
	private COLOR_TYPE _curHairColor = COLOR_TYPE.RED;
	private HAT_COLOR _curHatColor = HAT_COLOR.HAT_1;
	private WEAPON_COLOR _curWeaponColor = WEAPON_COLOR.COPPER;

	internal BODY_TYPE CurBody {
		get { return _curBody; }
	}

	internal HAIR_TYPE CurHair {
		get { return _curHair; }
	}

	internal SKIN_TYPE CurFace {
		get { return _curFace; }
	}

	internal BEARD_TYPE CurBeard {
		get { return _curBeard; }
	}

	internal HAT_TYPE CurHat {
		get { return _curHat; }
	}

	internal BACKET_TYPE CurBacket {
		get { return _curBacket; }
	}

	internal SKIN_TYPE CurSkin {
		get { return _curSkin; }
	}

	internal WEAPON_TYPE CurWeapon {
		get { return _curWeapon; }
	}

	internal COLOR_TYPE CurBeardColor {
		get { return _curBeardColor; }
	}

	internal COLOR_TYPE CurHairColor {
		get { return _curHairColor; }
	}

	internal HAT_COLOR CurHatColor {
		get { return _curHatColor; }
	}

	internal WEAPON_COLOR CurWeaponColor {
		get { return _curWeaponColor; }
	}

	void Awake() {
		_instance = this;
	}

	// Use this for initialization
	void Start () {
		ChangeBody (PlayerClosest._curBody);
		ChangeHair (PlayerClosest._curHair);
		ChangeBeard (PlayerClosest._curBeard);	
		ChangeHat (PlayerClosest._curHat);
		ChangeBacket (PlayerClosest._curBacket);
		ChangeSkin (PlayerClosest._curSkin);
		ChangeFace (PlayerClosest._curFace);
		ChangeWeapon (PlayerClosest._curWeapon);

		ChangeBeardColor (PlayerClosest._curBeardColor);
		ChangeHairColor (PlayerClosest._curHairColor);
		ChangeHatColor (PlayerClosest._curHatColor);
		ChangeWeaponColor (PlayerClosest._curWeaponColor);
		
	}

	/*public void ChangeBody(int type) {
		ChangeBody ((BODY_TYPE)type);
	}
	public void ChangeHair(int type) {
		ChangeHair ((HAIR_TYPE)type);
	}
	public void ChangeBeard(int type) {
		ChangeBeard ((BEARD_TYPE)type);
	}
	public void ChangeHat(int type) {
		ChangeHat ((HAT_TYPE)type);
	}
	public void ChangeBacket(int type) {
		ChangeBacket ((BACKET_TYPE)type);
	}
	public void ChangeSkin(int type) {
		ChangeSkin ((SKIN_TYPE)type);
	}
	public void ChangeFace(int type) {
		ChangeFace ((SKIN_TYPE)type);
	}
	public void ChangeWeapon(int type) {
		ChangeWeapon ((WEAPON_TYPE)type);
	}*/

	public void ChangeBody(BODY_TYPE bodyType) 
	{
		if (_curBody == bodyType)
			return;
		switch (bodyType) {
		case BODY_TYPE.FAT:
			_curBody = BODY_TYPE.FAT;
			transform.FindChild("NPC_Man_Fat").gameObject.SetActive(true);
			transform.FindChild("NPC_Man_Normal").gameObject.SetActive(false);
			transform.FindChild("NPC_Man_Skinny").gameObject.SetActive(false);
			break;
		case BODY_TYPE.NORMAL:
			_curBody = BODY_TYPE.NORMAL;
			transform.FindChild("NPC_Man_Fat").gameObject.SetActive(false);
			transform.FindChild("NPC_Man_Normal").gameObject.SetActive(true);
			transform.FindChild("NPC_Man_Skinny").gameObject.SetActive(false);
			break;
		case BODY_TYPE.SKINNY:
			_curBody = BODY_TYPE.SKINNY;
			transform.FindChild("NPC_Man_Fat").gameObject.SetActive(false);
			transform.FindChild("NPC_Man_Normal").gameObject.SetActive(false);
			transform.FindChild("NPC_Man_Skinny").gameObject.SetActive(true);
			break;
		}
	}

	public void ChangeHair(HAIR_TYPE hairType) {
		if (_curHair == hairType)
			return;
		if (_curHair != HAIR_TYPE.NONE) 
		{
			_headSlot.FindChild ("NPC_Hair_00" + (int)_curHair).gameObject.SetActive (false);
		}
		_curHair = hairType;

		if (hairType == HAIR_TYPE.NONE) 
			return;

		_headSlot.FindChild ("NPC_Hair_00" + (int)hairType).gameObject.SetActive(true);
	}

	public void ChangeBeard(BEARD_TYPE beardType) {
		if (_curBeard == beardType)
			return;
		if (_curBeard != BEARD_TYPE.NONE) 
		{
			_headSlot.FindChild ("NPC_Beard_00" + (int)_curBeard).gameObject.SetActive (false);
		}
		_curBeard = beardType;

		if (beardType == BEARD_TYPE.NONE)
			return;

		_headSlot.FindChild ("NPC_Beard_00" + (int)beardType).gameObject.SetActive(true);
	}

	public void ChangeHat(HAT_TYPE hatType) {
		if (_curHat == hatType)
			return;
		if (_curHat != HAT_TYPE.NONE) 
		{
			_headSlot.FindChild ("NPC_Hat_00" + (int)_curHat).gameObject.SetActive (false);
		}
		_curHat = hatType;

		if (hatType == HAT_TYPE.NONE) 
			return;

		_headSlot.FindChild ("NPC_Hat_00" + (int)hatType).gameObject.SetActive(true);
	}

	public void ChangeBacket(BACKET_TYPE backetType) {
		if (_curBacket == backetType)
			return;
		if (_curBacket != BACKET_TYPE.NONE) 
		{
			transform.FindChild ("NPC_Backet_00" + (int)_curBacket).gameObject.SetActive (false);
		}
		_curBacket = backetType;

		if (backetType == BACKET_TYPE.NONE) 
			return;

		transform.FindChild ("NPC_Backet_00" + (int)backetType).gameObject.SetActive(true);
	}

	public void ChangeSkin(SKIN_TYPE skinType) {
		if (_curSkin == skinType)
			return;
		Material mat = PlayerClosest.Get_Material (CLOSEST_TYPE.SKIN, (int)skinType);
		Material[] matArr = new Material[] {
			mat,
			transform.FindChild ("NPC_Man_Fat").GetComponent<SkinnedMeshRenderer> ().materials [1]
		};
		if (mat != null) 
		{
			_curSkin = skinType;
			transform.FindChild ("NPC_Man_Fat").GetComponent<SkinnedMeshRenderer> ().materials = matArr;
			transform.FindChild ("NPC_Man_Normal").GetComponent<SkinnedMeshRenderer> ().materials = matArr;
			transform.FindChild ("NPC_Man_Skinny").GetComponent<SkinnedMeshRenderer> ().materials = matArr;
		}
	}

	public void ChangeFace(SKIN_TYPE faceType) {
		if (_curFace == faceType)
			return;
		Material mat = PlayerClosest.Get_Material (CLOSEST_TYPE.FACE, (int)faceType);
		Material[] matArr = new Material[] {
			transform.FindChild ("NPC_Man_Fat").GetComponent<SkinnedMeshRenderer> ().materials [0],
			mat
		};
		if (mat != null)
		{
			_curFace = faceType;
			transform.FindChild ("NPC_Man_Fat").GetComponent<SkinnedMeshRenderer> ().materials = matArr;
			transform.FindChild ("NPC_Man_Normal").GetComponent<SkinnedMeshRenderer> ().materials = matArr;
			transform.FindChild ("NPC_Man_Skinny").GetComponent<SkinnedMeshRenderer> ().materials = matArr;
		}
	}

	public void ChangeWeapon(WEAPON_TYPE weaponType) {
		if (_curWeapon == weaponType)
			return;
		_weaponSlot.FindChild (_curWeaponStr).gameObject.SetActive (false);
		_curWeapon = weaponType;

		switch (_curWeapon) {
		case WEAPON_TYPE.AXE_1:
			_curWeaponStr = "NPC_Tools_Axe_001";
			break;
		case WEAPON_TYPE.AXE_2:
			_curWeaponStr = "NPC_Tools_Axe_002";
			break;
		case WEAPON_TYPE.AXE_3:
			_curWeaponStr = "NPC_Tools_Axe_003";
			break;
		case WEAPON_TYPE.HAMMER:
			_curWeaponStr = "NPC_Tools_Hammer_01";
			break;
		case WEAPON_TYPE.PICK:
			_curWeaponStr = "NPC_Tools_Pick_01";
			break;
		case WEAPON_TYPE.SAW:
			_curWeaponStr = "NPC_Tools_Saw_001";
			break;
		case WEAPON_TYPE.SHOVEL:
			_curWeaponStr = "NPC_Tools_Shovel_001";
			break;
		}
		_weaponSlot.FindChild (_curWeaponStr).gameObject.SetActive (true);
	}

	public void ChangeBeardColor(COLOR_TYPE colorType) {
		if (_curBeardColor == colorType)
			return;
		if (_curBeard != BEARD_TYPE.NONE) 
		{
			_curBeardColor = colorType;
			MeshRenderer mr = _headSlot.FindChild ("NPC_Beard_00" + (int)_curBeard).GetComponent<MeshRenderer>();
			mr.materials = new Material[]{PlayerClosest.Get_Material(CLOSEST_TYPE.BEARD, (int)colorType)};
		}
	}

	public void ChangeHairColor(COLOR_TYPE colorType) {
		if (_curHair != HAIR_TYPE.NONE) 
		{
			_curHairColor = colorType;
			MeshRenderer mr = _headSlot.FindChild ("NPC_Hair_00" + (int)_curHair).GetComponent<MeshRenderer>();
			mr.materials = new Material[]{PlayerClosest.Get_Material(CLOSEST_TYPE.HAIR, (int)colorType)};
		}
	}

	public void ChangeHatColor(HAT_COLOR colorType) {
		if (_curHat != HAT_TYPE.NONE) 
		{
			_curHatColor = colorType;
			MeshRenderer mr = _headSlot.FindChild ("NPC_Hat_00" + (int)_curHat).GetComponent<MeshRenderer>();
			mr.materials = new Material[]{PlayerClosest.Get_Material(CLOSEST_TYPE.HAT, (int)colorType)};
		}
	}

	public void ChangeWeaponColor(WEAPON_COLOR colorType) {
		_curWeaponColor = colorType;
		MeshRenderer mr = _weaponSlot.FindChild (_curWeaponStr).GetComponent<MeshRenderer>();
		mr.materials = new Material[]{PlayerClosest.Get_Material(CLOSEST_TYPE.WEAPON, (int)colorType)};
	}

}
