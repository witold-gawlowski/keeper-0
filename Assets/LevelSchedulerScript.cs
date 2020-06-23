using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSchedulerScript : MonoBehaviour
{
    public LevelTypeScriptableObjectScript[] types;
    public List<int> levelStructure;
    public List<int> levelMapTypes;
    public List<int> levelMapNumber;
    public LevelManagerScript levelManagerScript;
    public GlobalManagerScript globalManager;
    public int minLevelGroups = 1;
    public int maxLevelGroups = 4;
    public int minLevelsPerGroup = 1;
    public int maxLevelsPerGroup = 5;
    public int minMapsPerLevel = 0;
    public int maxMapsPerLevel = 3;
    Randomizer randomizer;

    public void Init(Randomizer rArg)
    {
        randomizer = rArg;
    }

    public int GetNumberOfNewMaps(int roundArg)
    {
        return levelMapNumber[roundArg-1];
    }

    public int GetLevelCount()
    {
        return levelMapNumber.Count;
    }

    public LevelTypeScriptableObjectScript GetMapType(int roundArg)
    {
        return types[levelMapTypes[roundArg - 1]];
    }

    void CreateLevelStructure()
    {
        levelStructure = new List<int>();
        int numberOfLevelGroups = randomizer.Range(minLevelGroups, maxLevelGroups+1);
        for (int i = 0; i < numberOfLevelGroups; i++)
        {
            int groupLevelCount = randomizer.Range(minLevelsPerGroup, maxLevelsPerGroup+1);
            levelStructure.Add(groupLevelCount);
        }
    }

    public void Create()
    {
        CreateLevelStructure();
        levelMapTypes = new List<int>();
        for(int i=0; i<levelStructure.Count; i++)
        {
            int levelTypeIndex = randomizer.Range(0, types.Length);
            int groupLevelCount = levelStructure[i];
            for (int j = 0; j < groupLevelCount; j++)
            {
                levelMapTypes.Add(levelTypeIndex);
                int mapsCount = randomizer.Range(minMapsPerLevel, maxMapsPerLevel+1);
                levelMapNumber.Add(mapsCount);
            }
        }
    }


    //public int GetGroupIndex(int roundArg)
    //{
    //    int groupIndex = -1;
    //    do
    //    {
    //        roundArg -= levelStructure[groupIndex];
    //        groupIndex++;
    //    } while (roundArg > 0);
    //    return groupIndex;
    //}




    //public LevelTypeScriptableObjectScript GetNextLevelParams()
    //{
    //    LevelTypeScriptableObjectScript result = null;
    //    int currentLevel = globalManager.GetCurrentLevel();
    //    float probabilityWeightTotal = 0;
    //    List<LevelTypeScriptableObjectScript> roster = new List<LevelTypeScriptableObjectScript>();
    //    for(int i=0; i<types.Length; i++)
    //    {
    //        LevelTypeScriptableObjectScript levelType = types[i];
    //        float weightAtCurrentLevel = levelType.weightVsLevelCurve.Evaluate(currentLevel);
    //        if (true)
    //        {
    //            roster.Add(levelType);
    //            probabilityWeightTotal += weightAtCurrentLevel;
    //        }
    //    }

    //    float randomFloat = Random.Range(0, probabilityWeightTotal);
    //    float counter = 0;
    //    for(int i=0; i<roster.Count; i++)
    //    {
    //        counter += roster[i].probabilityWeight;
    //        if(counter > randomFloat)
    //        {
    //            result = roster[i];
    //            break;
    //        }
    //    }

    //    return result;
    //}

}
