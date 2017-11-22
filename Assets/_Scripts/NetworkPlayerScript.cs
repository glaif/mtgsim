using UnityEngine;

public class NetworkPlayerScript : Photon.MonoBehaviour/*, IPunObservable*/ {
    // The PUN loglevel
    public PhotonLogLevel Loglevel = PhotonLogLevel.Informational;

    void Start() {
        Debug.Log("Starting NetworkPlayerScript");
        photonView.RPC("ChatMessage", PhotonTargets.All, "jup", "and jup!");
    }

    void Update() {

    }

    [PunRPC]
    void ChatMessage(string a, string b) {
        Debug.Log(string.Format("ChatMessage {0} {1}", a, b));
    }

    //public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
    //    Debug.Log("OnPhotonSerializeView called");
    //}
}