using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class RunCompletedPanelScript : MonoBehaviour
{
    public SceneFader fader;
    public TextMeshProUGUI text;

    public void UpdateText(int runNumber, int gemNumber)
    {
        text.text = "You have completed run #"+ runNumber+"! You have found "+gemNumber+" gems on the way! Contratulations!";
    }

    public void OnContiunueTap()
    {
        print("on continue tap");
        StartCoroutine(fader.FadeAndLoadScene(SceneFader.FadeDirection.In, "MainMenuScene"));
        EventManager.Clear();
    }
}
