﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Tools
{
    [System.Serializable]
    public class Distribution
    {
        public List<Vector2Int> d;

        public int GetTotalWeight()
        {
            int result = 0;
            for (int i = 0; i < d.Count; i++)
            {
                result += d[i].y;
            }
            return result;
        }
    }

    public static float GetButtonDiameter()
    {
        return (Mathf.Min(Screen.width, Screen.height) - 50) / 4;
    }

    public static float RandomGaussian01(Randomizer randomizerArg)
    {
        float v1, v2, s;
        do
        {
            v1 = 2.0f * randomizerArg.Range(0f, 1f) - 1.0f;
            v2 = 2.0f * randomizerArg.Range(0f, 1f) - 1.0f;
            s = v1 * v1 + v2 * v2;
        } while (s >= 1.0f || s == 0f);

        s = Mathf.Sqrt((-2.0f * Mathf.Log(s)) / s);

        return v1 * s;
    }

    public static float SeededRandom(int seedArg)
    {
        Random.InitState(seedArg);
        return Random.value;
    }

    public static int RandomFromDistribution(Distribution dTemp, Randomizer rArg)
    {
        int randomPointer = rArg.Range(0, dTemp.GetTotalWeight());
        int index = -1;
        int counter = 0;
        do{
            index++;
            counter += dTemp.d[index].y;
        } while (counter <= randomPointer);
        return dTemp.d[index].x;
    }

    public static int RandomIntegerFromGaussianWithThreshold(Randomizer randomizerArg, float mean, float stdDev, int rerandomThreshold = 0)
    {
        int maxRerandomCount = 5;
        int result = 0;
        int c = -1;
        do
        {
            c++;
            float unrefinedGauss = RandomGaussian01(randomizerArg) * stdDev + mean;
            result = Mathf.RoundToInt(unrefinedGauss);
        } while (result < rerandomThreshold && c < maxRerandomCount);

        if (c == maxRerandomCount)
        {
            throw new System.Exception();
        }

        return result;
    }
}
