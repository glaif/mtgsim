using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class Deck {
    private const string DeckPathStr = @"C:\stuff\MTG\";
    public const string OpponentDeckStr = "OpponentDeckStr";

    private List<Card> deckList = new List<Card>();

    public Deck(string deckFile, int cardCount) {
        //Debug.Log("MTGDeck::Constructor called: deck == " + deckFile);
        LoadDeck(deckFile, cardCount);
    }

    void LoadDeck(string deckFile, int cardCount) {
        //Debug.Log("MTGDeck::LoadDeck called: deckFile == " + deckFile);
        // Need to read deck from external file
        // Then instantiate and add cards to deck
        // Instantiate prefab for a card when it 
        // is drawn from deck, though

        //TEST CODE
        //TestDeck();
        if (deckFile == OpponentDeckStr) {
            LoadOpponentDeck(cardCount);
        } else {
            LoadPlayerDeck(deckFile);
        }
    }

    private void LoadOpponentDeck(int count) {
        for (int i = 0; i < count; i++)
            deckList.Add(new CardTester("CardBack", 0, 0, "", "NONE"));

        foreach (Card c in deckList) {
            c.SetZoneTappable(false);
        }
    }

    private void LoadPlayerDeck(string deckFile) {
        string deckFilePath = DeckPathStr + deckFile;
        
        StreamReader sr = new StreamReader(deckFilePath);
        string cardDescStr = sr.ReadLine();
        string[] cardDescArr;

        while (cardDescStr != null) {
            cardDescArr = cardDescStr.Split('|');
            deckList.Add(new CardTester(cardDescArr[0], int.Parse(cardDescArr[1]), 
                         int.Parse(cardDescArr[3]), cardDescArr[4], cardDescArr[5]));
            cardDescStr = sr.ReadLine();
        }

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
