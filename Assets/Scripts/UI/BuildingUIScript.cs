using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingUIScript : MonoBehaviour
{
    public System.Action RotateButtonTapEvent;
    public Text rewardValueText;
    public System.Action BuildingCanceledEvent;
    public GameObject rotateButton;
    public GameObject summaryPanel;
    public GameObject sellButton;
    public SummaryUIScript summaryUIScript;
    public BlockUIQueue blockUIQueue;
    public Image progressBarImage;

    public void TriggerRotateEvent()
    {
        RotateButtonTapEvent();
    }
    
    public void OnStartBuilding()
    {
        progressBarImage.fillAmount = 0;
    }

    public void OnLevelCompleteEvent()
    {
        summaryPanel.SetActive(true);
    }

    public void OnExitButtonTap()
    {
        BuildingCanceledEvent();
    }

    public void OnProgressUpdate(float fractionArg)
    {
        progressBarImage.fillAmount = fractionArg;
    }

    public void SetRotateButtonEnabled(bool value)
    { 
        rotateButton.SetActive(value);
    }

}
