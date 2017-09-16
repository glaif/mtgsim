using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DSMenuScript : MonoBehaviour {
    private GameObject dsgo;

    void Start() {
        dsgo = GameObject.FindGameObjectWithTag("DeckSelectMenu");
        if (dsgo == null) {
            Debug.Log("Null GameObject reference for DeckSelectMenu");
            Application.Quit();
        }
        MyRegistry.Register(dsgo.gameObject);
        dsgo.SetActive(false);
    }
	
    public void SelectDeckClick() {
        PlayerScript playerGOsc = GameObject.Find("Player").GetComponent<PlayerScript>();
        if (playerGOsc == null) {
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

        string deckName = ddText.text;
        MTGDeck deck = new MTGDeck(deckName);
        playerGOsc.Deck = deck;
        playerGOsc.PrepStartGame();
        dsgo.SetActive(false);
    }

    public void CancelClick() {
        dsgo.SetActive(false);
    }
}
