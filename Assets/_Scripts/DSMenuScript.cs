using UnityEngine;
using UnityEngine.UI;

public class DSMenuScript : MonoBehaviour {
    public GameObject dsGO;
    public GameObject wfpGO;
    public MainGameScript mgSC;
    public PlayerScript playerSC;
    public Text ddText;

    void Start() {
        dsGO.SetActive(false);
    }
	
    public void SelectDeckClick() {
        dsGO.SetActive(false);

        string deckName = ddText.text;
        Debug.Log(string.Format("Deck selected: {0}", ddText.text));

        deckName = "deck1.txt";  // TEST CODE
        playerSC.DeckName = deckName;
        mgSC.DeckSelected = true;
    }

    public void CancelClick() {
        dsGO.SetActive(false);
    }
}
