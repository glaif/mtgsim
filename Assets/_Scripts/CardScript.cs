using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardScript : MonoBehaviour {

    public Card Card { get; set; }
    bool shiftOn;
    bool enlargedCard;

    void Start () {
        shiftOn = false;
        enlargedCard = false;
    }
	
	void Update () {
        if (Input.GetKey(KeyCode.LeftShift)) {
            shiftOn = true;
        } else {
            shiftOn = false;
        }
    }

    public void OnMouseDown() {
        if (enlargedCard == false) {
            Debug.Log("Clicked on card in " + this.transform.parent.name + " zone.");
            this.transform.parent.SendMessage("OnMouseDown", null, SendMessageOptions.DontRequireReceiver);
        }
    }

    public void OnMouseOver() {
        if (shiftOn && enlargedCard == false) {
            this.transform.localScale = new Vector3(4f, 4f, 0f);
            this.transform.localPosition = new Vector3(this.transform.localPosition.x, 1.5f, -1f);
            enlargedCard = true;
        } else if (shiftOn == false && enlargedCard == true) {
            this.transform.localScale = new Vector3(1, 1, 1);
            this.transform.localPosition = new Vector3(this.transform.localPosition.x, 0f, 0);
            enlargedCard = false;
        }
    }

    public void OnMouseExit() {
        if (enlargedCard == true) {
            this.transform.localScale = new Vector3(1, 1, 1);
            this.transform.localPosition = new Vector3(this.transform.localPosition.x, 0f, 0);
            enlargedCard = false;
        }
    }
}
