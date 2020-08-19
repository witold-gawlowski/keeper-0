using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelManagerScript : MonoBehaviour
{

    public class LevelData
    {
        public GameObject levelObject;
        public int price;
        public bool isBought;
        public float returnValue;
        public float completionThreshold;
        public int rawReward;
        public int persistence;
        public int age;
        public LevelData(GameObject levelObjectArg,
            int priceArg,
            float returnValueArg,
            bool isBoughtArg,
            float completionThresholdArg,
            int rawRewardArg,
            int persistenceArg,
            int age)
        {
            levelObject = levelObjectArg;
            price = priceArg;
            isBought = isBoughtArg;
            returnValue = returnValueArg;
            completionThreshold = completionThresholdArg;
            rawReward = rawRewardArg;
            persistence = persistenceArg;
        }
    }
    public static LevelManagerScript ins;

    public GameObject levelPrefab;
    List<LevelData> levels;
    public LevelsUIScript levelsUIScript;
    public accountManager accountManager;
    ILevelSpecification runDescription;
    public GlobalManagerScript globalManager;
    public AnimationCurve levelDifficultyMultiplierCurve;
    public float rewardReductionFraction = 0.9f;
    public int rewardReductionConstant = 200;
    int levelTarget;
    public int seed = 101;
    bool seedALreadyCompleted;
    Randomizer randomizer;
    public bool hasBlockInventoryChangedSinceLastLevelUIUpdate;

    private void Awake()
    {
        ins = this;
        seedALreadyCompleted = SeedScript.instance.alreadyCompleted;
        runDescription = GetComponentInChildren<RunSpecificationFactory>();
        System.Object seed = SeedScript.instance.seed;
        if (seed is int)
        {
            int seedInt = (int)seed;
            randomizer = new Randomizer(seedInt);
            RunSpecificationFactory scheduler = runDescription as RunSpecificationFactory;
            scheduler.Init(randomizer);
        }
        else if(seed is RunSpecification)
        {
            RunSpecification seedSpec = seed as RunSpecification;
            runDescription = seedSpec;
        }
        else
        {
            Debug.Log("Wrong seed type!");
        }

        levelsUIScript.SetLevelBoughtEventHandler(BuyLevel);
        levelsUIScript.SetLevelRemovedEventHandler(RemoveLevel);
        levels = new List<LevelData>();
        hasBlockInventoryChangedSinceLastLevelUIUpdate = false;
    }


    public int GetLevelTarget()
    {
        return runDescription.GetTotalLevels();
    }

    public void OnStartNewRound()
    {
        RosterUpdateRoutine();
        levelsUIScript.UpdateButtonsDoabilityUI();
    }


    public int GetMinRosterPrice()
    {
        int result = levels[0].price;
        for(int i=1; i<levels.Count; i++)
        {
            if (result > levels[i].price)
            {
                result = levels[i].price;
            }
        }
        return result;
    }

    public int GetOwnedLevelsNumber()
    {
        int result = 0;
        for(int i=0; i<levels.Count; i++)
        {
            if (levels[i].isBought)
            {
                result++;
            }
        }
        return result;
    }

    public void HideNotOwnedLevels()
    {
        foreach (LevelData levelDataObjeect in levels)
        {
            if (!levelDataObjeect.isBought)
            {
                LevelScript levelScript = levelDataObjeect.levelObject.GetComponent<LevelScript>();
                levelScript.SetDisplayed(false);
            }
        }
    }

    public List<LevelData> GetLevelsData()
    {
        return levels;
    }

    public void RemoveLevel(GameObject levelObjectArg)
    {
        levelsUIScript.DeleteButtonForLevel(levelObjectArg);
        DestroyLevel(levelObjectArg);
    }


    public void DecayLevelRewardsAndHideNewIcons()
    {
        for (int i = levels.Count - 1; i >= 0; i--)
        {
            levelsUIScript.MoveToOldSection(levels[i].levelObject);
            levels[i].rawReward = Mathf.RoundToInt(levels[i].rawReward * rewardReductionFraction - rewardReductionConstant);
            if (levels[i].rawReward <= 0)
            {
                RemoveLevel(levels[i].levelObject);
            }
            else
            {
                levelsUIScript.UpdateRawReward(levels[i].levelObject, levels[i].rawReward);
            }
            //levelsUIScript.HideNewIconForLevel(levels[i].levelObject);
        }
    }

    public void RosterUpdateRoutine()
    {
        int currentLevel = globalManager.GetCurrentLevel();
        int totalRoundCount = runDescription.GetTotalLevels();
        int newLevelsInCurrentRound = runDescription.GetNumberOfNewMaps(currentLevel);
        MapParams nextLevelParams = runDescription.GetMapType(currentLevel);
        for (int i = 0; i < newLevelsInCurrentRound; i++)
        {
            GameObject newLevel = Instantiate(levelPrefab, Vector3.zero, Quaternion.identity);
            newLevel.name = newLevel.name + i;
            MapTextureDrawer snapshotCreatorScript = newLevel.GetComponent<MapTextureDrawer>();
            Map proceduralMap = newLevel.GetComponent<Map>();

            int newLevelCost = 123;
            float newReturnValue = runDescription.GetReward(currentLevel, i);
            int rawReward = runDescription.GetReward(currentLevel, i);
            int persistence = 123; //nextLevelParams.persistenceInterval;
            float newLevelCompletionThresholdFraction = runDescription.GetTarget(currentLevel, i);
            int age = 0;

            levels.Add(new LevelData(newLevel, newLevelCost, newReturnValue, false, newLevelCompletionThresholdFraction, rawReward, persistence, age));
            GameObject newLevelButton = levelsUIScript.SpawnShopLevelButton(newLevel, newLevelCost, newReturnValue, newLevelCompletionThresholdFraction, rawReward, persistence);
            LevelButtonScript newLevelButtonScript = newLevelButton.GetComponent<LevelButtonScript>();
            randomizer = runDescription.GetRandomizer(currentLevel, i);
            proceduralMap.Initialize(randomizer, nextLevelParams, currentLevel, totalRoundCount, seedALreadyCompleted);
            snapshotCreatorScript.createSnapshot();
            Sprite levelSprite = snapshotCreatorScript.GetLevelSprite();
            newLevelButtonScript.SetSprite(levelSprite);
            newLevelButtonScript.Initialize();
        }
    }

    

    private float GetProgressionCostMultiplier()
    {
        int currentLevel = globalManager.GetCurrentLevel();
        return levelDifficultyMultiplierCurve.Evaluate(currentLevel);
    }

    public float GetCompletionThreshold(GameObject level)
    {
        float result = 0;
        foreach (LevelData levelDataObjeect in levels)
        {
            if (levelDataObjeect.levelObject == level)
            {
                result = levelDataObjeect.completionThreshold;
            }
        }
        return result;
    }

    public float GetReturnValue(GameObject level)
    {
        float result = 0;
        foreach (LevelData levelDataObjeect in levels)
        {
            if (levelDataObjeect.levelObject == level)
            {
                result = levelDataObjeect.returnValue;
            }
        }
        return result;
    }

    public int GetPersistence(GameObject level)
    {
        int result = 0;
        foreach (LevelData levelDataObjeect in levels)
        {
            if (levelDataObjeect.levelObject == level)
            {
                result = levelDataObjeect.persistence;
            }
        }
        return result;
    }

    public int GetRawReward(GameObject level)
    {
        int result = 0;
        foreach (LevelData levelDataObjeect in levels)
        {
            if (levelDataObjeect.levelObject == level)
            {
                result = levelDataObjeect.rawReward;
            }
        }
        return result;
    }

    public void DestroyLevel(GameObject level)
    {
        foreach (LevelData levelDataObjeect in levels)
        {
            if(levelDataObjeect.levelObject == level)
            {
                levels.Remove(levelDataObjeect);
                break;
            }
        }
        Destroy(level);
    }

    public void BuyLevel(GameObject level)
    {
        int levelCost = 0;
        for(int i=0; i<levels.Count; i++)
        {
            if (levels[i].levelObject.Equals(level))
            {
                levels[i].isBought = true;
                levelCost = levels[i].price;
            }
        }

        if (accountManager.TryPay(levelCost))
        {
            levelsUIScript.TransformBuyButtonToBuildButton(level);
        }
    }
}
