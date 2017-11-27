using UnityEngine;
using UnityEngine.UI;

public class DSMenuScript : MonoBehaviour {
    public GameObject dsGO;
    public MainGameScript mainGameSC;
    public Text ddText;

    void Start() {
        dsGO.SetActive(false);
    }
	
    public void SelectDeckClick() {
        // Is there a possible race condition here?
        // Can LocalPlayer be null if mainGameSC is behind
        // this script?
        GameObject localPlayer = mainGameSC.PlayerGO;
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
        dsGO.SetActive(false);
        mainGameSC.UpdateGameState(MainGameScript.GameState.DEAL, null);
    }

    public void CancelClick() {
        dsGO.SetActive(false);
    }
}
