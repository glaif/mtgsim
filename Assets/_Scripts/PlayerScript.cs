using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {
    public Deck Deck { get; set; }

    // TEST Code
    public GameObject testCard;

    void Start () {
        //string deckName = PlayerPrefs.GetString("Player deck", "New deck");
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
        GameObject deckLocGO = GameObject.Find("Player/Cards/Deck");
        GameObject c = Instantiate(testCard, deckLocGO.transform);

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
