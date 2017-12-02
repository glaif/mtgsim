using DictionaryEntry = System.Collections.DictionaryEntry;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;

public class MainNetworkScript : Photon.PunBehaviour, IPlayerCom {
    private const string GameStateProp = "GameState";
    private const string GameStateParamsProp = "GameStateParams";
    private const string CardCountProp = "CardCount";

    public GameObject netPlayerPrefab;
    public GameObject netObjs;
    public PhotonLogLevel Loglevel = PhotonLogLevel.Informational;
    public GameObject dsGO;
    public GameObject conMessageGO;
    public MainGameScript mgSC;


    void Start () {
        PhotonNetwork.logLevel = Loglevel;
        PhotonNetwork.ConnectUsingSettings(MainGameScript.GameVersion);
    }


    private void TriggerClientGameStateChange(MainGameScript.GameState state, Dictionary<string, object> parms) {
        // Client received state Change from Master, 
        // so set Client state appropriately
        mgSC.SyncGameState(state, parms);
    }

    private void ProcessPropChangedAtClient(PhotonPlayer player, Hashtable changedProps) {
        // Handle for info sent from Master to Client
        foreach (DictionaryEntry prop in changedProps) {
            string key = prop.Key.ToString();

            Debug.Log("Processing property change at Client: prop - " + key + ", value: " + prop.Value.ToString() + ", Player: " + player);

            switch (key) {
                case GameStateProp:
                    MainGameScript.GameState state = (MainGameScript.GameState)prop.Value;
                    Dictionary<string, object> parms = null;

                    if (changedProps.ContainsKey(GameStateParamsProp)) {
                        parms = (Dictionary<string, object>)(changedProps[GameStateParamsProp]);
                    }
                    TriggerClientGameStateChange(state, parms);
                    break;
                default:
                    Debug.LogError("Unknown propery change attempted at Client");
                    break;
            }
        }
    }

    private void ProcessPropChangedAtMaster(PhotonPlayer player, Hashtable changedProps) {
        // Will use to handle for info sent from Client to Master
        foreach (DictionaryEntry prop in changedProps) {
            string key = prop.Key.ToString();

            Debug.Log("Processing property change at Master: prop - " + key);

            switch (key) {
                case CardCountProp:
                    int cardCount = (int)prop.Value;
                    mgSC.UpdateOppDeckCount(player.NickName, cardCount);
                    break;
                default:
                    Debug.LogError("Unknown propery change attempted at Master");
                    break;
            }
        }
    }


    // Event Calls

    public override void OnConnectedToMaster() {
        Debug.Log("Connected to Photon server");
        RoomOptions ro = new RoomOptions();
        ro.IsVisible = false;
        ro.MaxPlayers = 2;
        PhotonNetwork.JoinOrCreateRoom("MTG", ro, TypedLobby.Default);
        /* */
    }

    public override void OnJoinedRoom() {
        Debug.Log("Joined Photon room");
        GameObject netPlayer = PhotonNetwork.Instantiate(netPlayerPrefab.name, netObjs.transform.position, Quaternion.identity, 0);
        if (netPlayer == null) {
            Debug.LogError("Error trying to instantiate a new Network Player GO");
            return;
        }
        conMessageGO.SetActive(false);
        dsGO.SetActive(true);
    }

    public override void OnPhotonPlayerPropertiesChanged(object[] playerAndUpdatedProps) {
        PhotonPlayer player = playerAndUpdatedProps[0] as PhotonPlayer;
        Hashtable changedProps = playerAndUpdatedProps[1] as Hashtable;

        // TODO: Right now, this only works for two players.
        // Need to extend it so that it can handle more than two.
        // Possibly using PhotonNetwork.masterClient.ID 

        if (player.CustomProperties.Count == 0) {
            // Ignore room join announcements
            // These (seem to) happen when a player first joins a room
            // I think the new player gets a copy of the properties from 
            // the existing players in the room
            Debug.Log("Ignoring Properties Join Announcement Event from Player: " + player.NickName + " at Player: " + PhotonNetwork.player.NickName);
            return;
        }

        if (player.NickName == PhotonNetwork.player.NickName) {
            // Do not react to my own updates
            Debug.Log("Ignoring Properties Changed Event from (Self) Player: " + player.NickName + " at Player: " + PhotonNetwork.player.NickName);
            return;
        }

        if (IsMasterClient() == true) {
            // I'm the Master and I just
            // got an update from a Client
            Debug.Log("Master got Properties Changed Event from Player: " + player.NickName);
            ProcessPropChangedAtMaster(player, changedProps);
        } else {
            // I'm a Client and I just
            // got an update from the Master
            Debug.Log("Client got Properties Changed Event from Player: " + player.NickName);
            ProcessPropChangedAtClient(player, changedProps);
        }
    }

    public override void OnDisconnectedFromPhoton() {
        Debug.Log("Disconnected from Photon server");
    }

    public override void OnPhotonCreateRoomFailed(object[] codeAndMsg) {
        Debug.Log("Failed to create room on Photon server");
    }

    public override void OnPhotonJoinRoomFailed(object[] codeAndMsg) {
        Debug.Log("Failed to join room on Photon server");
    }

    public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer) {
        Debug.Log("Player connected: " + newPlayer.NickName);
    }

    public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer) {
        Debug.Log("Player disconnected: " + otherPlayer.NickName);
    }


    // Synchronized Property Change Calls from IPlayerCom

    public bool IsMasterClient() {
        return PhotonNetwork.isMasterClient;
    }

    public void SetNewState(MainGameScript.GameState state) {
        Debug.Log("Master setting game state (" + PhotonNetwork.player.NickName + "): " + state.ToString());
        Hashtable propsToSet = new Hashtable() {
            { GameStateProp, state }
        };

        PhotonNetwork.player.SetCustomProperties(propsToSet);
    }

    public void SetOppDeckSize(int cardCount) {
        Debug.Log("Client setting opponent deck size (" + PhotonNetwork.player.NickName + "): " + cardCount);
        Hashtable propsToSet = new Hashtable() {
            { CardCountProp, cardCount }
        };

        PhotonNetwork.player.SetCustomProperties(propsToSet);
    }
}
