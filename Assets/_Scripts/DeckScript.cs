using System.Collections.Generic;
using UnityEngine;

public class DeckScript : MonoBehaviour {
    public GameObject CardPrefab;  // Stores reference to the Card Prefab

    private Deck deck;

    private GameObject handGO;
    private HandScript handSC;

    private GameObject deckGO;
    private DeckScript deckSC;

    // TEST Code
    public GameObject deckTopCard;

    public void InitializeDeck(GameObject deckGO, Deck deck, GameObject handGO) {
        if (deckGO == null) {
            Debug.LogError("Could not find Deck GO");
            return;
        }
        this.deckGO = deckGO;

        deckSC = deckGO.GetComponent<DeckScript>();
        if (deckSC == null) {
            Debug.LogError("Could not find Deck SC");
            return;
        }

        if (handGO == null) {
            Debug.LogError("Could not find Hand GO");
            return;
        }
        this.handGO = handGO;

        handSC = handGO.GetComponent<HandScript>();
        if (handSC == null) {
            Debug.LogError("Could not find Hand SC");
            return;
        }

        if (deck == null) {
            Debug.LogError("Null Deck passed into InitializaDeck()");
            return;
        }
        this.deck = deck;
        ShuffleDeck();
    }

    private void ShuffleDeck() {
        this.deck.Shuffle();

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
        Debug.Log("drawCount = " + drawCount);
        Debug.Log("DrawFromTopOfDeck called to draw " + drawCount + " card" + (drawCount > 1 ? "s":""));

        List<Card> cardsDrawn = deck.GetNextNCards(drawCount);
        
        for (int i = 0; i < cardsDrawn.Count; i++) {
            GameObject c = Instantiate(CardPrefab, handGO.transform);
            cardsDrawn[i].CardPrefabInst = c;
            c.GetComponent<CardScript>().Card = cardsDrawn[i];

            //Texture cardSkin = (Texture)Resources.Load("Cards/1347");
            string cardSkinStr = "Cards/" + cardsDrawn[i].SetCode + "/" + cardsDrawn[i].Id.ToString();
            Texture cardSkin = (Texture)Resources.Load(cardSkinStr);
            if (cardSkin == null) {
                Debug.LogError("Error loading card skin");
                Debug.Log("card: " + cardsDrawn[i].ToString());
                Debug.Log("card skin string: " + cardSkinStr);
                return;
            }
            Material cardMat = new Material(Shader.Find("Unlit/Transparent"));
            cardMat.SetTexture("_MainTex", cardSkin);
            c.GetComponentInChildren<Renderer>().material = cardMat;
        }

        handSC.AddCardsToHand(cardsDrawn);

        gameObject.GetComponentInChildren<TextMesh>().text =
            string.Format("Cards\nRemaining: {0}", deck.GetDeckCount());
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
