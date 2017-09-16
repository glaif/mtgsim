using UnityEngine;
using System.Collections.Generic;

public class MTGDeck {
    string deckFile;
    List<MTGCard> deckList = new List<MTGCard>();
    

    public MTGDeck(string deck_file) {
        Debug.Log("MTGDeck::Constructor called: deck == " + deck_file);
        deckFile = deck_file;
        LoadDeck();
    }

    void LoadDeck() {
        Debug.Log("MTGDeck::LoadDeck called: deckFile == " + deckFile);
        TestDeck();
    }

    public void InitializeDeck() {
        Debug.Log("MTGDeck::InitializeDeck called: deck == " + deckFile);
        GameObject.Find("Player/Cards/Deck").GetComponent<MTGDeckScript>().InitializeDeck();
    }

    public int GetDeckCount() {
        return deckList.Count;
    }

    void TestDeck() {
        Debug.Log("MTGDeck::TestDeck called");
        int count = 0;
        while (count < 60) {
            deckList.Add(new MTGCardTester("TestCard", 1, 1, "B", "TES"));
            count += 1;
        }
    }
}
