using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Text.RegularExpressions;

public class MainMenuScript : MonoBehaviour
{
    public GameObject continueButton;
    public CompletedLevelsManager completedLevelsManager;
    public GameObject leaderboardsPanel;
    public GameObject shopPanel;
    public GameObject deckEditPanel;
    public GameObject hallOfFamePanel;
    public GameObject menuPanel;
    public GameObject DeckEmptyInfoPanel;
    public SceneFader sceneLoader;
    public TextMeshProUGUI gemText;
    public TMP_InputField seedInputField;
    public TextMeshProUGUI seedInputText;
    public SeedMapperScript seedMapper;
    public GameObject tutorialInfoPanel;
    public int maxTutorialLevel = 10;
    GameObject currentPanel;

    private void Awake()
    {
        if (IsMenuSceneLoaded())
        {
            continueButton.SetActive(true);
        }
        else
        {
            continueButton.SetActive(false);
        }

        currentPanel = menuPanel;
        shopPanel.SetActive(false);
        deckEditPanel.SetActive(false);
        hallOfFamePanel.SetActive(false);
        leaderboardsPanel.SetActive(false);
       
        EventManager.AddListener<UpdateGemShopUIEvent>(HandleGemUpdate);
    }

    private void Start()
    {
        if (RunResultScript.instance == null)
        {
            if (completedLevelsManager.IsTutorialRangeCompleted(maxTutorialLevel))
            {
                seedInputField.text = (Random.Range(0, int.MaxValue).ToString());
            }
            else
            {
                seedInputField.text = completedLevelsManager.GetIncompleteTutorialLevel(maxTutorialLevel).ToString();
            }
            
        }
        else
        {
            seedInputField.text = RunResultScript.instance.runNumber.ToString();
        }
       
    }

    public void OnInputSeeedSelected()
    {
        seedInputField.caretPosition = 2;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape) && currentPanel != menuPanel)
        {
            currentPanel.SetActive(false);
            currentPanel = menuPanel;
            menuPanel.SetActive(true);
        }
    }

    public void ClearPlayePrefs()
    {
        print("player prefs cleared!");
        PlayerPrefs.DeleteAll();
    }

    public void HandleGemUpdate(IEvent evArg)
    {
        UpdateGemShopUIEvent evData = evArg as UpdateGemShopUIEvent;
        gemText.text = "<sprite=\"Gem2\" index=0>" + evData.gems;
    }

    bool IsMenuSceneLoaded()
    {
        Scene menuSceneCandidate = SceneManager.GetSceneByName("MenuScene");
        return menuSceneCandidate != null && menuSceneCandidate.name == "MenuScene";
    }

    public void OnStartButtonTap()
    {
        int seedTemp = int.Parse(seedInputField.text);
        
        if (!completedLevelsManager.IsTutorialRangeCompleted(maxTutorialLevel) && (seedTemp <1|| seedTemp > maxTutorialLevel))
        {
            tutorialInfoPanel.SetActive(true);
            int incompleteLevel = completedLevelsManager.GetIncompleteTutorialLevel(maxTutorialLevel);
            seedInputField.SetTextWithoutNotify(incompleteLevel.ToString());
        }
        else if (Deck.instance.IsDeckEmpty())
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
            StartCoroutine(sceneLoader.FadeAndLoadScene(SceneFader.FadeDirection.In, "MenuScene"));
        }
    }

    public void OnContinueButtonTap()
    {
        SceneManager.UnloadSceneAsync("MainMenuScene");
    }

    public void OnDeckEditButtonTap()
    {
        currentPanel.SetActive(false);
        currentPanel = deckEditPanel;
        deckEditPanel.SetActive(true);
    }

    public void OnShopButtonTap()
    {
        currentPanel.SetActive(false);
        currentPanel = shopPanel;
        shopPanel.SetActive(true);
    }

    public void OnLeaderboardsButtonTap()
    {
        currentPanel.SetActive(false);
        currentPanel = leaderboardsPanel;
        leaderboardsPanel.SetActive(true);
    }

    public void OnHallOfFameButtonTap()
    {
        currentPanel.SetActive(false);
        currentPanel = hallOfFamePanel;
        hallOfFamePanel.SetActive(true);
    }
}
