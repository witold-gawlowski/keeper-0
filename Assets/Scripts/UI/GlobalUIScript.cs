using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalUIScript : MonoBehaviour
{
    LevelsUIScript levelUIScript;
    public GameObject briefing;
    public GameObject inventoryUI;
    public GameObject shopUI;
    public GameObject buildingUI;

    private void Awake()
    {
        levelUIScript = briefing.GetComponent<LevelsUIScript>();
        levelUIScript.AddRunLevelEventHandler(OnLevelRun);
        inventoryUI.SetActive(false);
        shopUI.SetActive(true);
    }
    
    public void OnShopButtonTap()
    {
        inventoryUI.SetActive(false);
        shopUI.SetActive(true);
    }

    public void OnInventoryButtonTap()
    {
        inventoryUI.SetActive(true);
        shopUI.SetActive(false);
    }

    void OnLevelRun(GameObject level)
    {
        buildingUI.SetActive(true);
        briefing.SetActive(false);
    }

    public void OnStartOfNewRound()
    {
        buildingUI.SetActive(false);
        briefing.SetActive(true);
    }
}
