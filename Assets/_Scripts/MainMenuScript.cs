using UnityEngine;

public class MainMenuScript : MonoBehaviour {
    private GameObject mmgo;
    private GameObject dsgo;

    public void StartGameClick() {
        Debug.Log("StartGameClick fired");
        mmgo.SetActive(false);
        if (dsgo == null) {
            dsgo = MyRegistry.Find("DeckSelectMenu");
            if (dsgo == null) {
                Debug.Log("Null GameObject reference for DeckSelectMenu");
            }
        }
        dsgo.SetActive(true);
    }

    public void StartNetworkGameClick() {
        // Will be used when we add the network play capabilities
    }

    public void ImportDeckClick() {
        Debug.Log("ImportDeckClick fired");
    }

    void Start() {
        mmgo = GameObject.FindWithTag("MainMenu");
        if (mmgo == null) {
            Debug.Log("Null GameObject reference for MainMenu");
            Application.Quit();
        }
        MyRegistry.Register(mmgo.gameObject);
        mmgo.SetActive(true);
    }

    void Update() {
        bool down = Input.GetButtonDown("ShowMainMenu");
        if (dsgo == null) {
            Debug.Log("Null GameObject reference for DeckSelectMenu");
            dsgo = MyRegistry.Find("DeckSelectMenu");
        }
        if (down) {
            dsgo.SetActive(false);
            if (mmgo.activeSelf == true) {
                mmgo.SetActive(false);
            } else {
                mmgo.SetActive(true);
            }
        }
    }
}