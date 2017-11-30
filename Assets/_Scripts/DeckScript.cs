using System.Collections.Generic;
using UnityEngine;

public class DeckScript : MonoBehaviour {
    public GameObject CardPrefab;  // Stores reference to the Card Prefab
    public HandScript handSC;

    private GameObject handGO;
    private GameObject deckGO;
    private Deck deck;

    // TEST Code
    public GameObject deckTopCard;

    public void InitializeDeck(Deck deck) {
        if (deck == null) {
            Debug.LogError("Null Deck passed into InitializaDeck()");
            return;
        }
        this.deck = deck;

        deckGO = gameObject;
        handGO = handSC.gameObject;

        ShuffleDeck();
    }

    private void ShuffleDeck() {
        deck.Shuffle();

        gameObject.GetComponentInChildren<TextMesh>().text =
            string.Format("Cards\nRemaining: {0}", deck.GetDeckCount());

        GameObject c = Instantiate(deckTopCard, deckGO.transform);
        Card dtc = new DeckTopCard();
        dtc.SetZoneTappable(false);
        c.GetComponent<CardScript>().Card = dtc;

        Texture cardSkin = (Texture)Resources.Load("Cards/CardBack");
        if (cardSkin == null) {
            Debug.LogError("Error loading card skin");
            return;
        }

        Material cardMat = new Material(Shader.Find("Unlit/Transparent"));
        cardMat.SetTexture("_MainTex", cardSkin);
        c.GetComponentInChildren<Renderer>().material = cardMat;
    }

    public void DealCards(int count) {
        if (count < 1) {
            Debug.LogError("Cannot deal less than 1 card");
            DrawFromTopOfDeck();
            return;
        }
        DrawFromTopOfDeck(count);
    }

    public void DrawFromTopOfDeck(int drawCount=1) {
        // Draw drawCount many cards from top of deck
        //Debug.Log("drawCount = " + drawCount);
        //Debug.Log("DrawFromTopOfDeck called to draw " + drawCount + " card" + (drawCount > 1 ? "s":""));

        List<Card> cardsDrawn = deck.GetNextNCards(drawCount);
        
        for (int i = 0; i < cardsDrawn.Count; i++) {
            string cardSkinStr = "Cards/" + cardsDrawn[i].SetCode + "/" + cardsDrawn[i].Id.ToString();
            GameObject c = PlaceCardInHand(cardSkinStr);
            cardsDrawn[i].CardPrefabInst = c;
            c.GetComponent<CardScript>().Card = cardsDrawn[i];
        }

        handSC.AddCardsToHand(cardsDrawn);

        SetDeckCardsRemaining(deck.GetDeckCount());
    }

    public GameObject PlaceCardInHand(string cardSkinStr) {
        GameObject c = Instantiate(CardPrefab, handGO.transform);
        Texture cardSkin = (Texture)Resources.Load(cardSkinStr);
        if (cardSkin == null) {
            Debug.LogError("Error loading card skin");
            Debug.Log("card skin string: " + cardSkinStr);
            return null;
        }
        Material cardMat = new Material(Shader.Find("Unlit/Transparent"));
        cardMat.SetTexture("_MainTex", cardSkin);
        c.GetComponentInChildren<Renderer>().material = cardMat;
        return c;
    }

    public void SetDeckCardsRemaining(int count) {
        gameObject.GetComponentInChildren<TextMesh>().text =
            string.Format("Cards\nRemaining: {0}", count);
    }

    public void ReplaceCardOnDeck(Card c) {
        deck.ReplaceCard(c);

        gameObject.GetComponentInChildren<TextMesh>().text =
            string.Format("Cards\nRemaining: {0}", deck.GetDeckCount());
    }

    public void OnMouseDown() {
        Debug.Log("Clicked on " + this.transform.name + " zone.");
        DrawFromTopOfDeck();
    }
}
