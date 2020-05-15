using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMoneyManagerScript : MonoBehaviour
{
    public BuildingUIScript buildingUIScript;
    public delegate void RewardChangeDelegate(int value);
    RewardChangeDelegate RewardChangeEvent;
    private int reward;
    public DragScript dragScript;
    public accountManager accountManager;
    
    private void Awake()
    { 
        RewardChangeEvent += buildingUIScript.UpdateUI;
        dragScript.blockPlacedEvent += AddReward;
    }

    public void ClearReward()
    {
        reward = 0;
    }

    public void AddReward(int value)
    {
        reward += value;
        RewardChangeEvent(reward);
    }

    public int Reward()
    {
        return reward;
    }
}
