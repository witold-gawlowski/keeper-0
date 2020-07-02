using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalManagerScript : MonoBehaviour
{
    public System.Action OnLevelCompletedActions;
    public System.Action OnLevelFinishedActions;
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
    public BlockShuffleContainer blockShuffleContainer;
    public SceneFader fader;

    int roundCount;

    private void Awake()
    {
        summaryUIScript.LevelCompletedEvent += OnLevelCompleted;
        buildingUIScript.BuildingCanceledEvent += OnLevelFinished;
        levelsUIScript.AddRunLevelEventHandler(OnLevelRun);
        StartNewRoundEvent += OnStartNewRound;
        buildingUIScript.RotateButtonTapEvent += blockSpawnerScript.HandleRotEvent;
        buildingUIScript.RotateButtonTapEvent += blockUIQueue.RotateTop;
        dragScript.blockPlacedEvent += blockSpawnerScript.ResetRotation;
        levelMoneyManagerScript.progressUpdatedEvent += buildingUIScript.OnProgressUpdate;
        levelMoneyManagerScript.levelCompletedEvent += blockShuffleContainer.OnLevelCompleted;
    }

    private void Start()
    {
        roundCount = 1;
        StartNewRoundEvent();
    }

    public void HandleBackButtonTap()
    {
        EventManager.Clear();
        StartCoroutine(fader.FadeAndLoadScene(SceneFader.FadeDirection.In, "MainMenuScene"));
        //SceneManager.LoadScene("MainMenuScene", LoadSceneMode.Single);
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
        levelManagerScript.OnStartNewRound();
        buttonSortScript.Sort();
        levelsUIScript.UpdateCompletedLevels(roundCount, levelManagerScript.GetLevelTarget());
    }

       
    private void OnLevelRun(GameObject level)
    {
        OnLevelCompletedActions += () => levelManagerScript.DestroyLevel(level);
        OnLevelCompletedActions += () => levelsUIScript.DeleteButtonForLevel(level);
        OnLevelFinishedActions += () => level.GetComponent<ProceduralMap>().ClearBlocks();
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
        dragScript.SetProceduralMap(level);
        Camera.main.transform.position = levelMap.GetLevelCenterPosition() - new Vector3(0, 3, 10);
        buildingUIScript.OnStartBuilding();
    }

    private void OnLevelCompleted()
    {
        OnLevelCompletedActions();
        OnLevelFinished();
        accountManager.AddFunds(levelMoneyManagerScript.GetTotalReward());
        roundCount++;
        StartNewRoundEvent();
    }

    private void OnLevelFinished()
    {
        OnLevelFinishedActions();
        OnLevelFinishedActions = null;
        OnLevelCompletedActions = null;
        globalUIScript.OnRoundFinish(); 
        blockSpawnerScript.ClearAllBlocks();
        dragScript.gameObject.SetActive(false);
        levelManagerScript.HideNotOwnedLevels();
        levelManagerScript.DecayLevelRewardsAndHideNewIcons();
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
