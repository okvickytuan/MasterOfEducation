using UnityEngine;
using System.Collections;

public enum CLOSEST_TYPE {
	BODY,
	HAIR,
	FACE,
	BEARD,
	HAT,
	BACKET,
	SKIN,
	WEAPON
}

public enum COLOR_TYPE {
	RED = 1,
	YELLOW = 2,
	GREEN = 3,
	GRAY = 4,
	BLUE = 5,
	WHITE = 6,
	PURPLE = 7,
	BLACK = 8,
	BROWN_HARD = 9,
	BROWN = 10
}

public enum WEAPON_COLOR {
	COPPER = 1,
	SILVER = 2,
	GOLD = 3
}

public enum HAT_COLOR {
	HAT_1 = 1,
	HAT_2 = 2,
	HAT_3 = 3,
	HAT_4 = 4,
	HAT_5 = 5,
	HAT_6 = 6,
	HAT_7 = 7
}

public enum BEARD_TYPE {
	NONE = 0,
	BEARD_1 = 1,
	BEARD_2 = 2,
	BEARD_3 = 3,
	BEARD_4 = 4,
	BEARD_5 = 5,
	BEARD_6 = 6,
	BEARD_7 = 7
}

public enum HAIR_TYPE {
	NONE = 0,
	HAIR_1 = 1,
	HAIR_2 = 2,
	HAIR_3 = 3,
	HAIR_4 = 4,
	HAIR_5 = 5,
	HAIR_6 = 6,
	HAIR_7 = 7,
	HAIR_8 = 8
}

public enum HAT_TYPE {
	NONE = 0,
	HAT_1 = 1,
	HAT_2 = 2,
	HAT_3 = 3,
	HAT_4 = 4,
	HAT_5 = 5,
	HAT_6 = 6
}

public enum BACKET_TYPE {
	NONE = 0,
	BACKET_1 = 1,
	BACKET_2 = 2,
	BACKET_3 = 3,
	BACKET_4 = 4,
	BACKET_5 = 5,
	BACKET_6 = 6
}

public enum SKIN_TYPE {
	SKIN_1 = 1,
	SKIN_2 = 2,
	SKIN_3 = 3,
	SKIN_4 = 4,
	SKIN_5 = 5,
	SKIN_6 = 6,
	SKIN_7 = 7,
	SKIN_8 = 8,
	SKIN_9 = 9,
	SKIN_10 = 10,
	SKIN_11 = 11,
	SKIN_12 = 12,
	SKIN_13 = 13
}

public enum BODY_TYPE {
	FAT = 0,
	NORMAL = 1,
	SKINNY = 2
}

public enum WEAPON_TYPE {
	AXE_1 = 0,
	AXE_2 = 1,
	AXE_3 = 2,
	HAMMER = 3,
	PICK = 4,
	SAW = 5,
	SHOVEL = 6
}

public class PlayerClosest {

	private static string COLOR_PATH = "Material/Color/";

	internal static int NUMBER_OF_HAIR = 8;

	public static Material Get_Material(CLOSEST_TYPE closestType, int index) 
	{
		switch (closestType) {
			case CLOSEST_TYPE.HAT:
				return Resources.Load("Material/Hat/NPC_Hat_" + index) as Material;
			case CLOSEST_TYPE.SKIN:
			case CLOSEST_TYPE.FACE:
				return Resources.Load("Material/Closest/NPC_Man_" + index) as Material;
			case CLOSEST_TYPE.BEARD:
				return Resources.Load(COLOR_PATH + "Beard/NPC_Beard_" + index) as Material;
			case CLOSEST_TYPE.HAIR:
				return Resources.Load(COLOR_PATH + "Hair/NPC_Hair_" + index) as Material;
			case CLOSEST_TYPE.WEAPON:
				return Resources.Load(COLOR_PATH + "Weapon/NPC_Tools_0" + index) as Material;
		}
		return null;
	}

	internal static BODY_TYPE _curBody = BODY_TYPE.FAT;
	internal static HAIR_TYPE _curHair = HAIR_TYPE.NONE;
	internal static BEARD_TYPE _curBeard = BEARD_TYPE.NONE;
	internal static HAT_TYPE _curHat = HAT_TYPE.NONE;
	internal static BACKET_TYPE _curBacket = BACKET_TYPE.NONE;
	internal static SKIN_TYPE _curSkin = SKIN_TYPE.SKIN_1;
	internal static SKIN_TYPE _curFace = SKIN_TYPE.SKIN_1;
	
	internal static WEAPON_TYPE _curWeapon = WEAPON_TYPE.AXE_1;
	internal static string _curWeaponStr = "NPC_Tools_Axe_001";
	
	internal static COLOR_TYPE _curBeardColor = COLOR_TYPE.RED;
	internal static COLOR_TYPE _curHairColor = COLOR_TYPE.RED;
	internal static HAT_COLOR _curHatColor = HAT_COLOR.HAT_1;
	internal static WEAPON_COLOR _curWeaponColor = WEAPON_COLOR.COPPER;

}
