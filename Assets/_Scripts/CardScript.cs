﻿using UnityEngine;

public class CardScript : MonoBehaviour {

    public Card Card { get; set; }

    private bool shiftOn;
    private bool enlargedCard;
    private Quaternion newRotation;

    void Start () {
        shiftOn = false;
        enlargedCard = false;
    }
	
	void Update () {
        if (this.transform.rotation != newRotation) {
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, newRotation, 0.1f);
        }

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

        if ((Card.IsTappable() == true) && (Card.IsTapped() == false)) {
            TapCard();
        } else if ((Card.IsTappable()) && (Card.IsTapped() == true)) {
            UntapCard();
        }
    }

    public void OnMouseOver() {
        if (shiftOn && enlargedCard == false) {
            this.transform.localScale = new Vector3(this.transform.localScale.x * 4f, this.transform.localScale.y * 4f, 1f);
            this.transform.localPosition = new Vector3(this.transform.localPosition.x, 3f, -1f);
            enlargedCard = true;
        } else if (shiftOn == false && enlargedCard == true) {
            this.transform.localScale = new Vector3(this.transform.localScale.x / 4f, this.transform.localScale.y / 4f, 1f);
            this.transform.localPosition = new Vector3(this.transform.localPosition.x, 0f, 0);
            enlargedCard = false;
        }
    }

    public void OnMouseExit() {
        if (enlargedCard == true) {
            this.transform.localScale = new Vector3(this.transform.localScale.x / 4f, this.transform.localScale.y / 4f, 1f);
            this.transform.localPosition = new Vector3(this.transform.localPosition.x, 0f, 0);
            enlargedCard = false;
        }
    }

    private void TapCard() {
        //this.transform.rotation = Quaternion.Euler(0, 0, 90);
        newRotation = Quaternion.Euler(0, 0, 90);
        Card.Tap();
    }

    private void UntapCard() {
        newRotation = Quaternion.Euler(0, 0, 0);
        //this.transform.rotation = Quaternion.Euler(0, 0, 0);
        Card.UnTap();
    }
}
