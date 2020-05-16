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


    public static float NextGaussian()
    {
        float v1, v2, s;
        do
        {
            v1 = 2.0f * Random.Range(0f, 1f) - 1.0f;
            v2 = 2.0f * Random.Range(0f, 1f) - 1.0f;
            s = v1 * v1 + v2 * v2;
        } while (s >= 1.0f || s == 0f);

        s = Mathf.Sqrt((-2.0f * Mathf.Log(s)) / s);

        return v1 * s;
    }

    public float GetReturnValue()
    {
        float result = NextGaussian() * returnValueDeviation + returnValueMean;
        if(result < 1.05f)
        {
            result = 1.05f;
        }
        return Mathf.RoundToInt(result/0.05f)*0.05f;
    }

    public int GetCost()
    {
        float unrefinedPrice = NextGaussian() * priceStandardDeviation + priceMean;
        if(unrefinedPrice < 0)
        {
            unrefinedPrice = 0;
        }
        return Mathf.RoundToInt(unrefinedPrice/10)*10;
    }
    
}