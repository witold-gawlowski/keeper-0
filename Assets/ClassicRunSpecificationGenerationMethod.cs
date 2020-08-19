using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Classic", menuName = "ScriptableObjects/Run Generation Methods/Classic", order = 1)]
public class ClassicRunSpecificationGenerationMethod : RunSpecificationGenerationMethod
{
    public int minLevelGroups = 1;
    public int maxLevelGroups = 4;
    public int minLevelsPerGroup = 1;
    public int maxLevelsPerGroup = 5;
    public int minMapsPerLevel = 0;
    public int maxMapsPerLevel = 3;

    public override void FillInSpecification(RunSpecification specification, MapParams[] mapTypes)
    {
        
    }
}
