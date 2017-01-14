using UnityEngine;
using System.Collections;

public class Owl : Animal {

	// Use this for initialization
	protected override void Start () {
		base.Start ();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	internal override void GoToRoad() {
		_isInCage = false;
		_destination = new int[2] { this._playerIndex + 1, 1 };
		_view.RPC ("AppearTo", PhotonTargets.All, this._playerIndex + 1);
		//MoveComplete ();
	}

	//Bien ra vi tri dau tien cua chuong (xuat chuong)
	[PunRPC]
	private void AppearTo(int area) {

		Transform des = _way.FindChild ("" + area).FindChild ("1");
		GameController._instance.debug.text = "" + area;
		//Nếu có thú thì đá
		Animal animal = GetAnimalInArea (area, 1);
		bool isKicked = false;
		if (animal != null) {
			isKicked = true;
			StartCoroutine(Kick(animal));
		}

		StartCoroutine (GoOutCage(des, isKicked));
	}

	private IEnumerator GoOutCage(Transform des, bool isKicked) {
		if (isKicked == true) {
			yield return new WaitForSeconds (2.0f);
		}

		transform.parent = des;
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;

		GameEffectManager._instance.CreateEffect ("Prefabs/Particle/healing", transform.parent.position, 2.0f);
		SoundManager._instance.PlayEffect (SoundConfig.OUT_OF_CAGE_PATH, 1.0f);
		yield return new WaitForSeconds (2.0f);

		MoveComplete ();
	}

	[PunRPC]
	protected override void JumpTo(int area, int areaIndex) {
		Transform des = _way.FindChild ("" + area).FindChild ("" + areaIndex);
		
		//Nếu có thú thì đá
		Animal animal = GetAnimalInArea (area, areaIndex);
		bool isKicked = false;
		if (animal != null) {
			isKicked = true;
			StartCoroutine(Kick(animal));
		}

		transform.parent = des;
		StartCoroutine(JumpAnim(1f, Vector3.zero, 0.4f, isKicked));
	}

	private IEnumerator JumpAnim(float jumpHeight, Vector3 dest, float time, bool isKicked) {
		if (isKicked == true) {
			yield return new WaitForSeconds (2.0f);
		}

		Vector3 startPos = transform.localPosition;
		float timer = 0.0f;
		
		while (timer <= 1.0) {
			var height = Mathf.Sin(Mathf.PI * timer) * jumpHeight;
			transform.localPosition = Vector3.Lerp(startPos, dest, timer) + Vector3.up * height; 
			
			timer += Time.deltaTime / time;
			yield return new WaitForFixedUpdate();
		}
		transform.parent.FindChild ("JumpSmoke").GetComponent<ParticleSystem>().Play();
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;

		SoundManager._instance.PlayEffect (SoundConfig.JUMP_PATH, 1.0f);
	}
}
