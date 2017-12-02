using UnityEngine;

public class AIPlayerComScript : MonoBehaviour {

    private MainGameScript mgSC;

    private GameObject playerGO = null;
    private PlayerScript playerSC = null;

    private GameObject opponentGO = null;
    private PlayerScript opponentSC = null;

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

        //playerSC.AIPlayerSC = this;

        opponentGO = mgSC.AddOpponent("AI-" + (mgSC.NumOpponents+1));
        Debug.Log("Got opponentGO in NetworkPlayerComScript::start: " + opponentGO.name);
        if (opponentGO == null) {
            Debug.LogError("Cannot get an available Opponent GO");
        }

        opponentSC = opponentGO.GetComponent<PlayerScript>();
        if (opponentSC == null) {
            Debug.LogError("Error null Opponent SC object");
            return;
        }

        opponentSC.DeckName = Deck.OpponentDeckStr;
    }
}
