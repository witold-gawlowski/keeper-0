using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Tools
{
    public static float RandomGaussian01()
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

    public static int RefinedGauss(float mean, float stdDev, int rerandomThreshold = 0)
    {
        int maxRerandomCount = 5;
        int result = 0;
        int c = -1;
        do
        {
            c++;
            float unrefinedGauss = RandomGaussian01() * stdDev + mean;
            result = Mathf.RoundToInt(unrefinedGauss);
        } while (result < rerandomThreshold && c < maxRerandomCount);

        if (c == maxRerandomCount)
        {
            throw new System.Exception();
        }

        return result;
    }
}
