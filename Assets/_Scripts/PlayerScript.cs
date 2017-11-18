using UnityEngine;

public class PlayerScript : MonoBehaviour {
    private GameObject playerGO;

    private Deck deck;
    private GameObject deckGO;
    private DeckScript deckSC;

    private Hand hand;
    private GameObject handGO;
    private HandScript handSC;

    void Start () {
        //string deckName = PlayerPrefs.GetString("Player deck", "New deck");

        // Need to ensure that these searches are 
        // disambiguous for multi-player setting
        playerGO = this.gameObject;
        if (playerGO == null) {
            Debug.LogError("Could not find Player GO");
            return;
        }

        deckGO = playerGO.transform.Find("Cards/Deck").gameObject;
        if (deckGO == null) {
            Debug.LogError("Could not find Deck GO");
            return;
        }

        deckSC = deckGO.GetComponent<DeckScript>();
        if (deckSC == null) {
            Debug.LogError("Could not find Deck SC");
            return;
        }

        handGO = playerGO.transform.Find("Cards/Hand").gameObject;
        if (handGO == null) {
            Debug.LogError("Could not find Hand GO");
            return;
        }

        handSC = handGO.GetComponent<HandScript>();
        if (handSC == null) {
            Debug.LogError("Could not find Hand SC");
            return;
        }
    }

    void Update() {
        
    }

    public void PrepStartGame(string deckName) {
        // Do all the pre game stuff that needs to happen to get this player ready to start
        InitializeDeck(deckName);
        InitializeHand();
    }

    public void DealCards(int count) {
        //Debug.Log("DealCards firing");
        deckSC.DealCards(count);
    }

    public int RecycleHand() {
        int curCount = handSC.CardCount();
        // Reshuffle the entire hand into the deck
        handSC.RecycleHand();
        return curCount;
    }

    private void InitializeDeck(string deckName) {
        //Debug.Log("InitializeDeck called");

        deck = new Deck(deckName);
        if (deck == null) {
            Debug.LogError("InitializeDeck() called with null deck");
        }
        deckSC.InitializeDeck(deckGO, deck, handGO);
    }

    private void InitializeHand() {
        hand = new Hand();
        handSC.InitializeHand(handGO, hand, deckGO);
    }
}
