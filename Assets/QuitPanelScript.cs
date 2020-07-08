using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class QuitPanelScript : MonoBehaviour
{
    public void OnCancelTap()
    {
        gameObject.SetActive(false);
    }

    public void OnOKTap()
    {
        gameObject.SetActive(false);
        EventManager.SendEvent(new RunFinishedEvent(0,-1, false));
    }
}
