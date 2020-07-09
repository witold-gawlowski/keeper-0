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

    public GameObject levelPrefab;
    List<LevelData> levels;
    public LevelsUIScript levelsUIScript;
    public accountManager accountManager;
    LevelSchedulerScript levelScheduler;
    public GlobalManagerScript globalManager;
    public AnimationCurve levelDifficultyMultiplierCurve;
    public float rewardReductionFraction = 0.9f;
    public int rewardReductionConstant = 200;
    int levelTarget;
    public int seed = 101;
    Randomizer randomizer;

    private void Awake()
    {
        seed = SeedScript.instance.seed;
        randomizer = new Randomizer(seed);
        levelScheduler = GetComponentInChildren<LevelSchedulerScript>();
        levelScheduler.Init(randomizer);
        levelsUIScript.SetLevelBoughtEventHandler(BuyLevel);
        levelsUIScript.SetLevelRemovedEventHandler(RemoveLevel);
        levels = new List<LevelData>();
        levelScheduler.Create();
    }


    public int GetLevelTarget()
    {
        return levelScheduler.GetLevelCount();
    }

    public void OnStartNewRound()
    {
        RosterUpdateRoutine();
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
        int newLevelsInCurrentRound = levelScheduler.GetNumberOfNewMaps(currentLevel);
        LevelTypeScriptableObjectScript nextLevelParams = levelScheduler.GetMapType(currentLevel);
        for (int i = 0; i < newLevelsInCurrentRound; i++)
        {
            GameObject newLevel = Instantiate(levelPrefab, Vector3.zero, Quaternion.identity);
            newLevel.name = newLevel.name + i;
            SnapshotCreatorScript snapshotCreatorScript = newLevel.GetComponent<SnapshotCreatorScript>();
            ProceduralMap proceduralMap = newLevel.GetComponent<ProceduralMap>();
            
            int newLevelCost = Mathf.RoundToInt(nextLevelParams.GetCost(randomizer) * GetProgressionCostMultiplier()/10)*10;
            float newReturnValue = nextLevelParams.GetReturnValue(randomizer);
            int rawReward = nextLevelParams.rewardValue;
            int persistence = nextLevelParams.persistenceInterval;
            float newLevelCompletionThresholdFraction = nextLevelParams.GetCompletionThresholdFraction(randomizer);
            int age = 0; 
            levels.Add(new LevelData(newLevel, newLevelCost, newReturnValue, false, newLevelCompletionThresholdFraction, rawReward, persistence, age));
            GameObject newLevelButton = levelsUIScript.SpawnShopLevelButton(newLevel, newLevelCost, newReturnValue, newLevelCompletionThresholdFraction, rawReward, persistence);
            LevelButtonScript newLevelButtonScript = newLevelButton.GetComponent<LevelButtonScript>();
            snapshotCreatorScript.finishedGeneratingSnapshotEvent += newLevelButtonScript.SetSprite;
            proceduralMap.Initialize(randomizer, nextLevelParams);
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
