using System.Collections.Generic;

public class Hand {
    private List<Card> handList = new List<Card>();

    public void AddCardToHand(Card card) {
        handList.Add(card);
        card.SetZoneTappable(false);
    }

    public Card RemoveCardFromHand(int index=0) {
        if (handList.Count == 0)
            return null;

        Card resCard = handList[index];
        handList.RemoveAt(index);
        return resCard;
    }

    public int CardCount() {
        return handList.Count;
    }
}
