using UnityEngine;

public class MainNetworkScript : Photon.PunBehaviour, IPlayerCom {
    public GameObject netPlayerPrefab;
    public GameObject netObjs;
    public PhotonLogLevel Loglevel = PhotonLogLevel.Informational;
    public GameObject dsGO;
    public GameObject conMessageGO;
    public MainGameScript mgSC;

    public bool MasterClient { get; private set; }


    void Start () {
        PhotonNetwork.logLevel = Loglevel;
        PhotonNetwork.ConnectUsingSettings(MainGameScript.GameVersion);
    }

    // Events Calls
    public override void OnConnectedToMaster() {
        Debug.Log("Connected to Photon server");
        RoomOptions ro = new RoomOptions();
        ro.IsVisible = false;
        ro.MaxPlayers = 2;
        PhotonNetwork.JoinOrCreateRoom("MTG", ro, TypedLobby.Default);
        /* */
    }

    public override void OnJoinedRoom() {
        Debug.Log("Joined Photon room");
        GameObject netPlayer = PhotonNetwork.Instantiate(netPlayerPrefab.name, netObjs.transform.position, Quaternion.identity, 0);
        if (netPlayer == null) {
            Debug.LogError("Error trying to instantiate a new Network Player GO");
            return;
        }
        MasterClient = PhotonNetwork.isMasterClient;
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

    // RPC Calls from IPlayerCom
    public void SendPrepStart() {
        photonView.RPC("RPCPrepStart", PhotonTargets.Others, PhotonNetwork.player.NickName, PhotonNetwork.player.ID);
    }

    public void SendReady() { }

    public void SendStartGame(int cardCount) { }

    // RPC Callbacks
    [PunRPC]
    void RPCPrepStart(string senderName, int senderId) {
        Debug.Log(string.Format("PrepStartGame received by {0} (ID: {1}), sender {2} (ID: {3})",
                    PhotonNetwork.player.NickName, PhotonNetwork.player.ID, senderName, senderId));

        mgSC.SyncGameState(MainGameScript.GameState.PREPSTART, null);
    }
}
