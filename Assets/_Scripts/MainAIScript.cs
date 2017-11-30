using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainAIScript : MonoBehaviour {
    [SerializeField]
    private GameObject aiPlayerPrefab;

    [SerializeField]
    private GameObject aiObjs;

    // Use this for initialization
    void Start () {
        GameObject aiPlayer = Instantiate(aiPlayerPrefab, aiObjs.transform.position, Quaternion.identity);
        if (aiPlayer == null) {
            Debug.LogError("Error trying to instantiate a new AI Player GO");
            return;
        }

        aiObjs = gameObject;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
