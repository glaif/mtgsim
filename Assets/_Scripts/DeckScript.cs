using UnityEngine;
using System.Collections;

public class DeckScript : MonoBehaviour {
    public MTGDeck deck;

    public class MTGDeck {
        string deckFile;

        public MTGDeck(string deck_file) {
            Debug.Log("MTGDeck::Constructor called: deck == " + deck_file);
            deckFile = deck_file;
        }

        public void LoadDeck() {
            Debug.Log("MTGDeck::LoadDeck called");
        }

        public void ShuffleDeck() {
            Debug.Log("MTGDeck::ShuffleDeck called: deck == " + deckFile);
        }
    }

	// Use this for initialization
	void Start () {
        string playerName = PlayerPrefs.GetString("Player deck", "New deck");
        deck = new MTGDeck(playerName);
        deck.LoadDeck();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ShuffleDeck() {
        Debug.Log("ShuffleDeck called");
        deck.ShuffleDeck();
    }
}
