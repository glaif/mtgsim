using UnityEngine;
using System.Collections;

public class DSMenuScript : MonoBehaviour {
    private GameObject dsgo;

    // Use this for initialization
    void Start () {
        dsgo = GameObject.FindGameObjectWithTag("DeckSelectMenu");
        if (dsgo == null) {
            Debug.Log("Null GameObject reference for DeckSelectMenu");
            Application.Quit();
        }
        MyRegistry.Register(dsgo.gameObject);
        dsgo.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void CancelClick() {
        dsgo.SetActive(false);
    }
}
