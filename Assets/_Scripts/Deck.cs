using UnityEngine;
using System.Collections.Generic;

public class Deck {
    string deckFile;
    List<Card> deckList = new List<Card>();
    

    public Deck(string deck_file) {
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
        GameObject.Find("Player/Cards/Deck").GetComponent<DeckScript>().InitializeDeck();
    }

    public int GetDeckCount() {
        return deckList.Count;
    }

    public List<Card> GetNextNCards(int n) {
        List<Card> retCards = new List<Card>();

        for (int i = 0; i < n; i++) {
            if (deckList.Count == 0)
                break;
            retCards.Add(deckList[deckList.Count - 1]);
            deckList.RemoveAt(deckList.Count - 1);
        }
        return retCards;
    }

    void TestDeck() {
        Debug.Log("MTGDeck::TestDeck called");
        int count = 0;
        while (count < 60) {
            deckList.Add(new CardTester("TestCard", 1, 1, "B", "TES"));
            count += 1;
        }
    }
}
