using UnityEngine;
using UnityEngine.UI;

public class DSMenuScript : MonoBehaviour {
    private GameObject dsGO;

    void Start() {
        dsGO = GameObject.FindGameObjectWithTag("DeckSelectMenu");
        if (dsGO == null) {
            Debug.Log("Null GameObject reference for DeckSelectMenu");
            Application.Quit();
        }
        UIGORegistry.Register(dsGO.gameObject);
        dsGO.SetActive(false);
    }
	
    public void SelectDeckClick() {
        MainGameScript mainGameSC = GameObject.Find("Battleground").GetComponent<MainGameScript>();
        if (mainGameSC == null) {
            Debug.Log("Null MainGameScript object reference for DeckSelectMenu");
        }

        // Is there a possible race condition here?
        // Can LocalPlayer be null if mainGameSC is behind
        // this script?
        GameObject localPlayer = mainGameSC.LocalPlayerGO;
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

        GameObject ddGO = GameObject.Find("SelectDeckDropdown/Label");
        if (ddGO == null) {
            Debug.LogError("Unable to find Deck Selection Dropdown GO");
            return;
        }

        Text ddText = ddGO.GetComponent<Text>();
        if (ddText == null) {
            Debug.LogError("Unable to get Label Text component for Deck Selection Dropdown");
            return;
        }

        //string deckName = ddText.text;
        string deckName = "deck1.txt";  // TEST CODE
        playerSC.PrepStartGame(deckName);

        // Start game state machine
        dsGO.SetActive(false);
        mainGameSC.UpdateGameState(MainGameScript.GameState.DEAL, null);
    }

    public void CancelClick() {
        dsGO.SetActive(false);
    }
}
