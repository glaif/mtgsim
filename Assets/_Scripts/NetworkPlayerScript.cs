using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkPlayerScript : NetworkBehaviour {
    private GameObject localPlayerGO;
    private PlayerScript playerSC;
    private GameObject dsGO;

    // Use this for initialization
    void Start () {
		if (isLocalPlayer) {
            MainGameScript mainGameSC = GameObject.Find("Battleground").GetComponent<MainGameScript>();
            if (mainGameSC == null) {
                Debug.Log("Null MainGameScript object reference for NetworkPlayerScript");
            }

            // Is there a possible race condition here?
            // Can LocalPlayer be null if mainGameSC is behind
            // this script?
            localPlayerGO = mainGameSC.LocalPlayerGO;
            if (localPlayerGO == null) {
                Debug.Log("Null localPlayer in SelectDeckClick");
                Debug.LogError("Error getting localPlayer from NetworkPlayerScript");
                return;
            }
            playerSC = localPlayerGO.GetComponent<PlayerScript>();
            if (playerSC == null) {
                Debug.LogError("Unable to find Player SC in NetworkPlayerScript");
                return;
            }
            Debug.Log("Local Player Start Complete");
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void OnStartClient() {
        Debug.Log("OnStartClient fired");
    }

    public override void OnStartLocalPlayer() {
        Debug.Log("OnStartLocalPlayer fired");
        if (dsGO == null) {
            dsGO = UIGORegistry.Find("DeckSelectMenu");
            if (dsGO == null) {
                Debug.Log("Null GameObject reference for DeckSelectMenu");
            }
        }
        dsGO.SetActive(true);
    }
}