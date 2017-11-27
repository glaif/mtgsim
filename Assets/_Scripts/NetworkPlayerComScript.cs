using UnityEngine;

public class NetworkPlayerComScript : Photon.MonoBehaviour, IPlayer {
    public PhotonLogLevel Loglevel = PhotonLogLevel.Informational;

    private MainGameScript mgSC;
    private PlayerScript playerSC;

    private int msgNum = 0;

    void Start() {
        Debug.Log("Starting NetworkPlayerScript");
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

        playerSC = mgSC.PlayerGO.GetComponent<PlayerScript>();
        if (playerSC == null) {
            Debug.LogError("Error null player SC object");
            return;
        }

        playerSC.NetPlayerSC = this;

        if (PhotonNetwork.isMasterClient) {
            // Notify the game that we are the master client
            Debug.Log("Setting master client: " + PhotonNetwork.player.ID);
            mgSC.MasterNetPlayerGO = this.gameObject;
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
        Debug.Log(string.Format("Ready sent by {0} (ID: {1}), received by {2} (ID: {3}) ", name, id, PhotonNetwork.player.NickName, PhotonNetwork.player.ID));
        if (photonView.isMine == true) {
            mgSC.SigOReady();
        }
    }
}
