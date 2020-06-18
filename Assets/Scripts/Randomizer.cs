using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Randomizer
{
    Random.State stateSave;

    public Randomizer(int seed)
    {
        Random.InitState(seed);
        stateSave = Random.state;
    }

    public int Range(int  min, int max)
    {
        Random.state = stateSave;
        int result = Random.Range(min, max);
        stateSave = Random.state;
        return result;
    }

    public float Range(float min, float max)
    {
        Random.state = stateSave;
        float result = Random.Range(min, max);
        stateSave = Random.state;
        return result;
    }

}
