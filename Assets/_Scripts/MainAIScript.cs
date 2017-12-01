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
    public void SendReady() { }

    public void SendPrepStart() { }

    public void SendStartGame(int cardCount) { }
}
