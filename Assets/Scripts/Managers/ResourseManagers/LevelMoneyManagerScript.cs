using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class LevelMoneyManagerScript : MonoBehaviour
{
    public System.Action<float> progressUpdatedEvent;
    public BuildingUIScript buildingUIScript;
    public System.Action levelCompletedEvent;
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
    private float completionThreshold;
    int rawReward;
    int gems;
    
    
    public void SetReturnValue(float returnValueArg)
    {
        returnValue = returnValueArg;
    }

    private void Awake()
    { 
        dragScript.blockPlacedEvent += BlockPlacedEventHandler;
        levelCompletedEvent += InitializeSummaryUI;
        levelCompletedEvent += buildingUIScript.OnLevelCompleteEvent;
        EventManager.AddListener<GemsFoundEvent>(GemsFoundHandler);
    }

    public void Initialize(int mapTotalFreeAreaArg, float completionThresholdArg, int rawRewardArg)
    {
        completionThreshold = completionThresholdArg;
        mapTotalFreeArea = mapTotalFreeAreaArg;
        rawReward = rawRewardArg;
        totalBlockValue = 0;
        totalBlockArea = 0;
        gems = 0;
    }

    public void BlockPlacedEventHandler(int rewardValueArg, int blockAreaArg)
    {
        AddBlockValue(rewardValueArg, blockAreaArg);
        progressUpdatedEvent(GetCompletedFraction()/completionThreshold);
        CheckCompleteness();
    }

    public int GetGems()
    {
        return gems;
    }

    IEnumerator LevelCompleteDelay()
    {
        yield return new WaitForSeconds(0.5f);
        levelCompletedEvent();
    }

    private void CheckCompleteness()
    {
        if(GetCompletedFraction() >= completionThreshold)
        {
            StartCoroutine(LevelCompleteDelay());
        }
    }

    public void GemsFoundHandler(IEvent evArg)
    {
        GemsFoundEvent evData = evArg as GemsFoundEvent;
        gems += evData.count;
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
        return rawReward;
        //return Mathf.RoundToInt(totalBlockValue * GetRewardMultiplier());
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
