using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MTGDeckScript : MonoBehaviour {
    GameObject player;

	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player");
        if (player == null) {
            Debug.LogError("MTGDeckScript::Start: Could not find Player GO");
            return;
        }
	}

    public void DrawFromTopOfDeck(int drawCount=1) {
        // Draw drawCount many cards from top of deck
    }

    
}
