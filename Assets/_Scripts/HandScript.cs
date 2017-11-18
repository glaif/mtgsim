using System.Collections.Generic;
using UnityEngine;

public class HandScript : MonoBehaviour {
    private Hand hand;

    private GameObject handGO;
    private HandScript handSC;

    private GameObject deckGO;
    private DeckScript deckSC;

    public void InitializeHand(GameObject handGO, Hand hand, GameObject deckGO) {
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

        if (hand == null) {
            Debug.LogError("Null Hand passed into InitializaDeck()");
            return;
        }
        this.hand = hand;
    }

    public void AddCardsToHand(List<Card> cards) {
        float i = hand.CardCount();
        foreach (Card card in cards) {
            hand.AddCardToHand(card);
            card.CardPrefabInst.transform.position =
                new Vector3(
                    card.CardPrefabInst.transform.position.x + (i*1.51f),
                    card.CardPrefabInst.transform.position.y,
                    card.CardPrefabInst.transform.position.z);
            i += 1f;
        }
    }

    public void RecycleHand() {
        GameObject cGO;
        Card c;
        int cardCount = hand.CardCount();

        Debug.Log("Recycling hand");
        for (int i = 0; i < cardCount; i++) {
            c = hand.RemoveCardFromHand();
            if (c != null) {
                cGO = c.CardPrefabInst;
                if (cGO != null)
                    Destroy(cGO);
                c.CardPrefabInst = null;
            }
            deckSC.ReplaceCardOnDeck(c);
        }
        Debug.Log("After recycle, card count: " + hand.CardCount());
    }

    public int CardCount() {
        return hand.CardCount();
    }
}
