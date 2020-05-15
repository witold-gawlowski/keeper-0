using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalUIScript : MonoBehaviour
{
    LevelsUIScript levelUIScript;
    public GameObject shopUI;
    public GameObject buildingUI;

    private void Awake()
    {
        levelUIScript = shopUI.GetComponent<LevelsUIScript>();
        levelUIScript.AddRunLevelEventHandler(OnLevelRun);
    }

    void OnLevelRun(GameObject level)
    {
        buildingUI.SetActive(true);
        shopUI.SetActive(false);
    }

    public void OnStartOfNewRound()
    {
        buildingUI.SetActive(false);
        shopUI.SetActive(true);
    }
}
