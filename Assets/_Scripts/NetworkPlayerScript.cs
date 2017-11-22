using UnityEngine;

public class NetworkPlayerScript : Photon.MonoBehaviour/*, IPunObservable*/ {
    // The PUN loglevel
    public PhotonLogLevel Loglevel = PhotonLogLevel.Informational;

    private int msgNum = 0;

    void Start() {
        Debug.Log("Starting NetworkPlayerScript");
        photonView.RPC("ChatMessage", PhotonTargets.Others, PhotonNetwork.player.NickName, "Hello!");
        //photonView.RPC("ChatMessage", PhotonTargets.All, PhotonNetwork.player.NickName, "Hello2!");
    }

    void Update() {

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