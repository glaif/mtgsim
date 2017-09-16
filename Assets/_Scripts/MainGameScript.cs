using UnityEngine;

public class MainGameScript : MonoBehaviour {
    public GameObject Player;
    public GameObject Opponent;

    // Use this for initialization
    void Start () {
		// Do Photon connect stuff and wait for opponent to join

	}
	
	// Update is called once per frame
	void Update () {
        // Once opponent connects roll d20
        // Winner hosts state machine for the game
        // Loser set up to receive state machine updates from winner
        // Turns: untap, upkeep, draw, main, combat, main, discard
	}
}
