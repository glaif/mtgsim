using UnityEngine;

public class NetworkPlayerComScript : Photon.MonoBehaviour, IPlayer {
    public PhotonLogLevel Loglevel = PhotonLogLevel.Informational;

    private MainGameScript mgSC;
    private PlayerScript playerSC;

    void Start() {
        Debug.Log("Starting NetworkPlayerScript");
        GameObject netObjsGO = GameObject.Find("Network Objects");
        if (netObjsGO == null) {
            Debug.LogError("Cannot find Network Objects GO");
        }
        transform.parent = netObjsGO.transform;

        if (photonView.isMine == true) {
            GameObject bgGO = GameObject.Find("Battleground");
            if (bgGO == null) {
                Debug.LogError("Cannot find Battleground GO");
            }

            mgSC = bgGO.GetComponent<MainGameScript>();
            if (mgSC == null) {
                Debug.LogError("Cannot find Main Game SC");
            }

            playerSC = mgSC.PlayerGO.GetComponent<PlayerScript>();
            if (playerSC == null) {
                Debug.LogError("Error null player SC object");
                return;
            }

            playerSC.NetPlayerSC = this;
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

    [PunRPC]
    void Ready(string name, int id) {
        Debug.Log(string.Format("Ready sent by {0} (ID: {1}), received by {2} (ID: {3})", name, id, PhotonNetwork.player.NickName, PhotonNetwork.player.ID));
        if (photonView.isMine == true) {
            mgSC.SigOReady();
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
