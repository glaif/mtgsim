using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckScript : MonoBehaviour {
    public GameObject CardPrefab;

    private GameObject player;
    private Deck deck;
    private GameObject handGO;

    public void InitializeDeck() {
        player = GameObject.Find("Player");
        if (player == null) {
            Debug.LogError("MTGDeckScript::Start: Could not find Player GO");
            return;
        }

        deck = player.GetComponent<PlayerScript>().Deck;
        if (deck == null) {
            Debug.LogError("MTGDeckScript::Start: Could not find Deck");
            return;
        }

        handGO = GameObject.Find("Player/Cards/Hand");
        if (handGO == null) {
            Debug.LogError("MTGDeckScript::Start: Could not find Hand GO");
            return;
        }

        gameObject.GetComponentInChildren<TextMesh>().text =
            string.Format("Cards\nRemaining: {0}", deck.GetDeckCount());
    }

    public void DealCards() {
        DrawFromTopOfDeck(7);
    }

    public void DrawFromTopOfDeck(int drawCount=1) {
        // Draw drawCount many cards from top of deck
        Debug.Log("darwCount = " + drawCount);
        Debug.Log("DrawFromTopOfDeck called to draw " + drawCount + " card" + (drawCount > 1 ? "s":""));

        List<Card> cardsDrawn = deck.GetNextNCards(drawCount);
        
        for (int i = 0; i < cardsDrawn.Count; i++) {
            GameObject c = Instantiate(CardPrefab, handGO.transform);
            cardsDrawn[i].CardPrefab = c;

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
            string.Format("Cards\nRemaining: {0}", deck.GetDeckCount());
    }

    public void OnMouseDown() {
        Debug.Log("Clicked on " + this.transform.name + " zone.");
        DrawFromTopOfDeck();
    }
}
