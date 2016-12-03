using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Dice : MonoBehaviour {

	public Sprite[] normalSprites = new Sprite[6];
	public Sprite[] hoverSprites = new Sprite[6];

	private Image _image;

	// Use this for initialization
	void Start () {
		_image = GetComponent<Image> ();
	}
	
	internal void Roll() {
		_image.enabled = true;
		GetComponent<Animator> ().SetBool ("isRolling", true);
	}

	internal void StopRoll() {
		_image.enabled = false;
		GetComponent<Animator> ().SetBool ("isRolling", false);
	}

	internal void SetDot(int dot) {
		GetComponent<Image> ().sprite = normalSprites [dot - 1];

		SpriteState spriteState = new SpriteState();
		spriteState.pressedSprite = hoverSprites [dot - 1];
		GetComponent<Button> ().spriteState = spriteState;
	}

}
