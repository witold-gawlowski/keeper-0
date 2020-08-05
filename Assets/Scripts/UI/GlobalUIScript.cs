using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TopBarUIUpdateEvent:IEvent
{
    public int? runNumber;
    public int? levelNumber;
    public int? targetLevelNumber;
    public int? gemsCollected;
    public int? totalCash;

    public TopBarUIUpdateEvent(int? runNumberArg, int? levelNumberArg, int? targetLevelNumberArg, int? gemsCollectedArg, int? totalCahsArg)
    {
        runNumber = runNumberArg;
        levelNumber = levelNumberArg;
        targetLevelNumber = targetLevelNumberArg;
        gemsCollected = gemsCollectedArg;
        totalCash = totalCahsArg;
    }
}

public class GlobalUIScript : MonoBehaviour
{

    public System.Action BuildingCanceledEvent;
    LevelsUIScript levelUIScript;
    public GameObject briefing;
    public GameObject inventoryUI;
    public GameObject shopUI;
    public GameObject buildingUI;

    public TextMeshProUGUI gemsText;
    public TextMeshProUGUI cashText;
    public Text completenessText;
    public TextMeshProUGUI runNumberText;
    public Image completnessBar;
    
    public GameObject quitConfirmPanel;


    private void Awake()
    {
        levelUIScript = briefing.GetComponent<LevelsUIScript>();
        inventoryUI.SetActive(true);
        shopUI.SetActive(false);
        buildingUI.SetActive(false);
        briefing.SetActive(true);
        EventManager.AddListener<TopBarUIUpdateEvent>(BriefingTopBarUIUpdateDispatcher);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (briefing.activeInHierarchy)
            {
                HandleBriefingBackButtonTap();
            }
            else if (buildingUI.activeInHierarchy)
            {
                HandleBuildingBackbuttonTap();
            }
            
        }
    }

    void HandleBuildingBackbuttonTap()
    {
        BuildingCanceledEvent();
    }

    public void HandleBriefingBackButtonTap()
    {
        quitConfirmPanel.SetActive(true);
    }

    public void BriefingTopBarUIUpdateDispatcher(IEvent evArg)
    {
        TopBarUIUpdateEvent evData = evArg as TopBarUIUpdateEvent;
        if (evData.gemsCollected.HasValue)
        {
            gemsText.text = "+<sprite=\"Gem2\" index=0>" + evData.gemsCollected;
        }
        if (evData.totalCash.HasValue)
        {
            cashText.text = "$" + evData.totalCash;
        }
        if (evData.levelNumber.HasValue && evData.targetLevelNumber.HasValue)
        {
            completenessText.text = evData.levelNumber-1 + "/" + evData.targetLevelNumber;
            completnessBar.fillAmount = 1.0f * (float)(evData.levelNumber-1) / (float)evData.targetLevelNumber;
        }
        if (evData.runNumber.HasValue)
        {
            runNumberText.text = "#" + evData.runNumber.ToString();
        }
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
        levelUIScript.OnFocus();
    }

    public void OnLevelRun(GameObject level)
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
