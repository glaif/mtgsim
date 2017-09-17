using System.Collections.Generic;

public class Hand {
    List<Card> handList = new List<Card>();

    public void AddCardToHand(Card card) {
        handList.Add(card);
    }

    public int CardCount() {
        return handList.Count;
    }
}
