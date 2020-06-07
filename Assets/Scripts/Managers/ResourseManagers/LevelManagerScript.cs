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
        public LevelData(GameObject levelObjectArg,
            int priceArg,
            float returnValueArg,
            bool isBoughtArg,
            float completionThresholdArg,
            int rawRewardArg,
            int persistenceArg)
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

    public int numbersOfLevelsInRoster = 3;
    public GameObject levelPrefab;
    List<LevelData> levels;
    public LevelsUIScript levelsUIScript;
    public accountManager accountManager;
    LevelSchedulerScript levelScheduler;
    public GlobalManagerScript globalManager;
    public AnimationCurve levelDifficultyMultiplierCurve;

    private void Awake()
    {
        levelScheduler = GetComponentInChildren<LevelSchedulerScript>();
        levelsUIScript.SetLevelBoughtEventHandler(BuyLevel);
        levels = new List<LevelData>();
    }

    public void OnStartNewRound()
    {
        RerollRoster();
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

    public void RerollRoster()
    {
        for(int i=levels.Count-1; i>=0; i--)
        {
            if (!levels[i].isBought)
            {
                levels[i].persistence--;
                if(levels[i].persistence == 0)
                {
                    levelsUIScript.DeleteButtonForLevel(levels[i].levelObject);
                    Destroy(levels[i].levelObject);
                    levels.RemoveAt(i);
                }
                else
                {
                    levelsUIScript.UpdatePersistence(levels[i].levelObject,  levels[i].persistence);
                }
            }
        }

        //TODO: split to separate functions
        for (int i = 0; i < numbersOfLevelsInRoster; i++)
        {
            GameObject newLevel = Instantiate(levelPrefab, Vector3.zero, Quaternion.identity);
            SnapshotCreatorScript snapshotCreatorScript = newLevel.GetComponent<SnapshotCreatorScript>();
            LevelScript levelScript = newLevel.GetComponent<LevelScript>();
            ProceduralMap proceduralMap = newLevel.GetComponent<ProceduralMap>();
            LevelTypeScriptableObjectScript nextLevelParams = levelScheduler.GetNextLevelParams();

            int newLevelCost = Mathf.RoundToInt(nextLevelParams.GetCost() * GetProgressionCostMultiplier()/10)*10;
            float newReturnValue = nextLevelParams.GetReturnValue();
            int rawReward = nextLevelParams.rewardValue;
            int persistence = nextLevelParams.persistenceInterval;
            float newLevelCompletionThresholdFraction = nextLevelParams.GetCompletionThresholdFraction();
            levels.Add(new LevelData(newLevel, newLevelCost, newReturnValue, false, newLevelCompletionThresholdFraction, rawReward, persistence));
            GameObject newLevelButton = levelsUIScript.SpawnShopLevelButton(newLevel, newLevelCost, newReturnValue, newLevelCompletionThresholdFraction, rawReward, persistence);
            LevelButtonScript newLevelButtonScript = newLevelButton.GetComponent<LevelButtonScript>();
            snapshotCreatorScript.finishedGeneratingSnapshotEvent += newLevelButtonScript.SetSprite;
            proceduralMap.Initialize(nextLevelParams);
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
