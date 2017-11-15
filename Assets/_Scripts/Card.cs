
/* 
 * Base class for card and card decorator hierarchy
 * 
 * Author: smaldone@gmail.com
 */

using UnityEngine;

public abstract class Card {

    public GameObject CardPrefabInst { get; set; }  // Stores refeence to card prefab instance for this card

    public string Name { get; private set; }
    public long Id { get; private set; }
    public int CMC { get; private set; }
    public string ColorCost { get; private set; }
    public string SetCode { get; private set; }

    private bool _tappable;
    private bool _zoneTappable;
    private bool _tapped;

    public Card(string name, long id, int cmc, string colorCost, string setCode) {
        Name = name;
        Id = id;
        CMC = cmc;
        ColorCost = colorCost;
        SetCode = setCode;
        _tappable = true;
        _tapped = false;
        _zoneTappable = true;
    }

    public void SetTappable(bool tappable) {
        _tappable = tappable;
    }

    public void SetZoneTappable(bool tappable) {
        _zoneTappable = tappable;
    }

    public bool IsTappable() {
        /* This should mostly depend on the decorators
         * This is the top-level to override
         * decorator bahavior (e.g., if card is in deck)
         */
        return _tappable && _zoneTappable;
    }

    public void Tap() {
        _tapped = true;
    }

    public void UnTap() {
        _tapped = false;
    }

    public bool IsTapped() {
        return _tapped;
    }

    // Only include functions that should be called in subclass and decorator instances
    public abstract void Cast();
    public abstract void Resolve();
    public abstract void Attack();
}
