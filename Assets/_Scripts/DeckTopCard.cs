using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckTopCard : Card {

    public DeckTopCard()
        : base("DeckTopCard", 0, 0, "", "") {

    }

    public override void Attack() {
        throw new System.NotImplementedException();
    }

    public override void Cast() {
        throw new System.NotImplementedException();
    }

    public override void Resolve() {
        throw new System.NotImplementedException();
    }
}
