using System.Collections.Generic;
using UnityEngine;

public class MainGameScript : MonoBehaviour {
    private PlayerScript playerSC;
    private GameObject puGO;  // Reference to reusable modal popup window GO
    private PUModalScript puSC;  // Reference to reusable modal popup window script

    public MainGameScript() {
        InitializeTransitionArray();
    }

    void Start() {
        GameObject player = GameObject.Find("Battleground/Player");
        if (player == null) {
            Debug.LogError("Error getting player GO");
        }
        playerSC = player.GetComponent<PlayerScript>();

        puGO = UIGORegistry.Find("PopupModal");
        if (puGO == null) {
            Debug.LogError("Error getting Popup Modal GO");
        }

        puSC = puGO.GetComponent<PUModalScript>();
        if (puSC == null) {
            Debug.LogError("Error getting Popup Modal script component");
        }

        // Do Photon connect stuff and wait for opponent to join
    }

    void Update() {
        // Once opponent connects roll d20
        // Winner hosts state machine for the game
        // Loser set up to receive state machine updates from winner
        // For now, just one player so set htings in motion


    }

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

    // Do the deal cards logic - int count=7, bool mulligan=true
    private void DealCards(Dictionary<string, object> parms) {
        Debug.Log("DealCards firing");
        if (playerSC == null) {
            Debug.LogError("Error null player script object");
            return;
        }
        GameObject deckGO = playerSC.DeckGO;

        if ((bool)parms["mulligan"]) {
            puGO.SetActive(true);

            deckGO.GetComponent<DeckScript>().DealCards((int)parms["count"]);
            
            // Check if the player wants to mulligan
            puSC.SetModalMessage("Take a mulligan?", MulliganPUResponse);
        }
    }

    public void MulliganPUResponse(bool response) {
        if (response) {
            Debug.Log("Mulligan chosen.  Resetting state to P_DEAL");

            HandScript handSC = playerSC.HandSC;
            if (handSC == null) {
                Debug.LogError("Error gatting hand script object");
                return;
            }

            int curCount = handSC.CardCount();
            // Reshuffle the entire hand into the deck
            handSC.RecycleHand();
            // Call dealcards again, with 1 less card in hand
            UpdateGameState(GameState.DEAL, new Dictionary<string, object>() { { "count", curCount - 1 }, { "mulligan", true } });
        } else {
            Debug.Log("No mulligan chosen.  Setting state to P_READY");

            UpdateGameState(GameState.P_READY, null);
        }
    }

    private void PReady(Dictionary<string, object> parms) {
        Debug.Log("PReady firing");
        // Wait here for other player(s) to move into ready state
        // Then move to P_UNTAP or O_UNTAP depending on who goes first
        UpdateGameState(GameState.P_UNTAP, null);
    }

    private void OReady(Dictionary<string, object> parms) {

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
