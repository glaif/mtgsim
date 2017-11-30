using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameScript : MonoBehaviour {
    //public GameObject playerPrefab;
    public GameObject PlayerGO;
    public GameObject Opponent1GO;
    public GameObject Opponent2GO;
    public GameObject Opponent3GO;
    public GameObject MasterNetPlayerGO { get; set; }

    public GameObject popupGO;     // Reference to reusable modal popup window GO
    public PUModalScript popupSC;  // Reference to reusable modal popup window script

    [SerializeField]
    private PlayerScript playerSC;

    private List<GameObject> playersList;

    private int numPlayers = 1;
    private bool OReadySignalled = false;

    private GameState currentState;
    private Dictionary<string, object> stateParams;

    public MainGameScript() {
        InitializeTransitionArray();
    }

    void Start() {
        //GameObject battleGroundGO = gameObject; // This is not strictly necessary, but is more clear
        //LocalPlayerGO = Instantiate(playerPrefab, battleGroundGO.transform);
        //if (LocalPlayerGO == null) {
        //    Debug.LogError("Error getting player GO");
        //}

        playerSC = PlayerGO.GetComponent<PlayerScript>();
        if (playerSC == null) {
            Debug.LogError("Error null player SC object");
            return;
        }

        currentState = GameState.JOIN;
    }

    void Update() {
        // Once opponent connects roll d20
        // Winner hosts state machine for the game
        // Loser set up to receive state machine updates from winner
        // For now, just one player so set things in motion

        MainGameLoop();
    }

    public GameObject GetNextAvailOpponentGO() {
        GameObject resGO = null;
        switch (numPlayers) {
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
            numPlayers++;
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
        currentState = UpdateGameState(currentState, ref stateParams);

    }

    private GameState Join(ref Dictionary<string, object> parms) {
        return new GameState();
    }

    private GameState PrepStart(ref Dictionary<string, object> parms) {
        return new GameState();
    }

    // Do the deal cards logic - int count=7, bool mulligan=true
    private GameState DealCards(ref Dictionary<string, object> parms) {
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
            UpdateGameState(GameState.DEAL, ref stateParams);
        } else {
            Debug.Log("No mulligan chosen.  Setting state to P_READY");
            UpdateGameState(GameState.P_READY, ref stateParams);
        }
    }

    private GameState UpdateGameState(GameState state, ref Dictionary<string, object> newParms) {
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
    private GameState PReady(ref Dictionary<string, object> parms) {
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

    private GameState OReady(ref Dictionary<string, object> parms) {
        Debug.Log("OReady firing");
        // Wait here for other player(s) to move into ready state
        StartCoroutine("WaitForSigOReady");
        return new GameState();
    }

    // P_UNTAP
    private GameState PUntap(ref Dictionary<string, object> parms) {
        Debug.Log("PUntap firing");
        // Untap all tapped cards
        // Keep a tapped cards list in the player script object
        // to track tapped cards
        // Also need to check each card to make sure it should untap
        return new GameState();
    }

    private GameState OUntap(ref Dictionary<string, object> parms) {
        return new GameState();
    }

    private GameState PUpkeep(ref Dictionary<string, object> parms) {
        Debug.Log("PUpkeep firing");
        // Check upkeep list for any upkeep requirements
        return new GameState();
    }

    private GameState OUpkeep(ref Dictionary<string, object> parms) {
        return new GameState();
    }

    private GameState PDraw(ref Dictionary<string, object> parms) {
        Debug.Log("PDraw firing");
        return new GameState();
    }

    private GameState ODraw(ref Dictionary<string, object> parms) {
        return new GameState();
    }

    private GameState PMain(ref Dictionary<string, object> parms) {
        return new GameState();
    }

    private GameState OMain(ref Dictionary<string, object> parms) {
        return new GameState();
    }

    private GameState PCombat(ref Dictionary<string, object> parms) {
        return new GameState();
    }

    private GameState OCombat(ref Dictionary<string, object> parms) {
        return new GameState();
    }

    private GameState PDiscard(ref Dictionary<string, object> parms) {
        return new GameState();
    }

    private GameState ODiscard(ref Dictionary<string, object> parms) {
        return new GameState();
    }

    // State machine code starts here
    public enum GameState {
        JOIN,
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

    private delegate GameState Transition(ref Dictionary<string, object> parms);

    private TransitionData[] transitionArray;

    private void InitializeTransitionArray() {
        transitionArray = new TransitionData[] {
            new TransitionData(Join, null),
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
