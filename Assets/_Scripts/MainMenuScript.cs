using UnityEngine;

public class MainMenuScript : MonoBehaviour {
    private GameObject mmGO;
    private GameObject dsGO;

    void Start() {
        mmGO = GameObject.FindWithTag("MainMenu");
        if (mmGO == null) {
            Debug.Log("Null GameObject reference for MainMenu");
            Application.Quit();
        }

        UIGORegistry.Register(mmGO.gameObject);
        mmGO.SetActive(true);
    }

    void Update() {
        // First check to see is escape key was just pressed
        bool down = Input.GetButtonDown("ShowMainMenu");

        if (dsGO == null) {
            Debug.Log("Null GameObject reference for DeckSelectMenu");
            dsGO = UIGORegistry.Find("DeckSelectMenu");
        }

        // Handle escape key menu trigger
        if (down) {
            dsGO.SetActive(false);
            if (mmGO.activeSelf == true) {
                mmGO.SetActive(false);
            } else {
                mmGO.SetActive(true);
            }
        }
    }

    public void StartGameClick() {
        Debug.Log("StartGameClick fired");
        mmGO.SetActive(false);
        if (dsGO == null) {
            dsGO = UIGORegistry.Find("DeckSelectMenu");
            if (dsGO == null) {
                Debug.Log("Null GameObject reference for DeckSelectMenu");
            }
        }
        dsGO.SetActive(true);
    }

    public void StartNetworkGameClick() {
        // Will be used when we add the network play capabilities
        // Do network init stuff
        StartGameClick();
    }

    public void ImportDeckClick() {
        Debug.Log("ImportDeckClick fired");
    }
}