﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingUIScript : MonoBehaviour
{
    public System.Action RotateButtonTapEvent;
    public Text rewardValueText;

    public System.Action FinishedBuildingEvent;
    public GameObject rotateButton;
    public GameObject summaryPanel;
    public GameObject sellButton;
    public SummaryUIScript summaryUIScript;

    public void OnSellButtonTap()
    {
        summaryPanel.SetActive(true);
        FinishedBuildingEvent();
    }

    public void OnRotateButtonTap()
    {
        RotateButtonTapEvent();
    }

    public void SetRotateButtonEnabled(bool value)
    { 
        rotateButton.SetActive(value);
    }

}
