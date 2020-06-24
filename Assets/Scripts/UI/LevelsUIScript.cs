using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelsUIScript : MonoBehaviour
{
    public Image levelMinimapImage;
    public GameObject LevelMinimapPanel;
    public GameObject levelButtonPrefab;
    public GameObject shopItemsParent;
    public GameObject inventoryItemsParent;
    public GameObject buyButton;
    public GameObject buildButton;
    public SelectedLevelPanelScript selectedLevelPanelScript;
    public Text cashText;
    public Text completedLevelsText;


    private void Awake()
    {
        selectedLevelPanelScript.BackButtonTapEvent += OnBackButtonTap;
        selectedLevelPanelScript.LevelBoughtEvent += SelectedLevelBuyButtonTapHandler;
        selectedLevelPanelScript.RunLevelEvent += SelectedLevelBuildButtonTapHandler;
        selectedLevelPanelScript.LevelRemoveEvent += SelectedLevelRemoveButtonTapHandler;
    }

    public void SelectedLevelRemoveButtonTapHandler(GameObject level)
    {
        selectedLevelPanelScript.gameObject.SetActive(false);
    }

    public void SelectedLevelBuyButtonTapHandler(GameObject level)
    {
        selectedLevelPanelScript.gameObject.SetActive(false);
    }

    public void SelectedLevelBuildButtonTapHandler(GameObject level)
    {
        selectedLevelPanelScript.gameObject.SetActive(false); 
    }

    public void SetLevelRemovedEventHandler(System.Action<GameObject> levelRemovedEventHandlerArg)
    {
        selectedLevelPanelScript.LevelRemoveEvent += levelRemovedEventHandlerArg;
    }

    public void SetLevelBoughtEventHandler(System.Action<GameObject> levelBoughtEventHandlerArg)
    {
        selectedLevelPanelScript.LevelBoughtEvent += levelBoughtEventHandlerArg;
    }

    public void AddRunLevelEventHandler(System.Action<GameObject> RunLevelEventHandlerArg)
    {
        selectedLevelPanelScript.RunLevelEvent += RunLevelEventHandlerArg;
    }

    public void OnBoughtLevelTapEvent(GameObject level)
    {
        selectedLevelPanelScript.Initialize(level, true);
        selectedLevelPanelScript.gameObject.SetActive(true);
    }

    public void OnNonBoughtLevelTapEvent(GameObject level)
    {
        selectedLevelPanelScript.Initialize(level, false);
        selectedLevelPanelScript.gameObject.SetActive(true);
    }

    public void UpdateCompletedLevels(int value, int maxLevels)
    {
        completedLevelsText.text = "Level " + value + "/" + maxLevels;
    }

    public void UpdateFunds(int value)
    {
        cashText.text = "$" + value;
    }

    public void DeleteButtonForLevelFromParent(GameObject level, Transform parent)
    {
        foreach (Transform t in parent)
        {
            LevelButtonScript levelButtonScriptTemp = t.GetComponent<LevelButtonScript>();
            if (levelButtonScriptTemp != null && levelButtonScriptTemp.GetAssociatedLevel().Equals(level))
            {
                Destroy(t.gameObject);
                return;
            }
        }
    }

    public void HideNewIconForLevel(GameObject level)
    {
        foreach (Transform t in shopItemsParent.transform)
        {
            LevelButtonScript levelButtonScriptTemp = t.GetComponent<LevelButtonScript>();
            if (levelButtonScriptTemp != null && levelButtonScriptTemp.GetAssociatedLevel().Equals(level))
            {
                levelButtonScriptTemp.HideNewIcon();
            }
        }
    }

    public void DeleteButtonForLevel(GameObject level)
    {
        DeleteButtonForLevelFromParent(level, shopItemsParent.transform);
        DeleteButtonForLevelFromParent(level, inventoryItemsParent.transform);
    }

    public void UpdateRawReward(GameObject level, int valueArg)
    {
        foreach (Transform t in shopItemsParent.transform)
        {
            LevelButtonScript levelButtonScriptTemp = t.GetComponent<LevelButtonScript>();
            if (levelButtonScriptTemp != null && levelButtonScriptTemp.GetAssociatedLevel().Equals(level))
            {
                levelButtonScriptTemp.UpdateRawReward(valueArg);
            }
        }
    }

    public void UpdatePersistence(GameObject level, int valueArg)
    {
        foreach (Transform t in shopItemsParent.transform)
        {
            LevelButtonScript levelButtonScriptTemp = t.GetComponent<LevelButtonScript>();
            if (levelButtonScriptTemp != null && levelButtonScriptTemp.GetAssociatedLevel().Equals(level))
            {
                levelButtonScriptTemp.UpdatePersistence(valueArg);
            }
        }
    }

    public void TransformBuyButtonToBuildButton(GameObject level)
    {
        foreach (Transform t in shopItemsParent.transform)
        {
            LevelButtonScript levelButtonScriptTemp = t.GetComponent<LevelButtonScript>();
            if (levelButtonScriptTemp != null && levelButtonScriptTemp.GetAssociatedLevel().Equals(level))
            {
                levelButtonScriptTemp.OnBuy();
                levelButtonScriptTemp.transform.SetParent(inventoryItemsParent.transform);
                levelButtonScriptTemp.transform.SetAsFirstSibling();
            }
        }
    }

    public GameObject SpawnShopLevelButton(GameObject level, int cost, float returnValueArg, float completionThresholdArg, int rawRewardArg, int persistenceArg)
    { 
        GameObject newLevelButton = Instantiate(levelButtonPrefab, shopItemsParent.transform);
        newLevelButton.transform.SetAsFirstSibling();
        newLevelButton.transform.SetParent(shopItemsParent.transform);
        LevelButtonScript levelButtonScript = newLevelButton.GetComponent<LevelButtonScript>();
        levelButtonScript.Initialize(cost,
            returnValueArg,
            completionThresholdArg,
            level,
            OnNonBoughtLevelTapEvent,
            OnBoughtLevelTapEvent,
            rawRewardArg,
            persistenceArg);
        levelButtonScript.OnBuy();
        return newLevelButton;
    }


    public void OnBackButtonTap()
    {
        selectedLevelPanelScript.gameObject.SetActive(false);
    }


}
