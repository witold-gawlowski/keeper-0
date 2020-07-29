using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "RunSpecification", menuName = "ScriptableObjects/RunSpecification", order = 1)]
public class RunSpecification : ScriptableObject, ILevelSpecification
{
    [System.Serializable]
    public class MapSpecification
    {
        public LevelTypeScriptableObjectScript type;
        public int seed;
    }
    [System.Serializable]
    public class LevelSpecification
    {
        public LevelTypeScriptableObjectScript type;
        public List<MapSpecification> specification;
    }
    public string id;
    public List<LevelSpecification> levelStructure;
    public int GetMapsOnLevel(int level)
    {
        return levelStructure[level].specification.Count;
    }
    public int GetTotalLevels()
    {
        return levelStructure.Count;
    }
}
