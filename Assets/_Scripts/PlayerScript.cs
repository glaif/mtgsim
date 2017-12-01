using UnityEngine;

public class PlayerScript : MonoBehaviour {
    public DeckScript deckSC;
    public HandScript handSC;
    public MainGameScript mgSC;

    public string DeckName { get; set; }

    //public NetworkPlayerComScript NetPlayerSC {
    //    get { return netPlayerSC; }
    //    set {
    //        netPlayerSC = value;

    //        if (playerComSC != null)
    //            Debug.LogError("Trying to set playerComSC a second time!");

    //        playerComSC = value;
    //    }
    //}

    //public AIPlayerComScript AIPlayerSC {
    //    get { return aiPlayerSC; }
    //    set {
    //        aiPlayerSC = value;

    //        if (playerComSC != null )
    //            Debug.LogError("Trying to set playerComSC a second time!");

    //        playerComSC = value;
    //    }
    //}

    //private IPlayerCom playerComSC;
    //private NetworkPlayerComScript netPlayerSC;
    //private AIPlayerComScript aiPlayerSC;

    private Deck deck;
    private Hand hand;

    public void PrepStartGame(int cardCount=0) {
        // Do all the pre game stuff that needs to happen to get this player ready to start
        // Player will have cardCount == 0
        // AI / Net Player will have cardCount > 0
        InitializeDeck(DeckName, cardCount);
        InitializeHand();
    }

    public void DealCards(int count) {
        Debug.Log("PlayerSC.DealCards firing");
        deckSC.DealCards(count);
    }

    public int RecycleHand() {
        int curCount = handSC.CardCount();
        // Reshuffle the entire hand into the deck
        handSC.RecycleHand();
        return curCount;
    }

    public void ShuffleDeck() {
        deckSC.ShuffleDeck();
    }

    private void InitializeDeck(string deckName, int cardCount=0) {
        deck = new Deck(deckName, cardCount);
        if (deck == null) {
            Debug.LogError("InitializeDeck() called with null deck");
        }
        deckSC.InitializeDeck(deck);
    }

    private void InitializeHand() {
        hand = new Hand();
        handSC.InitializeHand(hand);
    }
}
