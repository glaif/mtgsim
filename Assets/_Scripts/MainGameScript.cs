using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameScript : MonoBehaviour {
    public GameObject playerPrefab;
    public GameObject LocalPlayerGO { get; private set; }

    public GameObject MasterPlayerGO { get; set; }

    public GameObject popupGO;     // Reference to reusable modal popup window GO
    public PUModalScript popupSC;  // Reference to reusable modal popup window script

    private PlayerScript playerSC;

    private bool OReadySignalled = false;

    public MainGameScript() {
        InitializeTransitionArray();
    }

    void Start() {
        GameObject battleGroundGO = gameObject; // This is not strictly necessary, but is more clear
        LocalPlayerGO = Instantiate(playerPrefab, battleGroundGO.transform);
        if (LocalPlayerGO == null) {
            Debug.LogError("Error getting player GO");
        }
        playerSC = LocalPlayerGO.GetComponent<PlayerScript>();
        if (playerSC == null) {
            Debug.LogError("Error null player SC object");
            return;
        }
    }

    void Update() {
        // Once opponent connects roll d20
        // Winner hosts state machine for the game
        // Loser set up to receive state machine updates from winner
        // For now, just one player so set things in motion
    }

    // Do the deal cards logic - int count=7, bool mulligan=true
    private void DealCards(Dictionary<string, object> parms) {
        if ((bool)parms["mulligan"]) {
            popupGO.SetActive(true);

            playerSC.DealCards((int)parms["count"]);
            
            // Check if the player wants to mulligan
            popupSC.SetModalMessage("Take a mulligan?", MulliganPUResponse);
        }
    }

    public void MulliganPUResponse(bool response) {
        if (response) {
            Debug.Log("Mulligan chosen.  Resetting state to P_DEAL");

            int curCount = playerSC.RecycleHand();
            
            // Call dealcards again, with 1 less card in hand
            UpdateGameState(GameState.DEAL, new Dictionary<string, object>() { { "count", curCount - 1 }, { "mulligan", true } });
        } else {
            Debug.Log("No mulligan chosen.  Setting state to P_READY");

            UpdateGameState(GameState.P_READY, null);
        }
    }

    public void SigOReady() {
        OReadySignalled = true;
    }

    private IEnumerator WaitForSigOReady() {
        while (!OReadySignalled) {
            yield return new WaitForSeconds(.1f);
        }
    }

    // Game State Machine Functions

    public int UpdateGameState(GameState state, Dictionary<string, object> newParms) {
        // Turns: untap, upkeep, draw, main, combat, main, discard
        // Adjusts game state and synchronizes with remote player state machine
        Debug.Log("GameState changed to: " + state);

        TransitionData transData = GetTransition(state);
        if (newParms == null) {
            transData.TransFunc(transData.Parms);
        } else {
            transData.TransFunc(newParms);
        }
        return 0;
    }

    private void PReady(Dictionary<string, object> parms) {
        Debug.Log("PReady firing");
        UpdateGameState(GameState.O_READY, null);
    }

    private void OReady(Dictionary<string, object> parms) {
        Debug.Log("OReady firing");
        // Wait here for other player(s) to move into ready state
        StartCoroutine("WaitForSigOReady");
        // If we're the master client:
            // Refresh the game state for both players:
                // Query OPlayer for game state and 
                // update OPlayer on our game state
                // Roll dice for both players and notify OPlayer of the results
                // Then move to P_UNTAP or O_UNTAP depending on who goes first
    }

    private void PUntap(Dictionary<string, object> parms) {
        Debug.Log("PUntap firing");
        // Untap all tapped cards
        // Keep a tapped cards list in the player script object
        // to track tapped cards
        // Also need to check each card to make sure it should untap
    }

    private void OUntap(Dictionary<string, object> parms) {

    }

    private void PUpkeep(Dictionary<string, object> parms) {
        Debug.Log("PUpkeep firing");
        // Check upkeep list for any upkeep requirements
    }

    private void OUpkeep(Dictionary<string, object> parms) {

    }

    private void PDraw(Dictionary<string, object> parms) {
        Debug.Log("PDraw firing");
    }

    private void ODraw(Dictionary<string, object> parms) {

    }

    private void PMain(Dictionary<string, object> parms) {

    }

    private void OMain(Dictionary<string, object> parms) {

    }

    private void PCombat(Dictionary<string, object> parms) {

    }

    private void OCombat(Dictionary<string, object> parms) {

    }

    private void PDiscard(Dictionary<string, object> parms) {

    }

    private void ODiscard(Dictionary<string, object> parms) {

    }

    // State machine code starts here
    public enum GameState {
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

    private delegate void Transition(Dictionary<string, object> parms);

    private TransitionData[] transitionArray;

    private void InitializeTransitionArray() {
        transitionArray = new TransitionData[] {
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
