﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalManagerScript : MonoBehaviour
{
    public System.Action StartNewRoundEvent;
    public BuildingUIScript buildingUIScript;
    public LevelManagerScript levelManagerScript;
    public LevelsUIScript levelsUIScript;
    public DragScript dragScript;
    public BlockSpawnerScript blockSpawnerScript;
    public accountManager accountManager;
    public LevelMoneyManagerScript levelMoneyManagerScript;
    public BlockManagerScript blockManagerScript;
    public GlobalUIScript globalUIScript;
    public BlockShopScript blockShopScript;
    public BlocksUIScript blockUIScript;
    public ButtonSortScript buttonSortScript;
    public int startingReward = 50;

    int levelNumber;

    private void Awake()
    {
        buildingUIScript.FinishedBuildingEvent += OnLevelFinish;
        levelsUIScript.AddRunLevelEventHandler(OnLevelRun);
        StartNewRoundEvent += OnStartNewRound;
    }

    private void Start()
    {
        levelNumber = 1;
        StartNewRoundEvent();
    }

    private void OnStartNewRound()
    {
        blockShopScript.OnNewRoundStart();
        levelManagerScript.OnStartNewRound();
        globalUIScript.OnStartOfNewRound();
        buttonSortScript.Sort();
    }

    private void OnLevelRun(GameObject level)
    {
        buildingUIScript.FinishedBuildingEvent += ()=>levelManagerScript.DestroyLevel(level);
        blockManagerScript.OnStartBuilding();
        blockSpawnerScript.OnStartBuilding();
        dragScript.gameObject.SetActive(true);
        dragScript.OnStartBuilding();
        LevelScript levelScritp = level.GetComponent<LevelScript>();
        levelScritp.SetDisplayed(true);
        levelMoneyManagerScript.ClearReward();
        levelMoneyManagerScript.SetReturnValue(levelManagerScript.GetReturnValue(level));
        levelMoneyManagerScript.AddReward(startingReward);
        levelsUIScript.DeleteButtonForLevel(level);
        dragScript.SetProceduralMap(level);
    }


    private void OnLevelFinish()
    { 
        blockSpawnerScript.ClearAllBlocks();
        dragScript.gameObject.SetActive(false);
        levelManagerScript.HideNotOwnedLevels();
        accountManager.AddFunds(levelMoneyManagerScript.Reward());
        levelMoneyManagerScript.ClearReward();
        StartNewRoundEvent();
        blockManagerScript.UpdateInventoryUI();
        levelNumber++;
        levelsUIScript.UpdateCompletedLevels(levelNumber);
    }

    public int GetCurrentLevel()
    {
        return levelNumber;
    }
}
