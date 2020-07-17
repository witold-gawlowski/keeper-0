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
        if (RunResultScript.instance == null)
        {
            seedInputField.text = (Random.Range(0, int.MaxValue).ToString());
        }
        else
        {
            seedInputField.text = RunResultScript.instance.runNumber.ToString();
        }
        EventManager.AddListener<UpdateGemShopUIEvent>(HandleGemUpdate);
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
        //SceneManager.LoadScene("MenuScene");
        if (Deck.instance.IsDeckEmpty())
        {
            DeckEmptyInfoPanel.SetActive(true);
        }
        else
        {
            StartCoroutine(sceneLoader.FadeAndLoadScene(SceneFader.FadeDirection.In, "MenuScene"));
            int seedTemp = int.Parse(seedInputField.text);
            SeedScript.instance.seed = seedTemp;
            if (completedLevelsManager.IsSeedCompleted(seedTemp))
            {
                SeedScript.instance.alreadyCompleted = true;
            }
            else
            {
                SeedScript.instance.alreadyCompleted = false;
            }
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
