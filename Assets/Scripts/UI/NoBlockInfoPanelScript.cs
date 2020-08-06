using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoBlockInfoPanelScript : MonoBehaviour
{

    System.Action callback;
    public void OnOKTap()
    {
        gameObject.SetActive(false);
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
