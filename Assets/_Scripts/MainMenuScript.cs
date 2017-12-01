using UnityEngine;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour {
    private const string playerNamePrefKey = "PlayerName";

    public GameObject mmGO;
    public GameObject dsGO;
    public GameObject conMessageGO;
    public GameObject wfpGO;
    public InputField usernameIF;
    public MainGameScript mgSC;
    public MainNetworkScript mainNetSC;
    public MainAIScript mainAiSC;


    void Start() {
        // Turn off UI components that are not needed
        conMessageGO.SetActive(false);
        wfpGO.SetActive(false);
        
        // Turn on this menu
        mmGO.SetActive(true);

        // Prepopulate the username field, with the last username given
        SetDefaultUserName();
    }

    void Update() {
        //// First check to see is escape key was just pressed
        //bool down = Input.GetButtonDown("ShowMainMenu");

        //// Handle escape key menu trigger
        //if (down) {
        //    dsGO.SetActive(false);
        //    if (mmGO.activeSelf == true) {
        //        mmGO.SetActive(false);
        //    } else {
        //        mmGO.SetActive(true);
        //    }
        //}
    }

    public void StartLocalGameClick() {
        SetPlayerName();
        mmGO.SetActive(false);
        EnableLocalPlay();
        mgSC.PlayerComSC = mainAiSC;
        dsGO.SetActive(true);
    }

    public void StartNetworkGameClick() {
        SetPlayerName();
        mmGO.SetActive(false);
        conMessageGO.SetActive(true);
        EnableNetworkPlay();
        mgSC.PlayerComSC = mainNetSC;
    }

    public void ImportDeckClick() {
        Debug.Log("ImportDeckClick fired");
    }

    private void SetDefaultUserName() {
        string defaultName = "";
        if (PlayerPrefs.HasKey(playerNamePrefKey)) {
            defaultName = PlayerPrefs.GetString(playerNamePrefKey);
            usernameIF.text = defaultName;
        }
        PhotonNetwork.playerName = defaultName;
    }

    private void SetPlayerName() {
        // force a trailing space string in case value is an empty 
        // string, else playerName would not be updated.
        PhotonNetwork.playerName = usernameIF.text;
        PlayerPrefs.SetString(playerNamePrefKey, usernameIF.text);
    }

    private void EnableLocalPlay() {
        mainAiSC.enabled = true;
    }

    private void EnableNetworkPlay() {
        mainNetSC.enabled = true;
    }
}