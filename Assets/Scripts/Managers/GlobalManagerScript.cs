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
    public BlockUIQueue blockUIQueue;

    int levelNumber;

    private void Awake()
    {
        summaryUIScript.LevelQuitEvent += OnLevelFinish;
        levelsUIScript.AddRunLevelEventHandler(OnLevelRun);
        StartNewRoundEvent += OnStartNewRound;
        buildingUIScript.RotateButtonTapEvent += blockSpawnerScript.HandleRotEvent;
        buildingUIScript.RotateButtonTapEvent += blockUIQueue.RotateTop;
        dragScript.blockPlacedEvent += blockSpawnerScript.ResetRotation;
    }

    private void Start()
    {
        levelNumber = 1;
        StartNewRoundEvent();
    }

    private void OnStartNewRound()
    {
        blockShopScript.OnNewRoundStart();
        blockManagerScript.UpdateInventoryUI();
        levelManagerScript.OnStartNewRound();
        globalUIScript.OnStartOfNewRound();
        buttonSortScript.Sort();
    }

    private void OnLevelRun(GameObject level)
    {
        summaryUIScript.LevelQuitEvent += ()=>levelManagerScript.DestroyLevel(level);
        blockManagerScript.OnStartBuilding( level);
        dragScript.gameObject.SetActive(true);
        dragScript.OnStartBuilding();
        LevelScript levelScritp = level.GetComponent<LevelScript>();
        levelScritp.SetDisplayed(true);
        ProceduralMap levelMap = level.GetComponent<ProceduralMap>();
        levelMoneyManagerScript.Initialize(levelMap.GetFreeArea());
        levelMoneyManagerScript.SetReturnValue(levelManagerScript.GetReturnValue(level));
        levelsUIScript.DeleteButtonForLevel(level);
        dragScript.SetProceduralMap(level);
        Camera.main.transform.position = levelMap.GetLevelCenterPosition() - new Vector3(0, 3, 10);
    }


    private void OnLevelFinish()
    { 
        blockSpawnerScript.ClearAllBlocks();
        dragScript.gameObject.SetActive(false);
        levelManagerScript.HideNotOwnedLevels();
        accountManager.AddFunds(levelMoneyManagerScript.GetTotalReward());
        StartNewRoundEvent();
        blockManagerScript.UpdateInventoryUI();
        levelNumber++;
        levelsUIScript.UpdateCompletedLevels(levelNumber);
    }

    private bool CanPlayerContinue()
    {
        int funds = accountManager.GetMoney();
        int MinRosterPrice = levelManagerScript.GetMinRosterPrice();
        if(levelManagerScript.GetOwnedLevelsNumber() ==0)
        {
            if(funds < MinRosterPrice)
            {
                return false;
            }
        }
        return true;
    }

    public int GetCurrentLevel()
    {
        return levelNumber;
    }
}
