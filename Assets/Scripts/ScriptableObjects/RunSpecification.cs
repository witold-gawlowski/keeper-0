using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "RunSpecification", menuName = "ScriptableObjects/RunSpecification", order = 1)]
public class RunSpecification : ScriptableObject
{
    [System.Serializable]
    public class MapSpecification
    {
        public float target;
        public int reward;
        public Map mapGeography;
    }

    [System.Serializable]
    public class LevelSpecification
    {
        public List<MapSpecification> specification;
    }
    public string id;
    public List<LevelSpecification> levelStructure;

    public int GetTotalLevels()
    {
        return levelStructure.Count;
    }

    public int GetNumberOfNewMaps(int levelArg)
    {
        return levelStructure[levelArg-1].specification.Count;
    }

    public int GetReward(int levelArg, int mapNumberArg)
    {
        return levelStructure[levelArg - 1].specification[mapNumberArg].reward;
    }

    public float GetTarget(int levelArg, int mapNumberArg)
    {
        return levelStructure[levelArg - 1].specification[mapNumberArg].reward;
    }

}
