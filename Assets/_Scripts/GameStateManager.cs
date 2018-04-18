using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : GameStateMachine {

    private PubSubService pss;
    private int desiredPlayers = 2; // TODO: needs to be set by UI

    // Flags & Counters
    private int numPlayers;
    private int decksSelected;

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

    protected override void Join(Dictionary<string, object> parms) {
        // Initial game state

        // Wait for players to join
        if (numPlayers == desiredPlayers) {
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
        if (decksSelected == numPlayers)
            return;

        foreach (KeyValuePair<string, Opponent> opp in opponentList) {
            PlayerComSC.SetNewState(GameState.PREPSTART);
        }

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

    // TODO: Need a way to decide who is player and who is opponent for each
    // turn.  Maybe set a player order based on rolls results, and a turn 
    // counter per player.  Also need player/opponent abstractions
    // that I can assign client/subscribers to each round.

    protected override void Ready(Dictionary<string, object> parms) { }

    protected override void Untap(Dictionary<string, object> parms) {
        Debug.Log("PUntap firing");
        // Untap all tapped cards
        // Keep a tapped cards list in the player script object
        // to track tapped cards
        // Also need to check each card to make sure it should untap
    }

    protected override void Upkeep(Dictionary<string, object> parms) { }

    protected override void Draw(Dictionary<string, object> parms) { }

    protected override void Main(Dictionary<string, object> parms) { }

    protected override void Combat(Dictionary<string, object> parms) { }

    protected override void Discard(Dictionary<string, object> parms) { }

    public void ProcessClientStateUpdate() { }

    public void OnPlayerJoin() { }
}
