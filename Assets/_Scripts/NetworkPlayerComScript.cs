using UnityEngine;

public class NetworkPlayerComScript : Photon.MonoBehaviour, IPlayer {
    public PhotonLogLevel Loglevel = PhotonLogLevel.Informational;

    private MainGameScript mgSC;

    private GameObject playerGO = null;
    private PlayerScript playerSC = null;

    private GameObject opponentGO = null;
    private PlayerScript opponentSC = null;


    void Start() {
        Debug.Log("Starting NetworkPlayerScript - owner: " + photonView.owner + " isMine: " + photonView.isMine);
        
        GameObject netObjsGO = GameObject.Find("Network Objects");
        if (netObjsGO == null) {
            Debug.LogError("Cannot find Network Objects GO");
        }
        transform.parent = netObjsGO.transform;

        GameObject bgGO = GameObject.Find("Battleground");
        if (bgGO == null) {
            Debug.LogError("Cannot find Battleground GO");
        }

        mgSC = bgGO.GetComponent<MainGameScript>();
        if (mgSC == null) {
            Debug.LogError("Cannot find Main Game SC");
        }

        if (photonView.isMine == true) {
            // If this is a local network player, get the playerGO & SC
            // from the Main Game SC
            playerGO = mgSC.PlayerGO;
            playerSC = mgSC.PlayerGO.GetComponent<PlayerScript>();
            if (playerSC == null) {
                Debug.LogError("Error null player SC object");
                return;
            }

            playerSC.NetPlayerSC = this;

        } else {
            // If this is a remote network player, set the playerGO & SC
            // to the next available Opponent GO & SC from the Main Game SC
            opponentGO = mgSC.GetNextAvailOpponentGO();
            Debug.Log("Got opponentGO in NetworkPlayerComScript::start: " + opponentGO.name);
            if (opponentGO == null) {
                Debug.LogError("Cannot get an available Opponent GO");
            }
            opponentGO.SetActive(true);

            opponentSC = opponentGO.GetComponent<PlayerScript>();
            if (opponentSC == null) {
                Debug.LogError("Error null opponentSC SC object");
                return;
            }
        }

        // If I'm the Master Network Client, set it so in the master game SC
        // and notify the others
        if ((photonView.isMine == true) && (PhotonNetwork.isMasterClient)) {
            Debug.Log("Setting master client: " + PhotonNetwork.player.ID);

            // Set the master network client locally
            mgSC.MasterNetPlayerGO = this.gameObject;

            // Notify the others that I am the master client
            //photonView.RPC("SetMasterClient", PhotonTargets.OthersBuffered /*, ??? */);
        }
    }

    void Update() {

    }

    public void SendReady() {
        Debug.Log("Sending ready by " + PhotonNetwork.player.NickName + " (ID: " + PhotonNetwork.player.ID + ")");
        photonView.RPC("Ready", PhotonTargets.OthersBuffered, PhotonNetwork.player.NickName, PhotonNetwork.player.ID);
    }

    public void SendStartGame(int cardCount) {
        Debug.Log("Sending PrepStartGame by " + PhotonNetwork.player.NickName + " (ID: " + PhotonNetwork.player.ID + ")");
        photonView.RPC("PrepStartGame", PhotonTargets.OthersBuffered, cardCount, PhotonNetwork.player.NickName, PhotonNetwork.player.ID);
    }

    [PunRPC]
    void Ready(string name, int id) {
        Debug.Log(string.Format("Ready sent by {0} (ID: {1}), received by {2} (ID: {3})", 
                name, id, PhotonNetwork.player.NickName, PhotonNetwork.player.ID));
        if (photonView.isMine == true) {
            mgSC.SigOReady();
        }
    }

    [PunRPC]
    void PrepStartGame(int cardCount, string name, int id) {
        if (photonView.isMine == false) {
            Debug.Log(string.Format("PrepStartGame sent by {0} (ID: {1}), received by {2} (ID: {3}), count: {4}", 
                    name, id, PhotonNetwork.player.NickName, PhotonNetwork.player.ID, cardCount));
            Debug.Log("photonView.owner: " + photonView.owner);

            if (opponentSC == null)
                Debug.LogError("Null opponentSC inside RPC NetworkPlayerComScript::PrepStartGame");
            opponentSC.StartGame(cardCount);
        }
    }

    //[PunRPC]
    //void SetMasterClient(/* ??? */) {
    //    Debug.Log(string.Format("SetMasterClient received by {0} (ID: {1})", PhotonNetwork.player.NickName, PhotonNetwork.player.ID));

    //    if (mgSC == null) {
    //        Debug.Log(string.Format("mgSC is null for {0} (ID: {1})", PhotonNetwork.player.NickName, PhotonNetwork.player.ID));
    //        return;
    //    }

    //    if (mgSC.MasterNetPlayerGO == null) {
    //        Debug.Log(string.Format("mgSC.MasterNetPlayerGO is null for {0} (ID: {1})", PhotonNetwork.player.NickName, PhotonNetwork.player.ID));
    //        return;
    //    }

    //    // mgSC.MasterNetPlayerGO = ???;
    //}
}
