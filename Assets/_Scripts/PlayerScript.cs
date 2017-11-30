using UnityEngine;

public class PlayerScript : MonoBehaviour {
    public DeckScript deckSC;
    public HandScript handSC;
    public MainGameScript mgSC;

    public NetworkPlayerComScript NetPlayerSC {
        get { return netPlayerSC; }
        set {
            netPlayerSC = value;

            if (playerComSC != null)
                Debug.LogError("Trying to set playerComSC a second time!");

            playerComSC = value;
        }
    }

    public AIPlayerComScript AIPlayerSC {
        get { return aiPlayerSC; }
        set {
            aiPlayerSC = value;

            if (playerComSC != null )
                Debug.LogError("Trying to set playerComSC a second time!");

            playerComSC = value;
        }
    }

    public string DeckName { get; set; }

    private IPlayer playerComSC;
    private NetworkPlayerComScript netPlayerSC;
    private AIPlayerComScript aiPlayerSC;

    private Deck deck;
    private Hand hand;

    public void PrepStartGame(int cardCount=0) {
        // Do all the pre game stuff that needs to happen to get this player ready to start
        if (cardCount > 0) {
            // Player path
            InitializeDeck(DeckName);
        } else {
            // Opponent path
            InitializeDeck(Deck.OpponentDeckStr, cardCount);
        }
        InitializeHand();
    }

    public void DealCards(int count) {
        deckSC.DealCards(count);
    }

    public int RecycleHand() {
        int curCount = handSC.CardCount();
        // Reshuffle the entire hand into the deck
        handSC.RecycleHand();
        return curCount;
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


    // Callbacks to Main Game Script

    public void SendReady() {
        playerComSC.SendReady();
    }

    public void SendStartGame(int cardCount) {
        playerComSC.SendStartGame(cardCount);
    }
}
