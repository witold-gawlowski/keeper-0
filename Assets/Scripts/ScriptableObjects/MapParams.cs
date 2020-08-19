using UnityEngine;

[CreateAssetMenu(fileName = "MapParams", menuName = "ScriptableObjects/MapParams", order = 1)]
public class MapParams
{
    public string levelName = "LevelName";

    [Header("Map Generation Parameters")]
    public int width = 20;
    public int height = 30;
    public int steps = 3;
    public int deathLimit = 2;
    public int lifeLimit = 4;
    public float initialDensity = 0.5f;
    public bool removeSecondaryCaves;
    public int minimalMaxCaveSize;
    [Space(20)]

    [Header("Reward")]
    [Range(200, 1500)]
    public float rewardValueMean;
    [Range(50, 500)]
    public float rewardValueDeviation;
    [Space(20)]

    [Header("Completion")]
    [Range(0, 1)]
    public float completionsFractionMean;
    [Range(0, 0.5f)]
    public float completionFractionDispersion;
    [Space(20)]

    [Header("Rarity")]
    public float rarity = 1;

    public int GetReward(Randomizer rArg)
    {
        float result = Tools.RandomGaussian01(rArg) * rewardValueDeviation + rewardValueMean;
        if(result < 200)
        {
            result = 200;
        }
        return Mathf.RoundToInt(result);
    }

    public float GetTarget(Randomizer rArg)
    {
        float unrefinedFraction = Tools.RandomGaussian01(rArg) * completionFractionDispersion + completionsFractionMean;
        return Mathf.Clamp01(unrefinedFraction);
    }

   

}