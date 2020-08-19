using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "RunSpecification", menuName = "ScriptableObjects/RunSpecification", order = 1)]
public class RunSpecification : ScriptableObject, ILevelSpecification
{
    [System.Serializable]
    public class MapSpecification
    {
        public MapParams type;
        public float target;
        public int reward;
        public int seed;
    }
    [System.Serializable]
    public class LevelSpecification
    {
        public MapParams type;
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

    public MapParams GetMapType(int levelArg)
    {
        return levelStructure[levelArg-1].type;
    }

    public int GetReward(int levelArg, int mapNumberArg)
    {
        return levelStructure[levelArg - 1].specification[mapNumberArg].reward;
    }

    public float GetTarget(int levelArg, int mapNumberArg)
    {
        return levelStructure[levelArg - 1].specification[mapNumberArg].reward;
    }

    public Randomizer GetRandomizer(int levelArg, int mapNumberArg)
    {
        return new Randomizer(levelStructure[levelArg - 1].specification[mapNumberArg].seed);
    }
}
