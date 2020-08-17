using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StartNewGameButtonScript : MonoBehaviour
{
    public Image plusPadlock;
    public Text plusButtonText;
    public Button plusButton;
    public Button minusButton;
    [Header("inputFieldCompletedColorBlock")]
    public ColorBlock inputFieldCompletedColorBlock;
    [Header("inputFiledIncompleteColorBlock")]
    public ColorBlock inputFiledIncompleteColorBlock;
    public TMP_InputField seedInputField;
    public CompletedLevelsManager completedLevelsManager;
    public TextMeshProUGUI seedInputText;
    public GameObject tutorialInfoPanel;
    public GameObject DeckEmptyInfoPanel;
    public SeedMapperScript seedMapper;
    public SceneFader sceneFader;

    private void Start()
    {
        if (RunResultScript.instance == null)
        {
            seedInputField.text = (completedLevelsManager.LowestIncompleteLevel()).ToString();
        }
        else
        {
            seedInputField.text = RunResultScript.instance.runNumber.ToString();
            if (RunResultScript.instance.completed)
            {
                RunLevelCompletedAnim();
                //if it was the final level needed to complete the threshold then
                //run the padlock breaking animation
            }
        }
    }

    public void RunLevelCompletedAnim()
    {

    }

    public void OnEditEnd(string value)
    {
        int enteredValue = int.Parse(value);
        if (enteredValue > completedLevelsManager.GetMaxLevel())
        {
            DisplayThresholdsInfo();
        }
        else if (enteredValue <= 0)
        {
            DisplayPositiveValueInfo();
        }
        else if (completedLevelsManager.IsSeedCompleted(enteredValue))
        {
            SetLevelSelectionUILooksToLevelCompleted();
        }
        else
        {
            SetLevelSelectionUILooksToLevelNotCompleted();
        }
    }

    void DisplayPositiveValueInfo()
    {
        tutorialInfoPanel.SetActive(true);
        InfoPanelScript ips = tutorialInfoPanel.GetComponent<InfoPanelScript>();
        ips.SetText("You must enter a positive value.");
    }

    void DisplayThresholdsInfo()
    {
        int threshold = completedLevelsManager.NearestThreshold();
        int currentCompleted = completedLevelsManager.GetCompletedLevelsNumber();
        int maxLevel = completedLevelsManager.GetMaxLevel();
        int nextTierMax = completedLevelsManager.NextMaxLevel();
        tutorialInfoPanel.SetActive(true);
        InfoPanelScript ips = tutorialInfoPanel.GetComponent<InfoPanelScript>();
        ips.SetText("You need to complete " + (threshold - currentCompleted).ToString()
            + " more levels to unlock levels " + (maxLevel + 1).ToString() + "-" +
            nextTierMax.ToString() + ".");//add bold!
    }

    void SetLevelSelectionUILooksToLevelCompleted()
    {
        seedInputField.colors = inputFieldCompletedColorBlock;
    }

    void SetLevelSelectionUILooksToLevelNotCompleted()
    {
        seedInputField.colors = inputFiledIncompleteColorBlock;
    }

    public void OnPlusButtonTap()
    {
        int initialInputFieldValue = int.Parse(seedInputField.text);
        if(initialInputFieldValue == completedLevelsManager.GetMaxLevel())
        {
            DisplayThresholdsInfo();
        }
        else
        {
            seedInputField.text = (initialInputFieldValue + 1).ToString();
            UpdateLevelSelectionUI();
        }
    }

    public void OnMinusButtonTap()
    {
        int initialInputFieldValue = int.Parse(seedInputField.text);
        if (initialInputFieldValue < 1)
        {
            seedInputField.text = (int.Parse(seedInputField.text) - 1).ToString();
            UpdateLevelSelectionUI();
        }
        else
        {
            DisplayPositiveValueInfo();
        }
    }

    void UpdateLevelSelectionUI()
    {
        if (int.Parse(seedInputField.text) == completedLevelsManager.GetMaxLevel())
        {
            SetPlusButtonLocked(true);
        }
        else
        {
            SetPlusButtonLocked(false);
        }
    }

    void SetPlusButtonLocked(bool value)
    {
        if (value == false)
        {
            plusButtonText.enabled = false;
            plusPadlock.enabled = true;
        }
        else
        {
            plusButtonText.enabled = true;
            plusPadlock.enabled = false;
        }
    }

    void SetMinusButtonEnabled(bool value)
    {
        minusButton.enabled = value;
    }

    public void OnStartButtonTap()
    {
        int seedTemp = int.Parse(seedInputField.text);
        if (Deck.instance.IsDeckEmpty())
        {
            DeckEmptyInfoPanel.SetActive(true);
        }
        else
        {
            if (completedLevelsManager.IsSeedCompleted(seedTemp))
            {
                SeedScript.instance.alreadyCompleted = true;
            }
            else
            {
                SeedScript.instance.alreadyCompleted = false;
            }
            SeedScript.instance.seed = seedMapper.GetSeed(seedTemp);
            SeedScript.instance.nominalSeed = seedTemp;
            StartCoroutine(sceneFader.FadeAndLoadScene(SceneFader.FadeDirection.In, "MenuScene"));
        }
    }
}
