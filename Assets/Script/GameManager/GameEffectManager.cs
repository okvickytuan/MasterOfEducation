using UnityEngine;
using System.Collections;

public class GameEffectManager : MonoBehaviour {

	internal static GameEffectManager _instance;

	void Awake() {
		_instance = this;
	}

	// Use this for initialization
	void Start () {
		//InvokeRepeating ("test", 0.0f, 3.0f);
	}
	
	internal void CreateEffect(string path, Vector3 pos, float timeToDestroy) {
		GameObject prefab = Resources.Load<GameObject> (path);
		GameObject particle = Instantiate (prefab, pos, Quaternion.identity) as GameObject;
		Destroy (particle, timeToDestroy);
	}

	private void test() {
		CreateEffect ("Prefabs/Particle/visionBuff", new Vector3(200f, 3.5f, 170f), 2.0f);
	}

}
