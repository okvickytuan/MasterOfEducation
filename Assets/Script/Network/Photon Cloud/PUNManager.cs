using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PUNManager : ConnectAndJoinRandom {

	internal static PUNManager _instance;

	public GameObject player;
	public Image[] playerArrImg = new Image[4];

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
		DontDestroyOnLoad (this);
		_view = GetComponent<PhotonView> ();

		PhotonNetwork.automaticallySyncScene = true;
	}
	
	// Update is called once per frame
	public override void Update () {

	}

	public override void OnConnectedToMaster()
	{
		PhotonNetwork.JoinRandomRoom();
	}
	
	public override void OnJoinedLobby()
	{
		PhotonNetwork.JoinRandomRoom();
	}

	public override void OnFailedToConnectToPhoton(DisconnectCause cause)
	{
		Debug.LogError("Cause: " + cause);
	}

	public void ConnectToCloud() {
		if (!PhotonNetwork.connected)
		{
			Debug.Log("Let's connect to the Photon Master Server. Calling: PhotonNetwork.ConnectUsingSettings();");
			
			PhotonNetwork.ConnectUsingSettings(Version + "." + SceneManagerHelper.ActiveSceneBuildIndex);
		}
	}
	
	public void OnJoinedRoom()
	{
		joinedRoom = true;
		playerIndex = PhotonNetwork.room.playerCount - 1;
		_view.RPC ("ShowNewPlayerJoined", PhotonTargets.All, playerIndex);

		if (PhotonNetwork.room.playerCount == PhotonNetwork.room.maxPlayers) {
			PhotonNetwork.room.open = false;
			PhotonNetwork.room.visible = false;
			_view.RPC ("ChangeScene", PhotonTargets.All);
		}
	}

	public void OnPhotonPlayerConnected(PhotonPlayer other) {

	}

	[PunRPC]
	void ChangeScene() {
		Application.LoadLevel("ingameScene");
	}

	[PunRPC]
	void ShowNewPlayerJoined(int playerIndex) {
		for (int i=0; i<=playerIndex; i++) {
			this.playerArrImg [i].enabled = true;
		}
		print (this.playerArrImg [0].enabled);
	}

}
