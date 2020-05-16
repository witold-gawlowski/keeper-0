using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LevelsUIScript : MonoBehaviour
{
    public Image levelMinimapImage;
    public GameObject LevelMinimapPanel;
    public GameObject levelButtonPrefab;
    public GameObject levelButtonsParent;
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
    }

    public void SelectedLevelBuyButtonTapHandler(GameObject level)
    {
        selectedLevelPanelScript.gameObject.SetActive(false);
    }

    public void SelectedLevelBuildButtonTapHandler(GameObject level)
    {
        selectedLevelPanelScript.gameObject.SetActive(false); 
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

    public void UpdateCompletedLevels(int value)
    {
        completedLevelsText.text = "Level " + value;
    }

    public void UpdateFunds(int value)
    {
        cashText.text = "$" + value;
    }

    public void DeleteButtonForLevel(GameObject level)
    {
        foreach(Transform t in levelButtonsParent.transform)
        {
            LevelButtonScript levelButtonScriptTemp = t.GetComponent<LevelButtonScript>();
            if (levelButtonScriptTemp!=null && levelButtonScriptTemp.GetAssociatedLevel().Equals(level))
            {
                Destroy(t.gameObject);
                return;
            }
        }
    }

    public void TransformBuyButtonToBuildButton(GameObject level)
    {
        foreach (Transform t in levelButtonsParent.transform)
        {
            LevelButtonScript levelButtonScriptTemp = t.GetComponent<LevelButtonScript>();
            if (levelButtonScriptTemp != null && levelButtonScriptTemp.GetAssociatedLevel().Equals(level))
            {
                levelButtonScriptTemp.OnBuy();
            }
        }
    }

    public GameObject SpawnLevelButton(GameObject level, int cost, float returnValueArg)
    { 
        GameObject newLevelButton = Instantiate(levelButtonPrefab, levelButtonsParent.transform);
        newLevelButton.transform.SetParent(levelButtonsParent.transform);
        LevelButtonScript levelButtonScript = newLevelButton.GetComponent<LevelButtonScript>();
        levelButtonScript.Initialize(cost, returnValueArg, level, OnNonBoughtLevelTapEvent, OnBoughtLevelTapEvent);
        return newLevelButton;
    }


    public void OnBackButtonTap()
    {
        selectedLevelPanelScript.gameObject.SetActive(false);
    }


}
