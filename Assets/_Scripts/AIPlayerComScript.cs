using UnityEngine;

public class AIPlayerComScript : MonoBehaviour, IPlayer {

    private MainGameScript mgSC;
    private PlayerScript playerSC;

    void Start () {
        Debug.Log("AI Player loaded");

        GameObject aiObjsGO = GameObject.Find("AI Objects");
        if (aiObjsGO == null) {
            Debug.LogError("Cannot find AI Objects GO");
        }
        transform.parent = aiObjsGO.transform;

        GameObject bgGO = GameObject.Find("Battleground");
        if (bgGO == null) {
            Debug.LogError("Cannot find Battleground GO");
        }

        mgSC = bgGO.GetComponent<MainGameScript>();
        if (mgSC == null) {
            Debug.LogError("Cannot find Main Game SC");
        }

        playerSC = mgSC.playerSC;
        if (playerSC == null) {
            Debug.LogError("Error null player SC object");
            return;
        }

        playerSC.AIPlayerSC = this;
    }
	
	void Update () {
		
	}

    public void SendReady() {
        Debug.Log("Send ready called in AI Player SC");
    }

    public void SendStartGame(int cardCount) {
        Debug.Log("Send StartGame called in AI Player SC");
    }
}
