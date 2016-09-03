using UnityEngine;
using System.Collections;

public class PlayerTurning : MonoBehaviour {

	// Use this for initialization
	void Start () {
		TouchHandle._instance.AddEvent(RotaleLeft, RotaleRight);
	}

	void RotaleLeft() 
	{
		transform.Rotate(Vector3.up * 10f);
	}

	void RotaleRight() 
	{
		transform.Rotate(Vector3.up * -10f);
	}

}
