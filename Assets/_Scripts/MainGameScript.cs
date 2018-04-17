using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainGameScript : MonoBehaviour {
    private GameStateManager gsm = null;

    public GameObject Opponent1GO;
    public GameObject Opponent2GO;
    public GameObject Opponent3GO;
    public GameObject popupGO;     // Reference to reusable modal popup window GO
    public PUModalScript popupSC;  // Reference to reusable modal popup window script
    public PlayerScript playerSC;

    public IPlayerCom PlayerComSC { get; set; }

    private Dictionary<string, Opponent> opponentList;
    

    void Start() {
        Debug.Log("MainGameScript started");
    }

    void Update() {

    }

    public void UpdateOppDeckCount(string playerName, int cardCount) {
        // Once we recieve the Client deck count at the Master
        // update the opponent object and signal the Client
        // to move onto the next state
        Debug.Log("UpdateOppDeckCount called by: " + playerName + " - Master: " + PlayerComSC.IsMasterClient());
        UpdateOpponent(playerName, GameConstants.OppCardCountProp, cardCount);
        PlayerComSC.SetNewState(GameState.PREPSTART);
    }

    private void UpdateOpponent(string playerName, string prop, object value) {
        Opponent opp = opponentList[playerName];

        switch (prop) {
            case (GameConstants.OppCardCountProp):
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

}
