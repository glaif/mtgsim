using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameScript : MonoBehaviour {
    public GameObject Opponent1GO;
    public GameObject Opponent2GO;
    public GameObject Opponent3GO;
    public GameObject MasterNetPlayerGO { get; set; }

    public GameObject popupGO;     // Reference to reusable modal popup window GO
    public PUModalScript popupSC;  // Reference to reusable modal popup window script

    public PlayerScript playerSC;

    [SerializeField]
    private MainNetworkScript netSC;

    private Dictionary<string, Opponent> opponentList;

    private int numOpponents = 0;
    private int desiredOpponents = 1; // TODO: needs to be set by UI

    private bool OReadySignalled = false;

    private GameState currentState;
    private Dictionary<string, object> stateParams;

    void Start() {
        InitializeTransitionArray();
        currentState = GameState.JOIN;
    }

    void Update() {
        // The Master Client executes the main game loop & state machine
        if (netSC.enabled == false) {
            MainGameLoop();
            return;
        }
        if (netSC.MasterClient == true)
            MainGameLoop();
    }

    public GameObject AddOpponent(string PlayerName) {
        GameObject newOpponent = GetNextAvailOpponentGO();
        opponentList.Add(PlayerName, new Opponent(PlayerName, newOpponent));
        return newOpponent;
    }

    private GameObject GetNextAvailOpponentGO() {
        GameObject resGO = null;
        switch (numOpponents) {
            case 1:
                resGO = Opponent1GO;
                break;
            case 2:
                resGO = Opponent2GO;
                break;
            case 3:
                resGO = Opponent3GO;
                break;
            default:
                Debug.LogError("Too many OpponentGO's requested");
                break;
        }
        if (resGO != null)
            numOpponents++;
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
        currentState = UpdateGameState(currentState, stateParams);

    }

    private GameState Join(Dictionary<string, object> parms) {
        if (numOpponents < desiredOpponents)
            return GameState.JOIN;
        return GameState.PREPSTART;
    }

    private GameState PrepStart(Dictionary<string, object> parms) {
        playerSC.PrepStartGame();

        foreach (KeyValuePair<string, Opponent> opp in opponentList) {
            opp.Value.opponentSC.PrepStartGame();
        }

        return GameState.ROLL;
    }

    private GameState Roll(Dictionary<string, object> parms) {

        return GameState.DEAL;
    }

    // Do the deal cards logic - int count=7, bool mulligan=true
    private GameState DealCards(Dictionary<string, object> parms) {
        if ((bool)parms["mulligan"]) {
            popupGO.SetActive(true);

            playerSC.DealCards((int)parms["count"]);
            
            // Check if the player wants to mulligan
            popupSC.SetModalMessage("Take a mulligan?", MulliganPUResponse);
        }
        return new GameState();
    }

    public void MulliganPUResponse(bool response) {
        if (response == true) {
            Debug.Log("Mulligan chosen.  Resetting state to P_DEAL");
            int curCount = playerSC.RecycleHand();

            // Call dealcards again, with 1 less card in hand
            stateParams = new Dictionary<string, object>() { { "count", curCount - 1 }, { "mulligan", true } };
            UpdateGameState(GameState.DEAL, stateParams);
        } else {
            Debug.Log("No mulligan chosen.  Setting state to P_READY");
            UpdateGameState(GameState.P_READY, stateParams);
        }
    }

    private GameState UpdateGameState(GameState state, Dictionary<string, object> newParms) {
        // Turns: untap, upkeep, draw, main, combat, main, discard
        // Adjusts game state and synchronizes with remote player state machine
        Debug.Log("GameState changed to: " + state);

        GameState nextState;

        TransitionData transData = GetTransition(state);
        if (newParms == null) {
            nextState = transData.TransFunc(transData.Parms);
        } else {
            nextState = transData.TransFunc(newParms);
        }
        return nextState;
    }

    // P_READY
    private GameState PReady(Dictionary<string, object> parms) {
        Debug.Log("PReady firing");
        playerSC.SendReady();
        UpdateGameState(GameState.O_READY, null);
        return new GameState();
    }

    // O_READY
    public void SigOReady() {
        OReadySignalled = true;
    }

    private IEnumerator WaitForSigOReady() {
        while (!OReadySignalled) {
            yield return new WaitForSeconds(.1f);
        }
        Debug.Log("OReady done");

        // If we're the master client:
            // Refresh the game state for both players:
                // Query OPlayer for game state and 
                // update OPlayer on our game state
            // Roll dice for both players and notify OPlayer of the results
            // Then move to P_UNTAP or O_UNTAP depending on who goes first
        // Otherwise, OPlayer will initiate the state changes

        //UpdateGameState(GameState.O_READY, null);
    }

    private GameState OReady(Dictionary<string, object> parms) {
        Debug.Log("OReady firing");
        // Wait here for other player(s) to move into ready state
        StartCoroutine("WaitForSigOReady");
        return new GameState();
    }

    // P_UNTAP
    private GameState PUntap(Dictionary<string, object> parms) {
        Debug.Log("PUntap firing");
        // Untap all tapped cards
        // Keep a tapped cards list in the player script object
        // to track tapped cards
        // Also need to check each card to make sure it should untap
        return new GameState();
    }

    private GameState OUntap(Dictionary<string, object> parms) {
        return new GameState();
    }

    private GameState PUpkeep(Dictionary<string, object> parms) {
        Debug.Log("PUpkeep firing");
        // Check upkeep list for any upkeep requirements
        return new GameState();
    }

    private GameState OUpkeep(Dictionary<string, object> parms) {
        return new GameState();
    }

    private GameState PDraw(Dictionary<string, object> parms) {
        Debug.Log("PDraw firing");
        return new GameState();
    }

    private GameState ODraw(Dictionary<string, object> parms) {
        return new GameState();
    }

    private GameState PMain(Dictionary<string, object> parms) {
        return new GameState();
    }

    private GameState OMain(Dictionary<string, object> parms) {
        return new GameState();
    }

    private GameState PCombat(Dictionary<string, object> parms) {
        return new GameState();
    }

    private GameState OCombat(Dictionary<string, object> parms) {
        return new GameState();
    }

    private GameState PDiscard(Dictionary<string, object> parms) {
        return new GameState();
    }

    private GameState ODiscard(Dictionary<string, object> parms) {
        return new GameState();
    }

    // State machine code starts here
    public enum GameState {
        JOIN,
        PREPSTART,
        ROLL,
        DEAL,
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

    private delegate GameState Transition(Dictionary<string, object> parms);

    private TransitionData[] transitionArray;

    private void InitializeTransitionArray() {
        transitionArray = new TransitionData[] {
            new TransitionData(Join, null),
            new TransitionData(PrepStart, null),
            new TransitionData(Roll, null),
            new TransitionData(DealCards, new Dictionary<string, object>() { { "count", 7 }, { "mulligan", true } }),
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
