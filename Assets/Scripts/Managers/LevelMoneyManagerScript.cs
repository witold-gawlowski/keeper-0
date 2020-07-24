using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class LevelMoneyManagerScript : MonoBehaviour
{
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
    public float completionThreshold;
    int rawReward;
    int gems;
    public bool isLevelCompleted;
    
    
    public void SetReturnValue(float returnValueArg)
    {
        returnValue = returnValueArg;
    }

    private void Awake()
    { 
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
        buildingUIScript.UpdateBarCaption(0, Mathf.RoundToInt(completionThresholdArg));
        buildingUIScript.UpdateGemUI(0);
        isLevelCompleted = false;
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

    public void CheckCompleteness()
    {
        if(GetCompletedFraction() >= completionThreshold)
        {
            isLevelCompleted = true;
            StartCoroutine(LevelCompleteDelay());
        }
    }

    public void GemsFoundHandler(IEvent evArg)
    {
        GemsFoundEvent evData = evArg as GemsFoundEvent;
        gems += evData.count;
        buildingUIScript.UpdateGemUI(gems);
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
