using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class QuitPanelScript : MonoBehaviour
{
    public SceneFader fader;
    public void OnCancelTap()
    {
        gameObject.SetActive(false);
    }

    public void OnOKTap()
    {
        EventManager.SendEvent(new RunFinishedEvent(0,-1, false));
        StartCoroutine(fader.FadeAndLoadScene(SceneFader.FadeDirection.In, "MainMenuScene"));
        EventManager.Clear();
    }
}
