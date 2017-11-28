using UnityEngine;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour {
    public GameObject mmGO;
    public GameObject dsGO;
    public GameObject conMessageGO;
    public InputField usernameIF;
    public MainGameScript mainGameSC;
    public MainNetworkScript mainNetSC;
    public GameObject aiPlayerPrefab;
    public GameObject aiObjs;


    static string playerNamePrefKey = "PlayerName";

    void Start() {
        conMessageGO.SetActive(false);
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
        GameObject aiPlayer = Instantiate(aiPlayerPrefab, aiObjs.transform.position, Quaternion.identity);
        if (aiPlayer == null) {
            Debug.LogError("Error trying to instantiate a new AI Player GO");
            return;
        }
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
        SetPlayerName();
        mmGO.SetActive(false);
        conMessageGO.SetActive(true);
        EnableNetworking();
    }

    public void ImportDeckClick() {
        Debug.Log("ImportDeckClick fired");
    }

    private void EnableNetworking() {
        mainNetSC.enabled = true;
    }
}