using UnityEngine;

[CreateAssetMenu(fileName = "LevelType", menuName = "ScriptableObjects/LevelType", order = 1)]
public class LevelTypeScriptableObjectScript: ScriptableObject
{
    public string levelName = "LevelName";

    [Header("Map Generation Parameters")]
    public int width = 20;
    public int height = 30;
    public int steps = 3;
    public int deathLimit = 2;
    public int lifeLimit = 4;
    public float initialDensity = 0.5f;
    [Space(20)]

    [Header("Occurence Parameters")]
    public float probabilityWeight = 1;
    public int firstAppearanceLevel = 1;
    public AnimationCurve weightVsLevelCurve;
    [Space(20)]

    [Header("Price")]
    [Range(100, 1000)]
    public int priceMean;
    [Range(30, 1000)]
    public int priceStandardDeviation;
    [Space(20)]

    [Header("Return")]
    [Range(1, 2)]
    public float returnValueMean;
    [Range(0.05f, 0.4f)]
    public float returnValueDeviation;
    [Range(200, 5000)]
    public int rewardValue;
    [Space(20)]

    [Header("Completion")]
    [Range(0, 1)]
    public float completionsFractionMean;
    [Range(0, 0.5f)]
    public float completionFractionDispersion;
    [Space(20)]

    [Header("Persistence")]
    [Range(1, 15)]
    public int persistenceInterval;
    [Space(20)]

    [Header("Special  Options")]
    public int minimalMaxCaveSize;
    public bool removeSecondaryCaves;


   

    public float GetReturnValue(Randomizer rArg)
    {
        float result = Tools.RandomGaussian01(rArg) * returnValueDeviation + returnValueMean;
        if(result < 1.05f)
        {
            result = 1.05f;
        }
        return Mathf.RoundToInt(result/0.05f)*0.05f;
    }

    public int GetCost(Randomizer rArg)
    {
        float unrefinedPrice = Tools.RandomGaussian01(rArg) * priceStandardDeviation + priceMean;
        if(unrefinedPrice < 0)
        {
            unrefinedPrice = 0;
        }
        return Mathf.RoundToInt(unrefinedPrice/10)*10;
    }

    public float GetCompletionThresholdFraction(Randomizer rArg)
    {
        float unrefinedFraction = Tools.RandomGaussian01(rArg) * completionFractionDispersion + completionsFractionMean;
        return Mathf.Clamp01(unrefinedFraction);
    }

   

}