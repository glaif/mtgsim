using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClientStateManager : GameStateMachine {
    // State Flags
    private bool deckSelected = false;
    private bool deckCountSent = false;
    private bool localServer = false;
    private bool connectedToPSS = false;
    private bool gameJoined = false;

    private CallbackServiceServer css;
    private PubSubClient psc;

    private string playerName;
    private string comSvcIPaddr;
    private string comSvcPort;

    // TESTING STUFF
    public GameObject TestMessageGO;

    void Start () {
        Debug.Log("ClientStateManager started");
        css = new CallbackServiceServer();
        css.StartService();
        InitializeTransitionArray();
        currentState = UpdateGameState(GameState.JOIN, null);
        deckSelected = false;
    }
	
	void Update () {
        // Execute the main game loop & state machine
        if (connectedToPSS == false)
            return;
        MainGameLoop();
    }

    public void ConnectToLocalServer() {
        localServer = true;
        ConnectToServer("127.0.0.1", "50051");
    }

    public bool ConnectToServer(string ipaddr, string port) {
        this.comSvcIPaddr = ipaddr;
        this.comSvcPort = GameConstants.DefaultComServicePort;
        if ((port != null) && (port != "")) {
            this.comSvcPort = port;
        }

        if (((ipaddr == "127.0.0.1") || (ipaddr == "localhost")) 
                && (localServer == false)) {
            Debug.Log("Cannot connect to local com service unless you are the host client");
            return false;
        }

        if (psc == null) {
            psc = new PubSubClient(ipaddr, comSvcPort, GetPlayerName());
        }

        connectedToPSS = psc.ClientConnect();
        if (connectedToPSS == false) {
            Debug.LogError("Error connecting to PubSubService at Host.");
        }
        return connectedToPSS;
    }

    private bool JoinGame(string playerName, string clientUri) {
        if (gameJoined == true)
            return true;

        if (psc.Subscribe() == true) {
            gameJoined = true;
            return true;
        }

        // need to call stateupdate to notify host that you have joined the game
        Debug.LogError("Error: failed to join game.");
        return false;
    }


    private void DisplayState(GameState state) {
        TestMessageGO.GetComponent<Text>().text = "STATE = " + state.ToString();
    }

    private string GetPlayerName() {
        if (playerName == null) {
            Debug.LogError("Error: Trying to fetch null player name");
        }
        return playerName;
    }

    protected override void Join(Dictionary<string, object> parms) {
        // Initial game state
        // Client sends a message to join the game
        JoinGame(null, null);
    }

    protected override void SelectDeck(Dictionary<string, object> parms) {
        if (deckSelected == false)
            return;

        // Client sends its deck size to Master
        // This is set by a Client after sending deck count to Master
        if (deckCountSent)
            return;

        PlayerComSC.SetOppDeckSize(playerSC.deckSC.GetDeckCount());
        deckCountSent = true;
    }

    protected override void PrepStart(Dictionary<string, object> parms) {
        playerSC.PrepStartGame();
        currentState = UpdateGameState(GameState.ROLL, null);
    }

    protected override void Roll(Dictionary<string, object> parms) {
        stateParams = new Dictionary<string, object>() { { "count", GameConstants.StartingHandSize }, { "mulligan", true } };
        currentState = UpdateGameState(GameState.DEAL, stateParams);
    }

    // Do the deal cards logic - int count=7, bool mulligan=true
    protected override void DealCards(Dictionary<string, object> parms) {
        if ((bool)parms["mulligan"]) {
            popupGO.SetActive(true);

            playerSC.DealCards((int)parms["count"]);

            // Check if the player wants to mulligan
            popupSC.SetModalMessage("Take a mulligan?", MulliganPUResponse);
        }
        currentState = UpdateGameState(GameState.MULRES, null);
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
            currentState = UpdateGameState(GameState.READY, null);
        }
    }

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
}
