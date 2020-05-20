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
        dragScript.blockPlacedEvent += AddBlockValue;
        buildingUIScript.FinishedBuildingEvent += InitializeSummaryUI;
    }

    public void Initialize(int mapTotalFreeAreaArg)
    {
        mapTotalFreeArea = mapTotalFreeAreaArg;
        totalBlockValue = 0;
        totalBlockArea = 0;
    }

    public float GetCompletedFraction()
    {
        return 1.0f * totalBlockArea / mapTotalFreeArea;
    }
    
    public float GetRewardMultiplier()
    {
        return completionPercentToRewardMultiplier.Evaluate(GetCompletedFraction());
    }

    public int GetTotalReward()
    {
        return Mathf.RoundToInt(totalBlockValue * GetRewardMultiplier());
    }

    public void InitializeSummaryUI()
    {
        int percentCompleted = Mathf.RoundToInt(GetCompletedFraction() * 100);
        summaryUIScript.Init(totalBlockValue, percentCompleted, GetRewardMultiplier(), GetTotalReward());
    }

    public void AddBlockValue(int rewardValueArg, int blockAreaArg)
    {
        totalBlockValue += rewardValueArg;
        totalBlockArea += blockAreaArg;
    }

    public int GetReward()
    {
        return totalBlockValue;
    }
}
