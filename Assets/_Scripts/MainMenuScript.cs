using UnityEngine;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour {
    private const string playerNamePrefKey = "PlayerName";

    public GameObject mmGO;
    public GameObject dsGO;
    public GameObject conMessageGO;
    public GameObject wfpGO;
    public InputField usernameIF;
    public InputField serveripIF;
    public InputField portIF;
    public Text errorText;
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
        mainAiSC.SetMasterClient();
        mainAiSC.enabled = true;
        mgSC.PlayerComSC = mainAiSC;
        dsGO.SetActive(true);
    }

    public void StartNetworkHostGameClick() {
        SetPlayerName();
        mmGO.SetActive(false);
        mainNetSC.SetMasterClient();
        mainNetSC.enabled = true;
        mgSC.PlayerComSC = mainNetSC;

        mainNetSC.ConnectToLocalServer();
    }

    public void StartNetworkJoinGameClick() {
        SetPlayerName();
        mmGO.SetActive(false);
        conMessageGO.SetActive(true);
        mainNetSC.enabled = true;
        mgSC.PlayerComSC = mainNetSC;

        string ipaddr = serveripIF.text;
        string port = portIF.text;
        if ((ipaddr == null) || (ipaddr == "")) {
            RaiseErrorAndReset("Please enter a valid host IP address for the game");
            return;
        }
        bool res = mainNetSC.ConnectToServer(ipaddr, port);
        if (res == false) {
            RaiseErrorAndReset("Please enter a valid host IP address for the game");
            return;
        }
    }

    private void RaiseErrorAndReset(string text) {
        conMessageGO.SetActive(false);
        mainNetSC.enabled = false;
        mgSC.PlayerComSC = null;
        errorText.text = text;
        mmGO.SetActive(true);
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
        // PhotonNetwork.playerName = defaultName;
    }

    private void SetPlayerName() {
        // force a trailing space string in case value is an empty 
        // string, else playerName would not be updated.
        mainNetSC.SetPlayerName(usernameIF.text);
        PlayerPrefs.SetString(playerNamePrefKey, usernameIF.text);
    }
}