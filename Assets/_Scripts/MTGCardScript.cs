using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MTGCardScript : MonoBehaviour {

    public MTGCard Card { get; set; }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnMouseDown() {
        Debug.Log("Clicked on card in " + this.transform.parent.name + " zone.");
    }
}
