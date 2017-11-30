using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DSMenuScript : MonoBehaviour {
    public GameObject dsGO;
    public GameObject wfpGO;
    public MainGameScript mgSC;
    public Text ddText;

    void Start() {
        dsGO.SetActive(false);
    }
	
    public void SelectDeckClick() {
        dsGO.SetActive(false);
        // Set a waiting for player message...
        GameObject localPlayer = mgSC.PlayerGO;
        if (localPlayer == null) {
            Debug.Log("Null localPlayer in SelectDeckClick");
            Debug.LogError("Error getting localPlayer from MainGameScript");
            Application.Quit();
        }

        PlayerScript playerSC = localPlayer.GetComponent<PlayerScript>();
        if (playerSC == null) {
            Debug.LogError("Unable to find Player GO");
            return;
        }

        string deckName = ddText.text;
        Debug.Log(string.Format("Deck selected: {0}", ddText.text));

        deckName = "deck1.txt";  // TEST CODE

        playerSC.PrepStartGame(deckName);

        // Start game state machine
        //mgSC.UpdateGameState(MainGameScript.GameState.DEAL, null);
    }

    public void CancelClick() {
        dsGO.SetActive(false);
    }
}
