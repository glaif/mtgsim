using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {
    public Deck Deck { get; set; }
    public Hand Hand { get; set; }

    public GameObject DeckGO;
    public GameObject HandGO;

    // TEST Code
    public GameObject testCard;

    void Start () {
        //string deckName = PlayerPrefs.GetString("Player deck", "New deck");
        DeckGO = GameObject.Find("Player/Cards/Deck");
        HandGO = GameObject.Find("Player/Cards/Hand");
    }

    void Update() {
        
    }

    public void PrepStartGame(string deckName) {
        // Do all the pre game stuff that needs to happen to get this player ready to start
        
        InitializeDeck(deckName);
    }

    public void InitializeDeck(string deckName) {
        Debug.Log("InitializeDeck called");

        Deck = new Deck(deckName);
        if (Deck == null) {
            Debug.LogError("InitializeDeck() called with null deck");
            return;
        }
        Deck.InitializeDeck();

        // TEST Code
        GameObject c = Instantiate(testCard, DeckGO.transform);

        Texture cardSkin = (Texture)Resources.Load("Cards/CardBack");
        if (cardSkin == null) {
            Debug.LogError("Error loading card skin");
            return;
        }

        Material cardMat = new Material(Shader.Find("Unlit/Transparent"));
        cardMat.SetTexture("_MainTex", cardSkin);
        c.GetComponentInChildren<Renderer>().material = cardMat;
    }
}
