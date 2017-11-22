using UnityEngine;

public class NetworkPlayerScript : Photon.MonoBehaviour/*, IPunObservable*/ {
    // The PUN loglevel
    public PhotonLogLevel Loglevel = PhotonLogLevel.Informational;

    private MainGameScript mgSC;

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

        if (photonView.isMine == true) {
            photonView.RPC("ChatMessage", PhotonTargets.OthersBuffered, PhotonNetwork.player.NickName, "Hello!");
            photonView.RPC("Ready", PhotonTargets.OthersBuffered, null);
        }

        if (PhotonNetwork.isMasterClient) {
            // Notify the game that we are the master client
            Debug.Log("Setting master client: " + PhotonNetwork.player.ID);
            mgSC.MasterPlayerGO = this.gameObject;
        }
    }

    void Update() {

    }

    [PunRPC]
    void Ready() {
        mgSC.SigOReady();
    }

    [PunRPC]
    void ChatMessage(string a, string b) {
        Debug.Log(string.Format("ChatMessage (ID: {0}) {1} {2} {3}", PhotonNetwork.player.ID, a, b, msgNum));
        msgNum++;
    }

    //public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
    //    Debug.Log("OnPhotonSerializeView called");
    //}
}