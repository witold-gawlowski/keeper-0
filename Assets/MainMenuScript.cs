using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public GameObject continueButton;
    public GameObject leaderboardsPanel;
    public GameObject shopPanel;
    public GameObject deckEditPanel;
    public GameObject hallOfFamePanel;
    public GameObject menuPanel;

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

    bool IsMenuSceneLoaded()
    {
        Scene menuSceneCandidate = SceneManager.GetSceneByName("MenuScene");
        return menuSceneCandidate != null && menuSceneCandidate.name == "MenuScene";
    }

    public void OnStartButtonTap()
    {
        SceneManager.LoadScene("MenuScene");
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
