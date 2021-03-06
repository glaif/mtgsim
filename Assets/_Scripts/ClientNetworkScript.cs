﻿using DictionaryEntry = System.Collections.DictionaryEntry;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System;

public class MainNetworkScript : MonoBehaviour, IPlayerCom {
    private const string GameStateProp = "GameState";
    private const string GameStateParamsProp = "GameStateParams";
    private const string CardCountProp = "CardCount";

    private const string DefaultPort = "50051";

    /* Each client has a ComService.  For the master client
     * the ComService serves two purposes: (i) act as master
     * game state machine (for all players), and (ii) 
     * issue callbacks for this player.  For all other clients
     * it just issues callbacks to its respective player. */

    /* There are two communication paths: 
     * 1) ComClient issues RPC to Master ComService.
     * 2) Master ComService issues RPC callbacks to 
     * other (non-Master) ComServices.
     * #2 is utilized to notify state changes so we can avoid 
     * polling. */
    private bool masterClient = false;
    private ComServiceServer comSvc;
    private ComClient comCli;
    private string playerName;
    private string comSvcIPaddr;
    private string comSvcPort;
    private Dictionary<string, string> playerContextMap;

    public GameObject netPlayerPrefab;
    public GameObject netObjs;
    public GameObject dsGO;
    public GameObject conMessageGO;
    public MainGameScript mgSC;


    void Start () {
        Debug.Log("Starting main network script: masterClient == " + masterClient);
    }

    public void ConnectToLocalServer() {
        ConnectToServer("127.0.0.1", "50051");
    }

    public bool ConnectToServer(string ipaddr, string port) {
        this.comSvcIPaddr = ipaddr;
        this.comSvcPort = DefaultPort;
        if ((port != null) && (port != "")) {
            this.comSvcPort = port;
        }

        if (comSvc == null) {
            comSvc = new ComServiceServer();
        }
        if (comSvc.IsRunning() == false) {
            comSvc.StartService(masterClient, comSvcPort);
        }

        if (((ipaddr == "127.0.0.1") || (ipaddr == "localhost"))
            && (masterClient == false)) {
            Debug.Log("Cannot connect to local com service unless you are the host (master) client");
            return false;
        }

        if (comCli == null) {
            comCli = new ComClient(ipaddr, comSvcPort, GetPlayerName());
        }
        return comCli.PingService();
    }

    public bool JoinGame(string playerName, string clientUri) {
        // If I'm the Master client, respond to the call
        // Otherwise hand the call off to my RPC client to make
        // remotely to the Master client
        if (IsMasterClient() == true) {
            AddOpponent(playerName, clientUri);
            return true;
        } else {
            return comCli.JoinGame();
        }
    }

    private void AddOpponent(string playerName, string clientUri) {
        if (playerContextMap == null) {
            Debug.Log("Allocating playerContextMap");
            playerContextMap = new Dictionary<string, string>();
        }
        if (playerContextMap[playerName] != null) {
            Debug.LogError("Error: new player already found in network context map");
            return;
        }
        playerContextMap.Add(playerName, clientUri);
        mgSC.AddOpponent(playerName);
    }


    public void TriggerClientGameStateChange(MainGameScript.GameState state, Dictionary<string, object> parms) {
        // Client received state Change from Master, 
        // so set Client state appropriately
        mgSC.SyncGameState(state, parms);
    }

    //private void ProcessPropChangedAtClient(/*PhotonPlayer*/ string player, Hashtable changedProps) {
    //    // Handle for info sent from Master to Client
    //    foreach (DictionaryEntry prop in changedProps) {
    //        string key = prop.Key.ToString();

    //        Debug.Log("Processing property change at Client: prop - " + key + ", value: " + prop.Value.ToString() + ", Player: " + player);

    //        switch (key) {
    //            case GameStateProp:
    //                MainGameScript.GameState state = (MainGameScript.GameState)prop.Value;
    //                Dictionary<string, object> parms = null;

    //                if (changedProps.ContainsKey(GameStateParamsProp)) {
    //                    parms = (Dictionary<string, object>)(changedProps[GameStateParamsProp]);
    //                }
    //                TriggerClientGameStateChange(state, parms);
    //                break;
    //            default:
    //                Debug.LogError("Unknown propery change attempted at Client");
    //                break;
    //        }
    //    }
    //}

    //private void ProcessPropChangedAtMaster(/* PhotonPlayer */ string player, Hashtable changedProps) {
    //    // Will use to handle for info sent from Client to Master
    //    foreach (DictionaryEntry prop in changedProps) {
    //        string key = prop.Key.ToString();

    //        Debug.Log("Processing property change at Master: prop - " + key);

    //        switch (key) {
    //            case CardCountProp:
    //                int cardCount = (int)prop.Value;
    //                //mgSC.UpdateOppDeckCount(player.NickName, cardCount);
    //                mgSC.UpdateOppDeckCount(player, cardCount);
    //                break;
    //            default:
    //                Debug.LogError("Unknown propery change attempted at Master");
    //                break;
    //        }
    //    }
    //}


    // Event Calls


    /* public override void OnPhotonPlayerPropertiesChanged(object[] playerAndUpdatedProps) {
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
    } */

    // Methods from IPlayerCom

    public void SetPlayerName(string name) {
        if ((name == null) || (name == "")) {
            Debug.Log("Error setting network player name");
            return;
        }
        playerName = name;
    }

    public string GetPlayerName() {
        if (playerName == null) {
            throw new Exception("Error: Trying to fetch null player name");
        }
        return playerName;
    }

    public bool IsMasterClient() {
        return masterClient;
    }

    public void SetMasterClient() {
        masterClient = true;
    }

    public void SetNewState(MainGameScript.GameState state) {
        Debug.Log("Master setting game state: " + state.ToString());
    }

    public void SetOppDeckSize(int cardCount) {
        Debug.Log("Client setting opponent deck size: " + cardCount);
    }
}
