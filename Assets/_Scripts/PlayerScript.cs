using UnityEngine;

public class PlayerScript : MonoBehaviour {
    public DeckScript deckSC;
    public HandScript handSC;

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

    private IPlayer playerComSC;
    private NetworkPlayerComScript netPlayerSC;
    private AIPlayerComScript aiPlayerSC;
    //private GameObject playerGO;
    private Deck deck;
    private Hand hand;

    void Start () {
        //string deckName = PlayerPrefs.GetString("Player deck", "New deck");
        //playerGO = gameObject;
    }

    void Update() {
        
    }

    public void PrepStartGame(string deckName) {
        // Do all the pre game stuff that needs to happen to get this player ready to start
        InitializeDeck(deckName);
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

    private void InitializeDeck(string deckName) {
        deck = new Deck(deckName);
        if (deck == null) {
            Debug.LogError("InitializeDeck() called with null deck");
        }
        deckSC.InitializeDeck(deck);
    }

    private void InitializeHand() {
        hand = new Hand();
        handSC.InitializeHand(hand);
    }

    public void SendReady() {
        playerComSC.SendReady();
    }
}
