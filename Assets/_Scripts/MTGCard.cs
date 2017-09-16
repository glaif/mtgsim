
/* 
 * Base class for card and card decorator hierarchy
 * 
 * Author: smaldone@gmail.com
 */

public abstract class MTGCard {
    private string _name;
    private long _id;
    private int _cmc;
    private string _colorCost;
    private string _setCode;

    public MTGCard(string name, long id, int cmc, string colorCost, string setCode) {
        _name = name;
        _id = id;
        _cmc = cmc;
        _colorCost = colorCost;
        _setCode = setCode;
    }

    public string GetName() {
        return _name;
    }

    public long GetID() {
        return _id;
    }

    public int GetCMC() {
        return _cmc;
    }

    public string GetColorCost() {
        return _colorCost;
    }

    public string GetSetCode() {
        return _setCode;
    }

    // Only include functions that should be called in subclass and decorator instances
    public abstract void Cast();
    public abstract void Resolve();
    public abstract void Attack();
}
