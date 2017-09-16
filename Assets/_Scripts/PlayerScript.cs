using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {
    public MTGDeck Deck { get; set; }

    // TEST code
    public GameObject testCard;


    void Start () {
        //string deckName = PlayerPrefs.GetString("Player deck", "New deck");
	}
	
    public void PrepStartGame() {
        // Do all the pre game stuff that needs to happen to get this player ready to start
        ShuffleDeck();
    }

    public void ShuffleDeck() {
        Debug.Log("ShuffleDeck called");
        if (Deck == null) {
            Debug.LogError("ShuffleDeck() called with null deck");
            return;
        }
        Deck.ShuffleDeck();

        // TEST Code
        
        GameObject deckLocGO = GameObject.Find("Player/Cards/Deck");
        GameObject c = Instantiate(testCard, deckLocGO.transform);

        Texture cardSkin = (Texture)Resources.Load("Cards/1347");
        if (cardSkin == null) {
            Debug.LogError("Error loading card skin");
            return;
        }

        Material cardMat = new Material(Shader.Find("Unlit/Transparent"));
        cardMat.SetTexture("_MainTex", cardSkin);
        c.GetComponent<Renderer>().material = cardMat;
    }
}
