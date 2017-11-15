using System.Collections.Generic;
using UnityEngine;

public class DeckScript : MonoBehaviour {
    public GameObject CardPrefab;  // Stores reference to the Card Prefab

    private Deck _deck;
    private GameObject handGO;

    public void InitializeDeck(Deck deck) {
        if (deck == null) {
            Debug.LogError("Null Deck passed into InitializaDeck()");
            return;
        }

        handGO = GameObject.Find("Player/Cards/Hand");
        if (handGO == null) {
            Debug.LogError("Could not find Hand GO");
            return;
        }

        _deck = deck;

        _deck.Shuffle();

        gameObject.GetComponentInChildren<TextMesh>().text =
            string.Format("Cards\nRemaining: {0}", deck.GetDeckCount());
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

        List<Card> cardsDrawn = _deck.GetNextNCards(drawCount);
        
        for (int i = 0; i < cardsDrawn.Count; i++) {
            GameObject c = Instantiate(CardPrefab, handGO.transform);
            cardsDrawn[i].CardPrefabInst = c;
            c.GetComponent<CardScript>().Card = cardsDrawn[i];

            Texture cardSkin = (Texture)Resources.Load("Cards/1347");
            if (cardSkin == null) {
                Debug.LogError("Error loading card skin");
                return;
            }
            Material cardMat = new Material(Shader.Find("Unlit/Transparent"));
            cardMat.SetTexture("_MainTex", cardSkin);
            c.GetComponentInChildren<Renderer>().material = cardMat;
        }

        handGO.GetComponent<HandScript>().AddCardsToHand(cardsDrawn);

        gameObject.GetComponentInChildren<TextMesh>().text =
            string.Format("Cards\nRemaining: {0}", _deck.GetDeckCount());
    }

    public void ReplaceCardOnDeck(Card c) {
        _deck.ReplaceCard(c);

        gameObject.GetComponentInChildren<TextMesh>().text =
            string.Format("Cards\nRemaining: {0}", _deck.GetDeckCount());
    }

    public void OnMouseDown() {
        Debug.Log("Clicked on " + this.transform.name + " zone.");
        DrawFromTopOfDeck();
    }
}
