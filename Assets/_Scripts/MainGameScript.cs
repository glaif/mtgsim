﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameScript : MonoBehaviour {
    public const string GameVersion = "v0.2";

    private const int StartingHandSize = 7;

    public GameObject Opponent1GO;
    public GameObject Opponent2GO;
    public GameObject Opponent3GO;
    public GameObject popupGO;     // Reference to reusable modal popup window GO
    public PUModalScript popupSC;  // Reference to reusable modal popup window script
    public PlayerScript playerSC;

    //public GameObject MasterNetPlayerGO { get; set; }
    public int NumOpponents { get; private set; }
    public IPlayerCom PlayerComSC { get; set; }

    // flags
    public bool DeckSelected { get; set; }

    [SerializeField]
    private MainNetworkScript netSC;

    private Dictionary<string, Opponent> opponentList;
    private int desiredOpponents = 1; // TODO: needs to be set by UI
    private bool OReadySignalled = false;
    private TransitionData currentState;
    private Dictionary<string, object> stateParams;


    void Start() {
        InitializeTransitionArray();
        currentState = UpdateGameState(GameState.JOIN, null);
        DeckSelected = false;
    }

    void Update() {
        // The Master Client executes the main game loop & state machine
        if ((netSC.enabled == false) || (netSC.MasterClient == true)) {
            // Execute if networking disabled or
            // if networking enabled and is Master Client
            MainGameLoop();
        }
    }

    public GameObject AddOpponent(string playerName) {
        GameObject newOpponent = GetNextAvailOpponentGO();
        if (newOpponent != null) {
            NumOpponents++;
        } else {
            Debug.LogError("Cannot create Opponent Player: " + playerName);
            return null;
        }

        newOpponent.SetActive(true);

        if (opponentList == null) {
            Debug.Log("Allocating opponentList");
            opponentList = new Dictionary<string, Opponent>();
        }
        opponentList.Add(playerName, new Opponent(playerName, newOpponent));

        return newOpponent;
    }

    private GameObject GetNextAvailOpponentGO() {
        GameObject resGO = null;
        switch (NumOpponents) {
            case 0:
                resGO = Opponent1GO;
                break;
            case 1:
                resGO = Opponent2GO;
                break;
            case 2:
                resGO = Opponent3GO;
                break;
            default:
                Debug.LogError("Too many OpponentGO's requested");
                break;
        }
        return resGO;
    }


    // Game State Machine Functions

    private void MainGameLoop() {
        // Call the next state machine function
        // TODO: Need to add the inital states that are missing
        // e.g., player join, etc.
        // TODO: This script will call into the Main Network and Main AI scripts
        // depending on type of game being played.
        // TODO: Need to decouple the player objects from the
        // Main Network and Main AI Scripts.
        // TODO: Finish decoupling UI from game mechanics

        currentState.TransFunc(currentState.Parms);
    }

    private TransitionData UpdateGameState(GameState state, Dictionary<string, object> newParms) {
        // Turns: untap, upkeep, draw, main, combat, main, discard
        // Adjusts game state and synchronizes with remote player state machine

        //Debug.Log("GameState changed to: " + state);

        TransitionData transData = GetTransition(state);
        if (newParms != null) {
            transData.Parms = newParms;
        }
        return transData;
    }

    private void Join(Dictionary<string, object> parms) {
        if (NumOpponents == desiredOpponents)
            currentState = UpdateGameState(GameState.SELCTDECK, null);
    }

    private void SelectDeck(Dictionary<string, object> parms) {
        if (DeckSelected)
            currentState = UpdateGameState(GameState.PREPSTART, null);
    }

    private void PrepStart(Dictionary<string, object> parms) {
        playerSC.PrepStartGame();

        foreach (KeyValuePair<string, Opponent> opp in opponentList) {
            // TODO: Fix this so that we get correct card count from
            // each opponent - for now set to 60
            opp.Value.opponentSC.PrepStartGame(60);
        }

        if ((netSC.enabled == false) || (netSC.MasterClient == true)) {
            PlayerComSC.SendPrepStart();
        }
        currentState = UpdateGameState(GameState.ROLL, null);
    }

    private void Roll(Dictionary<string, object> parms) {
        stateParams = new Dictionary<string, object>() { { "count", StartingHandSize }, { "mulligan", true } };
        currentState = UpdateGameState(GameState.DEAL, null);
    }

    // Do the deal cards logic - int count=7, bool mulligan=true
    private void DealCards(Dictionary<string, object> parms) {
        if ((bool)parms["mulligan"]) {
            popupGO.SetActive(true);

            playerSC.DealCards((int)parms["count"]);
            
            // Check if the player wants to mulligan
            popupSC.SetModalMessage("Take a mulligan?", MulliganPUResponse);
        }
        currentState = UpdateGameState(GameState.MULRES, null);
    }

    private void WaitMulliganResponse(Dictionary<string, object> parms) {
        // Do nothing
        return;
    }

    public void MulliganPUResponse(bool response) {
        if (response == true) {
            Debug.Log("Mulligan chosen.  Resetting state to P_DEAL");
            int curCount = playerSC.RecycleHand();
            playerSC.ShuffleDeck();

            // Call dealcards again, with 1 less card in hand
            stateParams = new Dictionary<string, object>() { { "count", curCount - 1 }, { "mulligan", true } };
            currentState = UpdateGameState(GameState.DEAL, stateParams);
        } else {
            Debug.Log("No mulligan chosen.  Setting state to P_READY");
            // TODO: Deal opponent cards here
            foreach (KeyValuePair<string, Opponent> opp in opponentList) {
                // TODO: Fix this so that we get correct starting hand size from
                // each opponent - for now set to 7
                opp.Value.opponentSC.DealCards(7);
            }
            currentState = UpdateGameState(GameState.P_READY, null);
        }
    }

    // P_READY
    private void PReady(Dictionary<string, object> parms) {
        //Debug.Log("PReady firing");
        //playerSC.SendReady();
        //currentState = UpdateGameState(GameState.O_READY, null);
    }

    // O_READY
    public void SigOReady() {
        OReadySignalled = true;
    }

    private void OReady(Dictionary<string, object> parms) {
        Debug.Log("OReady firing");
        // Wait here for other player(s) to move into ready state
    }

    // P_UNTAP
    private void PUntap(Dictionary<string, object> parms) {
        Debug.Log("PUntap firing");
        // Untap all tapped cards
        // Keep a tapped cards list in the player script object
        // to track tapped cards
        // Also need to check each card to make sure it should untap
    }

    private void OUntap(Dictionary<string, object> parms) { }

    private void PUpkeep(Dictionary<string, object> parms) { }

    private void OUpkeep(Dictionary<string, object> parms) { }

    private void PDraw(Dictionary<string, object> parms) { }

    private void ODraw(Dictionary<string, object> parms) { }

    private void PMain(Dictionary<string, object> parms) { }

    private void OMain(Dictionary<string, object> parms) { }

    private void PCombat(Dictionary<string, object> parms) { }

    private void OCombat(Dictionary<string, object> parms) { }

    private void PDiscard(Dictionary<string, object> parms) { }

    private void ODiscard(Dictionary<string, object> parms) { }

    public enum GameState {
        JOIN,
        SELCTDECK,
        PREPSTART,
        ROLL,
        DEAL,
        MULRES,
        P_READY,
        P_UNTAP,
        P_UPKEEP,
        P_DRAW,
        P_MAIN,
        P_COMBAT,
        P_DISCARD,
        O_READY,
        O_UNTAP,
        O_UPKEEP,
        O_DRAW,
        O_MAIN,
        O_COMBAT,
        O_DISCARD
    };

    public void SyncGameState(GameState state, Dictionary<string, object> newParms) {
        currentState = UpdateGameState(state, newParms);
    }

    private delegate void Transition(Dictionary<string, object> parms);

    private TransitionData[] transitionArray;

    private void InitializeTransitionArray() {
        transitionArray = new TransitionData[] {
            new TransitionData(Join, null),
            new TransitionData(SelectDeck, null),
            new TransitionData(PrepStart, null),
            new TransitionData(Roll, null),
            new TransitionData(DealCards, new Dictionary<string, object>() { { "count", 7 }, { "mulligan", true } }),
            new TransitionData(WaitMulliganResponse, null),
            new TransitionData(PReady, null),
            new TransitionData(PUntap, null),
            new TransitionData(PUpkeep, null),
            new TransitionData(PDraw, null),
            new TransitionData(PMain, null),
            new TransitionData(PCombat, null),
            new TransitionData(PDiscard, null),
            new TransitionData(OReady, null),
            new TransitionData(OUntap, null),
            new TransitionData(OUpkeep, null),
            new TransitionData(ODraw, null),
            new TransitionData(OMain, null),
            new TransitionData(OCombat, null),
            new TransitionData(ODiscard, null)
        };
    }

    private TransitionData GetTransition(GameState current) {
        return transitionArray[(int)current];
    }

    private TransitionData GetNextTransition(GameState current) {
        GameState cur = current;
        // Check if we need to wrap around to the beginning os states
        if (cur == GameState.O_DISCARD) {
            cur = GameState.P_UNTAP;
        }
        return transitionArray[(int)cur];
    }

    private class TransitionData {
        public TransitionData(Transition tf, Dictionary<string, object> p) {
            TransFunc = tf;
            Parms = p;
        }

        public Transition TransFunc { get; set; }
        public Dictionary<string, object> Parms { get; set; }
    }
}
