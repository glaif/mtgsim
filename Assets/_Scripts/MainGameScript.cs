using System.Collections.Generic;
using UnityEngine;

public class MainGameScript : MonoBehaviour {
    public GameObject Player;
    public GameObject Opponent;

    private GameObject deck;
    private GameObject puGO;
    private TransitionData[] transitionArray;

    public MainGameScript() {
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

    // Use this for initialization
    void Start() {
        deck = GameObject.Find("Battleground/Player/Cards/Deck");
        if (deck == null) {
            Debug.LogError("Error getting deck GO");
            return;
        }
        // Do Photon connect stuff and wait for opponent to join
        puGO = GameObject.FindGameObjectWithTag("PopupModal");
    }

    // Update is called once per frame
    void Update() {
        // Once opponent connects roll d20
        // Winner hosts state machine for the game
        // Loser set up to receive state machine updates from winner
        // For now, just one player so set htings in motion


    }

    public int UpdateGameState(GameState state) {
        // Turns: untap, upkeep, draw, main, combat, main, discard
        // Adjusts game state and synchronizes with remote player state machine

        Debug.Log("GameState changed to: " + state);

        TransitionData tData = GetTransition(state);
        tData.TransFunc(tData.Parms);

        return 0;
    }

    // Do the deal cards logic - int count=7, bool mulligan=true
    private void DealCards(Dictionary<string, object> parms) {
        Debug.Log("DealCards firing");
        if (deck == null) {
            Debug.LogError("Error null deck GO");
            return;
        }

        if ((bool)parms["mulligan"]) {
            deck.GetComponent<DeckScript>().DealCards((int)parms["count"]);
            // Check if the player wants to mulligan
            PUModalScript puSC = puGO.GetComponent<PUModalScript>();
            puSC.SetModalMessage("Take a mulligan?", MulliganPUResponse);
        }
    }

    public void MulliganPUResponse(bool response) {

        if (response) {
            // Reshuffle the hand into the deck
            // Call dealcards again, with 6 cards
        } else {
            Debug.Log("No mulligan chosen.  Setting state to P_READY");
            //UpdateGameState(GameState.P_READY);
        }
    }

    private void PReady(Dictionary<string, object> parms) {

    }

    private void OReady(Dictionary<string, object> parms) {

    }

    private void PUntap(Dictionary<string, object> parms) {

    }

    private void OUntap(Dictionary<string, object> parms) {

    }

    private void PUpkeep(Dictionary<string, object> parms) {

    }

    private void OUpkeep(Dictionary<string, object> parms) {

    }

    private void PDraw(Dictionary<string, object> parms) {

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
