using UnityEngine;

public class Opponent {
    public GameObject gameObject { get; private set; }
    public PlayerScript opponentSC { get; private set; }
    public string name { get; private set; }
    public int deckSize { get; set; }

    public Opponent(string name, GameObject go) {
        this.name = name;
        this.gameObject = go;
        this.opponentSC = go.GetComponent<PlayerScript>();
    }
}
