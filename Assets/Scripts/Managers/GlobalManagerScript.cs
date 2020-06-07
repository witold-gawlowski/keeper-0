using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    int roundCount;

    private void Awake()
    {
        summaryUIScript.LevelCompletedEvent += OnLevelCompleted;
        buildingUIScript.BuildingCanceledEvent += OnLevelFinish;
        levelsUIScript.AddRunLevelEventHandler(OnLevelRun);
        StartNewRoundEvent += OnStartNewRound;
        buildingUIScript.RotateButtonTapEvent += blockSpawnerScript.HandleRotEvent;
        buildingUIScript.RotateButtonTapEvent += blockUIQueue.RotateTop;
        dragScript.blockPlacedEvent += blockSpawnerScript.ResetRotation;
        levelMoneyManagerScript.progressUpdatedEvent += buildingUIScript.OnProgressUpdate;
    }

    private void Start()
    {
        roundCount = 1;
        StartNewRoundEvent();
    }

    public void HandleBackButtonTap()
    {
        SceneManager.LoadScene("MainMenuScene", LoadSceneMode.Additive);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            HandleBackButtonTap();
        }
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
        summaryUIScript.LevelCompletedEvent += ()=>levelManagerScript.DestroyLevel(level);
        buildingUIScript.BuildingCanceledEvent += () => levelManagerScript.DestroyLevel(level);
        blockManagerScript.OnStartBuilding(level);
        dragScript.gameObject.SetActive(true);
        dragScript.OnStartBuilding();
        LevelScript levelScritp = level.GetComponent<LevelScript>();
        levelScritp.SetDisplayed(true);
        ProceduralMap levelMap = level.GetComponent<ProceduralMap>();
        levelMoneyManagerScript.Initialize(levelMap.GetFreeArea(),
            levelManagerScript.GetCompletionThreshold(level),
            levelManagerScript.GetRawReward(level));
        levelMoneyManagerScript.SetReturnValue(levelManagerScript.GetReturnValue(level));
        levelsUIScript.DeleteButtonForLevel(level);
        dragScript.SetProceduralMap(level);
        Camera.main.transform.position = levelMap.GetLevelCenterPosition() - new Vector3(0, 3, 10);
        buildingUIScript.OnStartBuilding();
    }

    private void OnLevelCompleted()
    {
        OnLevelFinish();
        accountManager.AddFunds(levelMoneyManagerScript.GetTotalReward());
    }

    private void OnLevelFinish()
    { 
        blockSpawnerScript.ClearAllBlocks();
        dragScript.gameObject.SetActive(false);
        levelManagerScript.HideNotOwnedLevels();
        StartNewRoundEvent();
        blockManagerScript.UpdateInventoryUI();
        roundCount++;
        levelsUIScript.UpdateCompletedLevels(roundCount);
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
        return roundCount;
    }
}
