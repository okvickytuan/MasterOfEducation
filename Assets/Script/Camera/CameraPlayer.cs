using UnityEngine;
using System.Collections;

[System.Serializable]
public class CameraPlayer : MonoBehaviour {

	public Vector3[] _position;
	public Quaternion[] _rotation;

	internal static CameraPlayer _instance;
	
	public Vector3[] PosPlayer
	{ 
		get { return _position; }
		set { _position = value; }
	}

	public Quaternion[] RotPlayer
	{ 
		get { return _rotation; }
		set { _rotation = value; }
	}

	void Awake() {
		_instance = this;
	}

	void Start() {
		DirectCamera (PUNManager._instance.PlayerIndex);
	}

	void RandPos() {
		int rand = Random.Range (0, 4);
		transform.position = _position [rand];
		transform.rotation = _rotation [rand];
	}

	public void DirectCamera(int playerIndex) {
		transform.position = _position [playerIndex];
		transform.rotation = _rotation [playerIndex];
	}

}
