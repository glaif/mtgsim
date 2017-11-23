using UnityEngine;

public class PlayerScript : MonoBehaviour {
    public DeckScript deckSC;
    public HandScript handSC;

    public NetworkPlayerScript netPlayerSC { get; set; }

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
        // Ultimately this will have to handle both the single and multi-player
        // cases.  For now, we just assume multi-player.  In the future, 
        // we should probably create a parent class that abstracts both
        // network and ai opponents.
        netPlayerSC.SendReady();
    }
}
