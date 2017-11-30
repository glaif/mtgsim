using System.Collections.Generic;
using UnityEngine;

public class HandScript : MonoBehaviour {
    public DeckScript deckSC;

    private Hand hand;

    public void InitializeHand(Hand hand) {
        if (hand == null) {
            Debug.LogError("Null Hand passed into InitializaDeck()");
            return;
        }
        this.hand = hand;
    }

    public void AddCardsToHand(List<Card> cards) {
        foreach (Card card in cards)
            AddCardToHand(card);
    }

    public void AddCardToHand(Card card) {
        card.CardPrefabInst.transform.position =
            new Vector3(
                card.CardPrefabInst.transform.position.x + (hand.CardCount() * 1.51f),
                card.CardPrefabInst.transform.position.y,
                card.CardPrefabInst.transform.position.z);
        hand.AddCardToHand(card);
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
