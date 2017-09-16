using UnityEngine;

public class MainGameScript : MonoBehaviour {
    public GameObject Player;
    public GameObject Opponent;
    private Transition[] transitionArray;

    public MainGameScript() {
        transitionArray = new Transition[] {
            DealCards,
            PUntap,
            PUpkeep,
            PDraw,
            PMain,
            PCombat,
            PDiscard,
            OUntap,
            OUpkeep,
            ODraw,
            OMain,
            OCombat,
            ODiscard
        };
    }

    // Use this for initialization
    void Start() {
        // Do Photon connect stuff and wait for opponent to join

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

        GetTransition(state)();

        return 0;
    }

    private void DealCards() {
        Debug.Log("DealCards firing");
        // Do the deal cards logic
    }

    private void PUntap() {

    }

    private void OUntap() {

    }

    private void PUpkeep() {

    }

    private void OUpkeep() {

    }

    private void PDraw() {

    }

    private void ODraw() {

    }

    private void PMain() {

    }

    private void OMain() {

    }

    private void PCombat() {

    }

    private void OCombat() {

    }

    private void PDiscard() {

    }

    private void ODiscard() {

    }

    // State machine code starts here
    public enum GameState {
        DEAL,
        P_UNTAP,
        P_UPKEEP,
        P_DRAW,
        P_MAIN,
        P_COMBAT,
        P_DISCARD,
        O_UNTAP,
        O_UPKEEP,
        O_DRAW,
        O_MAIN,
        O_COMBAT,
        O_DISCARD
    };

    private delegate void Transition();

    private Transition GetTransition(GameState current) {
        return transitionArray[(int)current];
    }

    private Transition GetNextTransition(GameState current) {
        GameState cur = current;
        // Check if we need to wrap around to the beginning os states
        if (cur == GameState.O_DISCARD) {
            cur = GameState.P_UNTAP;
        }
        return transitionArray[(int)cur];
    }
}
