using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandScript : MonoBehaviour {
    Hand hand;

    // Use this for initialization
    void Start() {
        hand = new Hand();
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
}
