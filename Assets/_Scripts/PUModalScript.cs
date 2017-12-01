using UnityEngine;
using UnityEngine.UI;

public class PUModalScript : MonoBehaviour {
    [SerializeField]
    private GameObject puGO;

    [SerializeField]
    private GameObject puModalTextGO;
    private PUResponse resFunction;

    public delegate void PUResponse(bool response);

    void Start() {
        puGO.SetActive(false);
    }

    public void SetModalMessage(string msg, PUResponse resFunc) {
        if (msg == null) {
            Debug.LogError("Invalid call to SetModalMessage with null msg");
            return;
        }
        puModalTextGO.GetComponent<Text>().text = msg;
        puGO.SetActive(true);
        resFunction = resFunc;
    }

    public void OnAcceptClicked() {
        puGO.SetActive(false);
        resFunction(true);
    }

    public void OnCancelClicked() {
        puGO.SetActive(false);
        resFunction(false);
    }

}
