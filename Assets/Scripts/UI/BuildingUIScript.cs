using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class BuildingUIScript : MonoBehaviour
{
    public System.Action RotateButtonTapEvent;
    public Text rewardValueText;
    
    public GameObject rotateButton;
    public GameObject summaryPanel;
    public GameObject sellButton;
    public SummaryUIScript summaryUIScript;
    public BlockUIQueue blockUIQueue;
    public Image progressBarImage;
    public Text barText;
    public TextMeshProUGUI gemText;

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

    public void OnProgressUpdate(float fractionArg)
    {
        progressBarImage.fillAmount = fractionArg;
    }

    public void UpdateGemUI(int countArg)
    {
        gemText.text = "<sprite=\"Gem2\", index=0>" + countArg.ToString();
    }

    public void UpdateBarCaption(int numerator, int denominator)
    {
        barText.text = numerator + "%";
    }

    public void SetRotateButtonEnabled(bool value)
    { 
        rotateButton.SetActive(value);
    }

}
