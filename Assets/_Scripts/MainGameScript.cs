using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainGameScript : MonoBehaviour {
    public const string GameVersion = "v0.2";

    public const string OppCardCountProp = "CardCount";

    private const int StartingHandSize = 7;

    public GameObject Opponent1GO;
    public GameObject Opponent2GO;
    public GameObject Opponent3GO;
    public GameObject popupGO;     // Reference to reusable modal popup window GO
    public PUModalScript popupSC;  // Reference to reusable modal popup window script
    public PlayerScript playerSC;

    // TESTING STUFF
    public GameObject TestMessageGO;

    public int NumOpponents { get; private set; }
    public IPlayerCom PlayerComSC { get; set; }

    // flags
    public bool DeckSelected { get; set; }
    private bool DeckCountSent = false;
    //private bool OReadySignalled = false;

    [SerializeField]
    private MainNetworkScript netSC;

    private Dictionary<string, Opponent> opponentList;
    private int desiredOpponents = 1; // TODO: needs to be set by UI
    private TransitionData currentState;
    private Dictionary<string, object> stateParams;


    void Start() {
        Debug.Log("MainGameScript started");
        InitializeTransitionArray();
        currentState = UpdateGameState(GameState.JOIN, null);
        DeckSelected = false;
    }

    void Update() {
        // Execute the main game loop & state machine
        if (PlayerComSC == null)
            return;
        MainGameLoop();
    }

    public void UpdateOppDeckCount(string playerName, int cardCount) {
        // Once we recieve the Client deck count at the Master
        // update the opponent object and signal the Client
        // to move onto the next state
        Debug.Log("UpdateOppDeckCount called by: " + playerName + " - Master: " + PlayerComSC.IsMasterClient());
        UpdateOpponent(playerName, MainGameScript.OppCardCountProp, cardCount);
        PlayerComSC.SetNewState(GameState.PREPSTART);
    }

    private void UpdateOpponent(string playerName, string prop, object value) {
        Opponent opp = opponentList[playerName];

        switch (prop) {
            case (MainGameScript.OppCardCountProp):
                opp.deckSize = (int)value;
                break;
            default:
                Debug.LogError("Unknown Opponent Prop passed to UpdateOpponent");
                break;
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

    private void DisplayState(GameState state) {
        TestMessageGO.GetComponent<Text>().text = "STATE = " + state.ToString();
    }

    private TransitionData UpdateGameState(GameState state, Dictionary<string, object> newParms) {
        // Turns: untap, upkeep, draw, main, combat, main, discard
        // Adjusts game state and synchronizes with remote player state machine

        //Debug.Log("GameState changed to: " + state);

        DisplayState(state); // Testing

        TransitionData transData = GetTransition(state);
        if (newParms != null) {
            transData.Parms = newParms;
        }
        return transData;
    }

    private void Join(Dictionary<string, object> parms) {
        // Initial game state
        if (PlayerComSC.IsMasterClient()) {
            // Master path
            // Matser joins by default without having to send a message
            // It just waits for opponents to join
            if (NumOpponents == desiredOpponents) {
                // Signal opponents to move to select deck state
                foreach (KeyValuePair<string, Opponent> opp in opponentList) {
                    PlayerComSC.SetNewState(GameState.SELCTDECK);
                }
                // Move to select deck state
                currentState = UpdateGameState(GameState.SELCTDECK, null);
            }
        } else {
            // Client path
            // Client sends a message to join a game hosted by another player
            netSC.JoinGame(null, null);
        }
    }

    private void SelectDeck(Dictionary<string, object> parms) {
        if (!DeckSelected)
            return;

        if (PlayerComSC.IsMasterClient()) {
            // Master path

            //foreach (KeyValuePair<string, Opponent> opp in opponentList) {
            //    PlayerComSC.SetNewState(GameState.PREPSTART);
            //}

            currentState = UpdateGameState(GameState.PREPSTART, null);
        } else {
            // Client path
            // Client sends its deck size to Master

            // This is set by a Client after sending deck count to Master
            if (DeckCountSent)
                return;

            PlayerComSC.SetOppDeckSize(playerSC.deckSC.GetDeckCount());
            DeckCountSent = true;
        }
    }

    private void PrepStart(Dictionary<string, object> parms) {
        playerSC.PrepStartGame();

        if (PlayerComSC.IsMasterClient()) {
            foreach (KeyValuePair<string, Opponent> opp in opponentList) {
                opp.Value.opponentSC.PrepStartGame(opp.Value.deckSize);
            }
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
    //public void SigOReady() {
    //    OReadySignalled = true;
    //}

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
        // Check if we need to wrap around to the beginning of states
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
