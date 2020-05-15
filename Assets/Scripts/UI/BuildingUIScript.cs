using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingUIScript : MonoBehaviour
{
    public Text rewardValueText;
    public delegate void FinishedBuilding();
    public FinishedBuilding FinishedBuildingEvent;

    public void OnSellButtonTap()
    {
        FinishedBuildingEvent();
    }

    public void UpdateUI(int value)
    {
        rewardValueText.text = "+$" + value.ToString();
    }
}
