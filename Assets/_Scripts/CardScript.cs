using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardScript : MonoBehaviour {

    public Card Card { get; set; }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnMouseDown() {
        Debug.Log("Clicked on card in " + this.transform.parent.name + " zone.");
        this.transform.parent.SendMessage("OnMouseDown", null, SendMessageOptions.DontRequireReceiver);
    }
}
