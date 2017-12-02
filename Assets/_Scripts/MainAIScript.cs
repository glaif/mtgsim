using UnityEngine;

public class MainAIScript : MonoBehaviour, IPlayerCom {
    [SerializeField]
    private GameObject aiPlayerPrefab;

    [SerializeField]
    private GameObject aiObjs;


    void Start () {
        GameObject aiPlayer = Instantiate(aiPlayerPrefab, aiObjs.transform.position, Quaternion.identity);
        if (aiPlayer == null) {
            Debug.LogError("Error trying to instantiate a new AI Player GO");
            return;
        }

        aiObjs = gameObject;
    }

    // RPC Calls from IPlayerCom
    public bool IsMasterClient() {
        return true;
    }

    public void SetNewState(MainGameScript.GameState state) {

    }

    public void SetOppDeckSize(int cardCount) {

    }
}
