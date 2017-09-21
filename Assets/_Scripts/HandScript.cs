using System.Collections.Generic;
using UnityEngine;

public class HandScript : MonoBehaviour {
    private Hand hand;
    private GameObject playerGO;
    private PlayerScript playerSC;

    public void InitializeHand() {
        playerGO = GameObject.Find("Player");
        if (playerGO == null) {
            Debug.LogError("Could not find Player GO");
            return;
        }

        playerSC = playerGO.GetComponent<PlayerScript>();
        if (playerSC == null) {
            Debug.LogError("Could not find Player script object");
            return;
        }

        hand = playerSC.Hand;
    }

    public void AddCardsToHand(List<Card> cards) {
        float i = hand.CardCount();
        foreach (Card card in cards) {
            hand.AddCardToHand(card);
            Debug.Log("transform before: " + card.CardPrefabInst.transform.position);
            Debug.Log("i: " + i);
            card.CardPrefabInst.transform.position =
                new Vector3(
                    card.CardPrefabInst.transform.position.x + (i*1.51f),
                    card.CardPrefabInst.transform.position.y,
                    card.CardPrefabInst.transform.position.z);
            Debug.Log("transform after: " + card.CardPrefabInst.transform.position);
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
            playerSC.DeckGO.GetComponent<DeckScript>().ReplaceCardOnDeck(c);
        }
        Debug.Log("After recycle, card count: " + hand.CardCount());
    }

    public int CardCount() {
        return hand.CardCount();
    }
}
