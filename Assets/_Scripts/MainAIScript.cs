using UnityEngine;

public class MainAIScript : MonoBehaviour, IPlayerCom {
    [SerializeField]
    private GameObject aiPlayerPrefab;

    [SerializeField]
    private GameObject aiObjs;

    private bool masterClient = false;

    void Start () {
        Debug.Log("Starting main AI script: masterClient == " + masterClient);

        GameObject aiPlayer = Instantiate(aiPlayerPrefab, aiObjs.transform.position, Quaternion.identity);
        if (aiPlayer == null) {
            Debug.LogError("Error trying to instantiate a new AI Player GO");
            return;
        }

        aiObjs = gameObject;
    }

    // Methods from IPlayerCom

    public void SetPlayerName(string name) { }

    public string GetPlayerName() {
        return null;
    }

    public bool IsMasterClient() {
        return true;
    }

    public void SetMasterClient() {
        masterClient = true;
    }

    public void SetNewState(MainGameScript.GameState state) {

    }

    public void SetOppDeckSize(int cardCount) {

    }
}
