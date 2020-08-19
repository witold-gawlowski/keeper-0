using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RunFinishedEvent : IEvent
{
    public int gems;
    public int runNumber;
    public bool completed;
    public RunFinishedEvent(int gemsArg, int runNumberArg, bool completedArg)
    {
        gems = gemsArg;
        runNumber = runNumberArg;
        completed = completedArg;
    }
}

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
    public SummaryUIScript summaryUIScript;
    public BlockUIQueue blockUIQueue;
    public BlockShuffleContainer blockShuffleContainer;
    public SceneFader fader;
    public RunCompletedPanelScript runCompletedPanelScript;
    public LevelCameraFitteer levelCameraFitter;
    public TutorialScript tutorialScript;

    int roundCount;

    private void Awake()
    {
        tutorialScript.Disable();
        summaryUIScript.LevelCompletedEvent += OnLevelCompleted;
        globalUIScript.BuildingCanceledEvent += OnLevelFinished;
        EventManager.AddListener<OpenMapEvent>(OnMapOpen);
        StartNewRoundEvent += OnStartNewRound;
        buildingUIScript.RotateButtonTapEvent += blockSpawnerScript.HandleRotEvent;
        buildingUIScript.RotateButtonTapEvent += blockUIQueue.RotateTop;
        levelMoneyManagerScript.levelCompletedEvent += blockShuffleContainer.OnLevelCompleted;
    }

    private void Start()
    {
        roundCount = 1;
        StartNewRoundEvent();
    }

    public void OnRunCompleted()
    {
        runCompletedPanelScript.gameObject.SetActive(true);
        runCompletedPanelScript.UpdateText(SeedScript.instance.nominalSeed, accountManager.GetGems());
        EventManager.SendEvent(new RunFinishedEvent(accountManager.GetGems(), levelManagerScript.seed, true));
        EventManager.Clear();
    }

    private void OnStartNewRound()
    {
        if (roundCount > levelManagerScript.GetLevelTarget())
        {
            OnRunCompleted();
        }
        else
        {
            blockShopScript.OnNewRoundStart();
            levelManagerScript.OnStartNewRound();

            EventManager.SendEvent(new TopBarUIUpdateEvent(
            levelManagerScript.seed,
            GetCurrentLevel(),
            levelManagerScript.GetLevelTarget(),
            levelMoneyManagerScript.GetGems(), accountManager.GetMoney()));
        }
    }

    private void OnMapOpen(IEvent evArg)
    {
        print("on map open");
        OpenMapEvent evData = evArg as OpenMapEvent;
        GameObject level = evData.levelGO;
        OnLevelCompletedActions += () => levelManagerScript.DestroyLevel(level);
        OnLevelFinishedActions += () => level.GetComponent<Map>().ClearBlocks();
        blockManagerScript.OnStartBuilding(level);
        dragScript.gameObject.SetActive(true);
        dragScript.OnStartBuilding();
        LevelScript levelScritp = level.GetComponent<LevelScript>();
        levelScritp.SetDisplayed(true);
        Map levelMap = level.GetComponent<Map>();
        levelMoneyManagerScript.Initialize(levelMap.GetFreeArea(),
            levelManagerScript.GetCompletionThreshold(level),
            levelManagerScript.GetRawReward(level));
        levelMoneyManagerScript.SetReturnValue(levelManagerScript.GetReturnValue(level));
        dragScript.SetProceduralMap(level);
        Camera.main.transform.position = levelMap.GetCenterPosition() - new Vector3(0, 3, 10);
        buildingUIScript.OnStartBuilding();
        levelCameraFitter.Setup(level);
        tutorialScript.Init();
        blockSpawnerScript.OnMapOpen();
        globalUIScript.OnLevelRun(level);
    }

    private void OnLevelCompleted()
    {
        accountManager.AddGems(levelMoneyManagerScript.GetGems());
        levelsUIScript.DeleteLastSelectedLevel();
        OnLevelCompletedActions();
        OnLevelFinished();
        accountManager.AddFunds(levelMoneyManagerScript.GetTotalReward());
        roundCount++;

        EventManager.SendEvent(new TopBarUIUpdateEvent(
            levelManagerScript.seed,
            roundCount,
            levelManagerScript.GetLevelTarget(),
            accountManager.GetGems(),
            accountManager.GetMoney()));
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
        tutorialScript.Disable();
    }

    public int GetCurrentLevel()
    {
        return roundCount;
    }
}
