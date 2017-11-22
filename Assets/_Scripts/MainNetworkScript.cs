using UnityEngine;

public class MainNetworkScript : Photon.PunBehaviour {
    public GameObject netPlayerPrefab;
    public GameObject netObjs;
    public PhotonLogLevel Loglevel = PhotonLogLevel.Informational;
    public GameObject dsGO;
    public GameObject conMessageGO;

    private string GAME_VERSION = "v0.2";

    void Start () {
        PhotonNetwork.logLevel = Loglevel;
        PhotonNetwork.ConnectUsingSettings(GAME_VERSION);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void OnConnectedToMaster() {
        Debug.Log("Connected to Photon server");
        RoomOptions ro = new RoomOptions();
        ro.IsVisible = false;
        ro.MaxPlayers = 2;
        PhotonNetwork.JoinOrCreateRoom("MTG", ro, TypedLobby.Default);
    }

    public override void OnJoinedRoom() {
        Debug.Log("Joined Photon room");
        GameObject netPlayer = PhotonNetwork.Instantiate(netPlayerPrefab.name, netObjs.transform.position, Quaternion.identity, 0);
        netPlayer.transform.parent = netObjs.transform;
        conMessageGO.SetActive(false);
        dsGO.SetActive(true);
    }

    public override void OnDisconnectedFromPhoton() {
        Debug.Log("Disconnected from Photon server");
    }

    public override void OnPhotonCreateRoomFailed(object[] codeAndMsg) {
        Debug.Log("Failed to create room on Photon server");
    }

    public override void OnPhotonJoinRoomFailed(object[] codeAndMsg) {
        Debug.Log("Failed to join room on Photon server");
    }

    public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer) {
        Debug.Log("Player connected: " + newPlayer.NickName);
    }

    public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer) {
        Debug.Log("Player disconnected: " + otherPlayer.NickName);
    }
}
