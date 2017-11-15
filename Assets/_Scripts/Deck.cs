using UnityEngine;
using System.Collections.Generic;

public class Deck {
    private string deckFile;
    private List<Card> deckList = new List<Card>();
    

    public Deck(string deck_file) {
        Debug.Log("MTGDeck::Constructor called: deck == " + deck_file);
        deckFile = deck_file;
        LoadDeck();
    }

    void LoadDeck() {
        Debug.Log("MTGDeck::LoadDeck called: deckFile == " + deckFile);
        // Need to read deck from external file
        // Then instantiate and add cards to deck
        // Instantiate prefab for a card when it 
        // is drawn from deck, though
        
        //TEST CODE
        TestDeck();
        foreach (Card c in deckList) {
            c.SetZoneTappable(false);
        }
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

    public void ReplaceCard(Card c) {
        deckList.Add(c);
    }

    private static System.Random rng = new System.Random();

    public void Shuffle() {
        int n = deckList.Count;
        Card value;

        while (n > 1) {
            n--;
            int k = rng.Next(n + 1);
            value = deckList[k];
            deckList[k] = deckList[n];
            deckList[n] = value;
        }
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
