using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanelScript : MonoBehaviour
{
    public Text text;
    System.Action callback;
    public void OnOKTap()
    {
        gameObject.SetActive(false);
    }

    public void SetText(string textArg)
    {
        text.text = textArg;
    }

    public void OnYesTap()
    {
        callback();
        gameObject.SetActive(false);
    }

    public void Init(System.Action callbackArg)
    {
        this.callback = callbackArg;
    }
}
