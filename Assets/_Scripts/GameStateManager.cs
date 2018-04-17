using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : GameStateMachine {

    public int NumPlayers { get; private set; }

    private PubSubService pss;
    private int desiredPlayers = 2; // TODO: needs to be set by UI

    void Start () {
        Debug.Log("GameStateManager started");
        pss = new PubSubService();
        InitializeTransitionArray();
        currentState = UpdateGameState(GameState.JOIN, null);
    }
	
	void Update () {
        // Execute the main game loop & state machine
        MainGameLoop();
    }


    public void PlayerJoined() {

    }


    protected override void Join(Dictionary<string, object> parms) {
        // Initial game state

        // Wait for players to join
        if (NumPlayers == desiredPlayers) {
            // Signal players to move to select deck state
            foreach (KeyValuePair<string, Opponent> opp in opponentList) {
                PlayerComSC.SetNewState(GameState.SELCTDECK);
            }
            // Move to select deck state
            currentState = UpdateGameState(GameState.SELCTDECK, null);
        }
    }

    protected override void SelectDeck(Dictionary<string, object> parms) {
        // Wait for all players to select their decks
        if (!DeckSelected)
            return;

            currentState = UpdateGameState(GameState.PREPSTART, null);
    }

    protected override void PrepStart(Dictionary<string, object> parms) {
        // Signal players to prep game and let them know about
        // other players' deck sizes -- do we need to do that?
        foreach (KeyValuePair<string, Opponent> opp in opponentList) {
            opp.Value.opponentSC.PrepStartGame(opp.Value.deckSize);
        }

        currentState = UpdateGameState(GameState.ROLL, null);
    }

    protected override void Roll(Dictionary<string, object> parms) {
        // Roll for each player and let them know the outcome
        currentState = UpdateGameState(GameState.DEAL, null);
    }

    protected override void DealCards(Dictionary<string, object> parms) {
        // Do the deal cards logic
    }

    protected override void PReady(Dictionary<string, object> parms) {
        //Debug.Log("PReady firing");
        //playerSC.SendReady();
        //currentState = UpdateGameState(GameState.O_READY, null);
    }

    protected override void OReady(Dictionary<string, object> parms) {
        Debug.Log("OReady firing");
        // Wait here for other player(s) to move into ready state
    }

    protected override void PUntap(Dictionary<string, object> parms) {
        Debug.Log("PUntap firing");
        // Untap all tapped cards
        // Keep a tapped cards list in the player script object
        // to track tapped cards
        // Also need to check each card to make sure it should untap
    }

    protected override void OUntap(Dictionary<string, object> parms) { }

    protected override void PUpkeep(Dictionary<string, object> parms) { }

    protected override void OUpkeep(Dictionary<string, object> parms) { }

    protected override void PDraw(Dictionary<string, object> parms) { }

    protected override void ODraw(Dictionary<string, object> parms) { }

    protected override void PMain(Dictionary<string, object> parms) { }

    protected override void OMain(Dictionary<string, object> parms) { }

    protected override void PCombat(Dictionary<string, object> parms) { }

    protected override void OCombat(Dictionary<string, object> parms) { }

    protected override void PDiscard(Dictionary<string, object> parms) { }

    protected override void ODiscard(Dictionary<string, object> parms) { }
}
