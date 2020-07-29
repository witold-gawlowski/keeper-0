using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILevelSpecification
{
    int GetTotalLevels();
    int GetNumberOfNewMaps(int levelArg);
    LevelTypeScriptableObjectScript GetMapType(int levelArg);
}
