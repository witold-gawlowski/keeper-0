using System.Collections;
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
    public SummaryUIScript summaryUIScript;
    public int startingReward = 50;

    int levelNumber;

    private void Awake()
    {
        summaryUIScript.LevelQuitEvent += OnLevelFinish;
        levelsUIScript.AddRunLevelEventHandler(OnLevelRun);
        StartNewRoundEvent += OnStartNewRound;
        buildingUIScript.RotateButtonTapEvent += dragScript.HandleRotButtonTap;
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
        summaryUIScript.LevelQuitEvent += ()=>levelManagerScript.DestroyLevel(level);
        blockManagerScript.OnStartBuilding( level);
        blockSpawnerScript.OnStartBuilding();
        dragScript.gameObject.SetActive(true);
        dragScript.OnStartBuilding();
        LevelScript levelScritp = level.GetComponent<LevelScript>();
        levelScritp.SetDisplayed(true);
        ProceduralMap levelMap = level.GetComponent<ProceduralMap>();
        levelMoneyManagerScript.Initialize(levelMap.GetFreeArea());
        levelMoneyManagerScript.SetReturnValue(levelManagerScript.GetReturnValue(level));
        levelMoneyManagerScript.AddReward(startingReward, 0);
        levelsUIScript.DeleteButtonForLevel(level);
        dragScript.SetProceduralMap(level);
        Camera.main.transform.position = levelMap.GetLevelCenterPosition() - new Vector3(0, 3, 10);
    }


    private void OnLevelFinish()
    { 
        blockSpawnerScript.ClearAllBlocks();
        dragScript.gameObject.SetActive(false);
        levelManagerScript.HideNotOwnedLevels();
        accountManager.AddFunds(levelMoneyManagerScript.Reward());
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
