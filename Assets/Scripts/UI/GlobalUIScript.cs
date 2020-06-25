using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalUIScript : MonoBehaviour
{
    public static GlobalUIScript instance;

    LevelsUIScript levelUIScript;
    public GameObject briefing;
    public GameObject inventoryUI;
    public GameObject shopUI;
    public GameObject buildingUI;
    RectTransform rectTransform;

    private void Awake()
    {
        instance = this;

        rectTransform = GetComponent<RectTransform>();
        levelUIScript = briefing.GetComponent<LevelsUIScript>();
        levelUIScript.AddRunLevelEventHandler(OnLevelRun);
        inventoryUI.SetActive(false);
        shopUI.SetActive(true);
    }

    public float GetButtonDiameter()
    {
        return (Mathf.Min(rectTransform.rect.width, rectTransform.rect.height) - 50) / 4;
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

    public void OnRoundFinish()
    {
        buildingUI.SetActive(false);
        briefing.SetActive(true);
    }

}
