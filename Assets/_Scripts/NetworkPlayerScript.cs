using UnityEngine;

public class NetworkPlayerScript : Photon.PunBehaviour {
    // The PUN loglevel
    public PhotonLogLevel Loglevel = PhotonLogLevel.Informational;

    private string GAME_VERSION = "v0.2";

    // Use this for initialization
    void Start() {
        PhotonNetwork.logLevel = Loglevel;
        PhotonNetwork.ConnectUsingSettings(GAME_VERSION);
    }

    // Update is called once per frame
    void Update() {

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
        //this.photonView.RPC("ChatMessage", PhotonTargets.All, "jup", "and jup!");
    }

    [PunRPC]
    void ChatMessage(string a, string b) {
        Debug.Log(string.Format("ChatMessage {0} {1}", a, b));
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

}