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
    public GameObject oldItemsParent;
    public GameObject buyButton;
    public GameObject buildButton;
    public BlockManagerScript blockManagerScript;
    public SelectedLevelPanelScript selectedLevelPanelScript;
    public Text cashText;
    public Text completedLevelsText;
    public LevelManagerScript levelManagerScript;
    
    public GameObject lastLevelSelected;

    private void Awake()
    {
        selectedLevelPanelScript.BackButtonTapEvent += OnBackButtonTap;
        selectedLevelPanelScript.LevelBoughtEvent += SelectedLevelBuyButtonTapHandler;
        EventManager.AddListener<OpenMapEvent>(SelectedLevelBuildButtonTapHandler);
        selectedLevelPanelScript.LevelRemoveEvent += SelectedLevelRemoveButtonTapHandler;
    }

    public void OnFocus()
    {
        UpdateButtonsDoabilityUI();
    }

    public void UpdateButtonsDoabilityUI()
    {
        UpdateButtonsDoabilityUI(shopItemsParent.transform);
        UpdateButtonsDoabilityUI(inventoryItemsParent.transform);
    }

    void UpdateButtonsDoabilityUI(Transform parentArg)
    {
        foreach(Transform tTemp in parentArg)
        {
            LevelButtonScript lbs = tTemp.GetComponent<LevelButtonScript>();
            int levelArea = lbs.associatedLevel.GetComponent<ProceduralMap>().GetFreeArea();
            int inventoryTotalArea = blockManagerScript.GetTotalInventoryArea();
            float completionFraction = levelManagerScript.GetCompletionThreshold(lbs.associatedLevel);
            if (levelArea* completionFraction <= inventoryTotalArea)
            {
                lbs.SetDefaultLook();
            }
            else
            {
                lbs.SetUnableToCompleteLook();
            }
        }
    }

    public void SelectedLevelRemoveButtonTapHandler(GameObject level)
    {
        selectedLevelPanelScript.gameObject.SetActive(false);
    }

    public void SelectedLevelBuyButtonTapHandler(GameObject level)
    {
        selectedLevelPanelScript.gameObject.SetActive(false);
    }

    public void SelectedLevelBuildButtonTapHandler(IEvent evArg)
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

    public void OnBoughtLevelTapEvent(GameObject level)
    {
        bool inventoryEmpty = blockManagerScript.IsInventoryEmpty();
        selectedLevelPanelScript.Initialize(level, true, inventoryEmpty);
        selectedLevelPanelScript.gameObject.SetActive(true);
        lastLevelSelected = level;
    }

    public void OnNonBoughtLevelTapEvent(GameObject level)
    {
        selectedLevelPanelScript.Initialize(level, false);
        selectedLevelPanelScript.gameObject.SetActive(true);
        lastLevelSelected = level;
    }

    public void UpdateCompletedLevels(int value, int maxLevels)
    {
        completedLevelsText.text = "Level " + value + "/" + maxLevels;
    }

    public void UpdateFunds(int value)
    {
        cashText.text = "$" + value;
    }

    public void DeleteLastSelectedLevel()
    {
        DeleteButtonForLevelFromParent(lastLevelSelected, inventoryItemsParent.transform);
        DeleteButtonForLevelFromParent(lastLevelSelected, oldItemsParent.transform);
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
    public void MoveToOldSection(GameObject level)
    {
        foreach (Transform t in shopItemsParent.transform)
        {
            LevelButtonScript levelButtonScriptTemp = t.GetComponent<LevelButtonScript>();
            if (levelButtonScriptTemp != null && levelButtonScriptTemp.GetAssociatedLevel().Equals(level))
            {
                t.SetParent(oldItemsParent.transform);
                t.SetAsFirstSibling();
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
        DeleteButtonForLevelFromParent(level, oldItemsParent.transform);
    }

    public void UpdateRawReward(GameObject level, int valueArg)
    {
        UpdateRawReward(level, valueArg,  shopItemsParent.transform);
        UpdateRawReward(level, valueArg, oldItemsParent.transform);
    }
    public void UpdateRawReward(GameObject level, int valueArg, Transform parent)
    {
        foreach (Transform t in parent)
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
