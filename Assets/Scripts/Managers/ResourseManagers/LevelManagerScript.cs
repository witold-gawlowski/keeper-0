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
        public LevelData(GameObject levelObjectArg, int priceArg, float returnValueArg, bool isBoughtArg)
        {
            levelObject = levelObjectArg;
            price = priceArg;
            isBought = isBoughtArg;
            returnValue = returnValueArg;
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
                levelsUIScript.DeleteButtonForLevel(levels[i].levelObject);
                Destroy(levels[i].levelObject);
                levels.RemoveAt(i);
            }
        }
        
        for (int i = 0; i < numbersOfLevelsInRoster; i++)
        {
            GameObject newLevel = Instantiate(levelPrefab, Vector3.zero, Quaternion.identity);
            SnapshotCreatorScript snapshotCreatorScript = newLevel.GetComponent<SnapshotCreatorScript>();
            LevelScript levelScript = newLevel.GetComponent<LevelScript>();
            ProceduralMap proceduralMap = newLevel.GetComponent<ProceduralMap>();
            LevelTypeScriptableObjectScript nextLevelParams = levelScheduler.GetNextLevelParams();
            int newLevelCost = Mathf.RoundToInt(nextLevelParams.GetCost() * GetProgressionCostMultiplier()/10)*10;
            float newReturnValue = nextLevelParams.GetReturnValue();
            levels.Add(new LevelData(newLevel, newLevelCost, newReturnValue, false));
            GameObject newLevelButton = levelsUIScript.SpawnLevelButton(newLevel, newLevelCost, newReturnValue);
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
