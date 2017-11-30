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
        PlayerScript playerSC = mgSC.playerSC;
        if (playerSC == null) {
            Debug.LogError("Unable to find Player GO");
            return;
        }

        string deckName = ddText.text;
        Debug.Log(string.Format("Deck selected: {0}", ddText.text));

        deckName = "deck1.txt";  // TEST CODE

        playerSC.PrepStartGame();

        // Start game state machine
        //mgSC.UpdateGameState(MainGameScript.GameState.DEAL, null);
    }

    public void CancelClick() {
        dsGO.SetActive(false);
    }
}
