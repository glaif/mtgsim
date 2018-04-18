using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameStateMachine : MonoBehaviour {
    protected TransitionData currentState;
    protected Dictionary<string, object> stateParams;

    // Game State Machine Functions

    protected void MainGameLoop() {
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

    // Game State Machine Functions

    protected abstract void Join(Dictionary<string, object> parms);

    protected abstract void SelectDeck(Dictionary<string, object> parms);

    protected abstract void PrepStart(Dictionary<string, object> parms);

    protected abstract void Roll(Dictionary<string, object> parms);

    protected abstract void DealCards(Dictionary<string, object> parms);

    protected virtual void WaitMulliganResponse(Dictionary<string, object> parms) {
        // Do nothing
        return;
    }

    protected abstract void Ready(Dictionary<string, object> parms);

    protected abstract void Untap(Dictionary<string, object> parms);

    protected abstract void Upkeep(Dictionary<string, object> parms);

    protected abstract void Draw(Dictionary<string, object> parms);

    protected abstract void Main(Dictionary<string, object> parms);

    protected abstract void Combat(Dictionary<string, object> parms);

    protected abstract void Discard(Dictionary<string, object> parms);


    protected delegate void Transition(Dictionary<string, object> parms);

    protected TransitionData[] transitionArray;

    protected void InitializeTransitionArray() {
        transitionArray = new TransitionData[] {
            new TransitionData(Join, null),
            new TransitionData(SelectDeck, null),
            new TransitionData(PrepStart, null),
            new TransitionData(Roll, null),
            new TransitionData(DealCards, null),
            new TransitionData(WaitMulliganResponse, null),
            new TransitionData(Ready, null),
            new TransitionData(Untap, null),
            new TransitionData(Upkeep, null),
            new TransitionData(Draw, null),
            new TransitionData(Main, null),
            new TransitionData(Combat, null),
            new TransitionData(Discard, null)
        };
    }

    public enum GameState {
        JOIN,
        SELCTDECK,
        PREPSTART,
        ROLL,
        DEAL,
        MULRES,
        READY,
        UNTAP,
        UPKEEP,
        DRAW,
        MAIN,
        COMBAT,
        DISCARD
    };

    public enum ClientStateTypes {
        CST_COUNT,
        CST_MULLIGAN,
        CST_NULL
    };

    protected TransitionData UpdateGameState(GameState state, Dictionary<string, object> newParms) {
        // Turns: untap, upkeep, draw, main, combat, main, discard
        // Adjusts game state and synchronizes with remote player state machine

        //Debug.Log("GameState changed to: " + state);

        TransitionData transData = GetTransition(state);
        if (newParms != null) {
            transData.Parms = newParms;
        }
        return transData;
    }

    public void SyncGameState(GameState state, Dictionary<string, object> newParms) {
        currentState = UpdateGameState(state, newParms);
    }

    protected TransitionData GetTransition(GameState current) {
        return transitionArray[(int)current];
    }

    protected TransitionData GetNextTransition(GameState current) {
        GameState cur = current;
        // Check if we need to wrap around to the beginning of states
        if (cur == GameState.DISCARD) {
            cur = GameState.UNTAP;
        }
        return transitionArray[(int)cur];
    }

    protected class TransitionData {
        public TransitionData(Transition tf, Dictionary<string, object> p) {
            TransFunc = tf;
            Parms = p;
        }

        public Transition TransFunc { get; set; }
        public Dictionary<string, object> Parms { get; set; }
    }

    public static string CST2str(ClientStateTypes cst) {
        string res = null;
        switch (cst) {
            case (ClientStateTypes.CST_COUNT):
                res = "count";
                break;
            case (ClientStateTypes.CST_MULLIGAN):
                res = "mulligan";
                break;
            default:
                Debug.LogError("Error translating CLientStateType to string.");
                break;
        }
        return res;
    }

    public static ClientStateTypes Str2CST(string cstStr) {
        ClientStateTypes res = ClientStateTypes.CST_NULL;
        switch (cstStr) {
            case ("count"):
                res = ClientStateTypes.CST_COUNT;
                break;
            case ("mulligan"):
                res = ClientStateTypes.CST_MULLIGAN;
                break;
            default:
                Debug.LogError("Error translating CLientStateType to string.");
                break;
        }
        return res;
    }
}
