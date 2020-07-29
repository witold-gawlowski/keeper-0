using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILevelSpecification
{
    int GetTotalLevels();
    int GetNumberOfNewMaps(int levelArg);
    LevelTypeScriptableObjectScript GetMapType(int levelArg);
    int GetReward(int levelArg, int mapNumberArg);
    float GetTarget(int levelArg, int mapNumberArg);
    Randomizer GetRandomizer(int levelArg, int mapNumberArg);
}
