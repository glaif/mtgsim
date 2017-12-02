using UnityEngine;

public class NetworkPlayerComScript : Photon.MonoBehaviour {
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
            playerSC = mgSC.playerSC;
            if (playerSC == null) {
                Debug.LogError("Error null player SC object");
                return;
            }

        } else {
            // If this is a remote network player, set the playerGO & SC
            // to the next available Opponent GO & SC from the Main Game SC
            opponentGO = mgSC.AddOpponent(PhotonNetwork.player.NickName);
            Debug.Log("Got opponentGO in NetworkPlayerComScript::start: " + opponentGO.name);
            if (opponentGO == null) {
                Debug.LogError("Cannot get an available Opponent GO");
            }

            opponentSC = opponentGO.GetComponent<PlayerScript>();
            if (opponentSC == null) {
                Debug.LogError("Error null opponentSC SC object");
                return;
            }
            opponentSC.DeckName = Deck.OpponentDeckStr;
        }

        // If I'm the Master Network Client, set it so in the master game SC
        // and notify the others
        //if ((photonView.isMine == true) && (PhotonNetwork.isMasterClient)) {
        //    Debug.Log("Setting master client: " + PhotonNetwork.player.ID);

        //    // Set the master network client locally
        //    mgSC.MasterNetPlayerGO = this.gameObject;
        //}
    }
}
