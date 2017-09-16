using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MTGDeckScript : MonoBehaviour {
    GameObject player;
    MTGDeck deck;

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

        gameObject.GetComponentInChildren<TextMesh>().text =
            string.Format("Cards\nRemaining: {0}", deck.GetDeckCount());
    }

    public void DrawFromTopOfDeck(int drawCount=1) {
        // Draw drawCount many cards from top of deck
        Debug.Log("darwCount = " + drawCount);
        Debug.Log("DrawFromTopOfDeck called to draw " + drawCount + " card" + (drawCount > 1 ? "s":""));
        gameObject.GetComponentInChildren<TextMesh>().text =
            string.Format("Cards\nRemaining: {0}", deck.GetDeckCount());
    }

    public void OnMouseDown() {
        Debug.Log("Clicked on " + this.transform.name + " zone.");
        DrawFromTopOfDeck();
    }
}
