using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public GameObject continueButton;
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

}
