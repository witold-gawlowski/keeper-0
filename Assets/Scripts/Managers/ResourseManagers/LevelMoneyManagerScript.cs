using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMoneyManagerScript : MonoBehaviour
{
    public BuildingUIScript buildingUIScript;
    public delegate void RewardChangeDelegate(int value);
    RewardChangeDelegate RewardChangeEvent;
    private int totalBlockValue;
    public DragScript dragScript;
    public accountManager accountManager;
    private float returnValue;
    private int totalBlockArea;
    public SummaryUIScript summaryUIScript;
    private int mapTotalFreeArea;
    public AnimationCurve completionPercentToRewardMultiplier;
    
    public void SetReturnValue(float returnValueArg)
    {
        returnValue = returnValueArg;
    }

    private void Awake()
    { 
        RewardChangeEvent += buildingUIScript.UpdateUI;
        dragScript.blockPlacedEvent += AddReward;
        buildingUIScript.FinishedBuildingEvent += InitializeSummaryUI;
    }

    public void Initialize(int mapTotalFreeAreaArg)
    {
        mapTotalFreeArea = mapTotalFreeAreaArg;
        totalBlockValue = 0;
        totalBlockArea = 0;
    }

    public void InitializeSummaryUI()
    {
        int completedPercent = Mathf.RoundToInt(1.0f * totalBlockArea / mapTotalFreeArea);
        float rewardMultiplier = completionPercentToRewardMultiplier.Evaluate(completedPercent);
        int totalReward = Mathf.RoundToInt(totalBlockValue * rewardMultiplier);
        summaryUIScript.Init(totalBlockValue, completedPercent, rewardMultiplier, totalReward);
    }

    public void AddReward(int rewardValueArg, int blockAreaArg)
    {
        totalBlockValue += rewardValueArg;
        totalBlockArea += blockAreaArg;
        RewardChangeEvent(totalBlockValue);
    }

    public int Reward()
    {
        return totalBlockValue;
    }
}
