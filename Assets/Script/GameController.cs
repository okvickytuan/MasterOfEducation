using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public GameObject[] _animal = new GameObject[4];

	private PhotonView _view;

	// Use this for initialization
	void Start () {
		_view = GetComponent<PhotonView> ();
		//_view.RPC ("CreateAnimal", PhotonTargets.All);
		CreateAnimal ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//[PunRPC]
	void CreateAnimal() {
		int index = PUNManager._instance.PlayerIndex;
		for (int j=0; j<3; j++) {
			GameObject go = PhotonNetwork.Instantiate (_animal [index].name, Vector3.zero, Random.rotation, 0) as GameObject;
			//go.transform.parent = GameObject.Find ("Player " + (index+1)).transform.FindChild("slot_" + (j+1));
			//go.transform.localPosition = Vector3.zero;
			Animal animal = go.GetComponent<Animal>();
			animal.GitAnimalParent(index, j+1);
			//_view.RPC("GitParent", PhotonTargets.All, go, index, j);
		}
	}

}
