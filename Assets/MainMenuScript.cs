using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class MainMenuScript : MonoBehaviour
{
    public GameObject continueButton;
    public GameObject leaderboardsPanel;
    public GameObject shopPanel;
    public GameObject deckEditPanel;
    public GameObject hallOfFamePanel;
    public GameObject menuPanel;
    public SceneFader sceneLoader;
    public TextMeshProUGUI gemText;
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
        StartCoroutine(sceneLoader.FadeAndLoadScene(SceneFader.FadeDirection.In, "MenuScene"));
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
