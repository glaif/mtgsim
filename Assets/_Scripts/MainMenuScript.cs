using UnityEngine;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour {
    public GameObject mmGO;
    public GameObject dsGO;
    public InputField usernameIF;
    public MainGameScript mainGameSC;

    static string playerNamePrefKey = "PlayerName";

    void Start() {
        mmGO.SetActive(true);
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

    public void StartGameClick() {
        SetPlayerName();
        mmGO.SetActive(false);
        dsGO.SetActive(true);
    }

    private void SetDefaultUserName() {
        string defaultName = "";
        if (PlayerPrefs.HasKey(playerNamePrefKey)) {
            defaultName = PlayerPrefs.GetString(playerNamePrefKey);
            usernameIF.text = defaultName;
        }
        PhotonNetwork.playerName = defaultName;
    }

    public void SetPlayerName() {
        // force a trailing space string in case value is an empty 
        // string, else playerName would not be updated.
        PhotonNetwork.playerName = usernameIF.text;
        PlayerPrefs.SetString(playerNamePrefKey, usernameIF.text);
    }

    public void StartNetworkGameClick() {
        EnableNetworking();
        StartGameClick();
    }

    public void ImportDeckClick() {
        Debug.Log("ImportDeckClick fired");
    }

    private void EnableNetworking() {
        GameObject localPlayerGO = mainGameSC.LocalPlayerGO;
        if (localPlayerGO == null) {
            Debug.Log("Null localPlayer in SelectDeckClick");
            Debug.LogError("Error getting localPlayer from MainGameScript");
            Application.Quit();
        }

        Transform localNetPlayerT = localPlayerGO.transform.Find("NetworkPlayer");
        if (localNetPlayerT == null) {
            Debug.LogError("Null Transform object reference for NetworkPlayer");
        }
        localNetPlayerT.gameObject.SetActive(true);
    }
}