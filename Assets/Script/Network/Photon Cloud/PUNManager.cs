using UnityEngine;
using System.Collections;

public class PUNManager : ConnectAndJoinRandom {

	internal static PUNManager _instance;

	public GameObject player;

	private bool joinedRoom = false;
	private int playerIndex = 0;

	private PhotonView _view;

	public int PlayerIndex { get { return playerIndex; } }

	public void Awake() {
		_instance = this;
	}

	// Use this for initialization
	public override void Start () {
		base.Start ();
		DontDestroyOnLoad (gameObject);
		_view = GetComponent<PhotonView> ();

		PhotonNetwork.automaticallySyncScene = true;
	}
	
	// Update is called once per frame
	public override void Update () {
		base.Update ();

	}

	public override void OnFailedToConnectToPhoton(DisconnectCause cause)
	{
		Debug.LogError("Cause: " + cause);
	}
	
	public void OnJoinedRoom()
	{
		float randX = Random.Range (-4.0f, 4.0f);
		GameObject go = PhotonNetwork.Instantiate (player.name, new Vector3(randX, 0, 0), Quaternion.identity, 0);

		joinedRoom = true;
		playerIndex = PhotonNetwork.room.playerCount - 1;
		if (PhotonNetwork.room.playerCount == PhotonNetwork.room.maxPlayers) {
			_view.RPC("ChangeScene", PhotonTargets.All);
		}
	}

	public void OnPhotonPlayerConnected(PhotonPlayer other) {

	}

	[PunRPC]
	void ChangeScene() {
		gameObject.name = PhotonNetwork.room.playerCount.ToString();
		PhotonNetwork.LoadLevel("ingameScene");

	}

}
