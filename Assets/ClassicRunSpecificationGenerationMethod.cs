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
    public float base_gem_density;

    public override void FillInSpecification(ref RunSpecification specification, MapParams[] mapTypes, int seed)
    {
        Random.InitState(seed);
        int type_count = mapTypes.Length;
        specification.levelStructure = new List<RunSpecification.LevelSpecification>();
        int level_groups = Random.Range(minLevelGroups, maxLevelGroups);
        int level_number = 1;
        for(int i=0; i<level_groups; i++)
        {
            int number_of_levels = Random.Range(minLevelsPerGroup, maxLevelsPerGroup);
            int random_type_index = Random.Range(0, type_count);
            MapParams group_type = mapTypes[random_type_index];
            for(int j=0; j<number_of_levels; j++)
            {
                var levelSpecification = new RunSpecification.LevelSpecification();
                int number_of_maps = Random.Range(minMapsPerLevel, maxMapsPerLevel);
                int current_level_number = level_number++;
                for (int k = 0; k<number_of_maps; k++)
                {
                    var map = new RunSpecification.MapSpecification();
                    ProceduralMap map_geography = new ProceduralMap();
                    float gem_density = current_level_number * base_gem_density;
                    map_geography.Init(group_type, gem_density);
                    map_geography.Generate();
                    map.mapGeography = map_geography;
                    int reward = Tools.RandomIntegerFromGaussianWithThreshold(
                        group_type.rewardValueMean,
                        group_type.rewardValueDeviation);
                    map.reward = reward;
                    float target = Tools.RandomGaussian01() * group_type.completionsFractionMean + group_type.completionsFractionMean;
                    map.target = target;
                    levelSpecification.specification.Add(map);
                }
                specification.levelStructure.Add(levelSpecification);
            }
        }
    }
}
