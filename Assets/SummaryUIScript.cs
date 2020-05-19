using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SummaryUIScript : MonoBehaviour
{
    public System.Action LevelQuitEvent;
    public Text baseValueText;
    public Text completedPercentText;
    public Text rewardMultiplierText;
    public Text totalRewardText;

    public void OnAcceptButtonTap()
    {
        LevelQuitEvent();
        gameObject.SetActive(false);
    }

    public void Init(int baseValueArg, int completedPercentArg, float rewardMultiplierArg, int totalRewardArg)
    {
        baseValueText.text = "BaseValue: $" + baseValueArg.ToString();
        completedPercentText.text = "Completed " + completedPercentArg.ToString() + "%";
        rewardMultiplierText.text = "Reward multiplier: x" + rewardMultiplierArg.ToString("0.00");
        totalRewardText.text = "Total Reward: $" + totalRewardArg.ToString();
    }
}
