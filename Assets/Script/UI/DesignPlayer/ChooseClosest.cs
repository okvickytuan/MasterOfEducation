using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class ChooseClosest : MonoBehaviour {

	public Sprite _normalButton;
	public Sprite _hoverButton;

	private CLOSEST_TYPE _type;
	private int index;
	private Image _img;

	void Start() {
		index = transform.GetSiblingIndex ();
		_img = GetComponent<Image> ();
	}

	public void ChangeClosest(string closestType) {
		_type = (CLOSEST_TYPE) Enum.Parse (typeof(CLOSEST_TYPE), closestType);
		int tmp;
		switch (_type) {
		case CLOSEST_TYPE.HAIR:
			tmp = (int)PlayerAnimation._instance.CurHair;
			VisibleOldClosest(tmp);
			PlayerAnimation._instance.ChangeHair((HAIR_TYPE)index);
			break;
		case CLOSEST_TYPE.FACE:
			tmp = (int)PlayerAnimation._instance.CurFace;
			VisibleOldClosest(tmp-1);
			PlayerAnimation._instance.ChangeFace((SKIN_TYPE)(index+1));
			break;
		case CLOSEST_TYPE.BODY:
			tmp = (int)PlayerAnimation._instance.CurBody;
			VisibleOldClosest(tmp);
			PlayerAnimation._instance.ChangeBody((BODY_TYPE)index);
			break;
		case CLOSEST_TYPE.BEARD:
			tmp = (int)PlayerAnimation._instance.CurBeard;
			VisibleOldClosest(tmp);
			PlayerAnimation._instance.ChangeBeard((BEARD_TYPE)index);
			break;
		case CLOSEST_TYPE.HAT:
			tmp = (int)PlayerAnimation._instance.CurHat;
			VisibleOldClosest(tmp);
			PlayerAnimation._instance.ChangeHat((HAT_TYPE)index);
			break;
		case CLOSEST_TYPE.BACKET:
			tmp = (int)PlayerAnimation._instance.CurBacket;
			VisibleOldClosest(tmp);
			PlayerAnimation._instance.ChangeBacket((BACKET_TYPE)index);
			break;
		case CLOSEST_TYPE.SKIN:
			tmp = (int)PlayerAnimation._instance.CurSkin;
			VisibleOldClosest(tmp-1);
			PlayerAnimation._instance.ChangeSkin((SKIN_TYPE)(index+1));
			break;
		case CLOSEST_TYPE.WEAPON:
			tmp = (int)PlayerAnimation._instance.CurWeapon;
			VisibleOldClosest(tmp);
			PlayerAnimation._instance.ChangeWeapon((WEAPON_TYPE)index);
			break;
		}
	}

	public void ChangeClosestColor(string colorType) {
		_type = (CLOSEST_TYPE)Enum.Parse (typeof(CLOSEST_TYPE), transform.parent.parent.name.ToUpper());
		switch (_type) {
		case CLOSEST_TYPE.HAIR:
			COLOR_TYPE tmp_1 = (COLOR_TYPE)Enum.Parse (typeof(COLOR_TYPE), colorType);
			if (index == (int)PlayerAnimation._instance.CurHair && tmp_1 == PlayerAnimation._instance.CurHairColor)
				return;
			PlayerAnimation._instance.ChangeHairColor(tmp_1);
			break;
		case CLOSEST_TYPE.BEARD:
			COLOR_TYPE tmp_2 = (COLOR_TYPE)Enum.Parse (typeof(COLOR_TYPE), colorType);
			if (index == (int)PlayerAnimation._instance.CurBeard && tmp_2 == PlayerAnimation._instance.CurBeardColor)
				return;
			PlayerAnimation._instance.ChangeBeardColor(tmp_2);
			break;
		case CLOSEST_TYPE.HAT:
			HAT_COLOR tmp_3 = (HAT_COLOR)Enum.Parse (typeof(HAT_COLOR), colorType);
			if (index == (int)PlayerAnimation._instance.CurHat && tmp_3 == PlayerAnimation._instance.CurHatColor)
				return;
			PlayerAnimation._instance.ChangeHatColor(tmp_3);
			break;
		case CLOSEST_TYPE.WEAPON:
			WEAPON_COLOR tmp_4 = (WEAPON_COLOR)Enum.Parse (typeof(WEAPON_COLOR), colorType);
			if (index == (int)PlayerAnimation._instance.CurWeapon && tmp_4 == PlayerAnimation._instance.CurWeaponColor)
				return;
			PlayerAnimation._instance.ChangeWeaponColor(tmp_4);
			break;
		}
	}

	private void VisibleOldClosest(int closestType) {
		if (index == closestType)
			return;
		Transform oldTab = transform.parent.GetChild (closestType);
		if (_type == CLOSEST_TYPE.HAIR || _type == CLOSEST_TYPE.BEARD || 
		    _type == CLOSEST_TYPE.HAT || _type == CLOSEST_TYPE.WEAPON) 
		{
			oldTab.FindChild ("Color").gameObject.SetActive (false);
			transform.FindChild ("Color").gameObject.SetActive (true);
		}

		oldTab.GetComponent<Image> ().sprite = _normalButton;
		_img.sprite = _hoverButton;
	}

}
