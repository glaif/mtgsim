using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MTGCardTester : MTGCard {

    public MTGCardTester(string name, long id, int cmc, string colorCost, string setCode) 
        : base(name, id, cmc, colorCost, setCode) {

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
