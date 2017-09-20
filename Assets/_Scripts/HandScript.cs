using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandScript : MonoBehaviour {
    private Hand hand;
    private GameObject playerGO;
    private PlayerScript playerSC;

    // Use this for initialization
    void Start() {
        hand = new Hand();

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
    }

    // Update is called once per frame
    void Update() {

    }

    public void AddCardsToHand(List<Card> cards) {
        float i = hand.CardCount();
        foreach (Card card in cards) {
            hand.AddCardToHand(card);
            Debug.Log("transform before: " + card.CardPrefab.transform.position);
            Debug.Log("i: " + i);
            card.CardPrefab.transform.position =
                new Vector3(
                    card.CardPrefab.transform.position.x + (i*1.51f),
                    card.CardPrefab.transform.position.y,
                    card.CardPrefab.transform.position.z);
            Debug.Log("transform after: " + card.CardPrefab.transform.position);
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
                cGO = c.CardPrefab;
                if (cGO != null)
                    Destroy(cGO);
                c.CardPrefab = null;
            }
            playerSC.DeckGO.GetComponent<DeckScript>().ReplaceCardOnDeck(c);
        }
        Debug.Log("After recycle, card count: " + hand.CardCount());
    }

    public int CardCount() {
        return hand.CardCount();
    }
}
