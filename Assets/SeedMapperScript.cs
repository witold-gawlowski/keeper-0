using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedMapperScript : MonoBehaviour
{
    public List<int> map;
    public int Map(int originalSeed)
    {
        if (originalSeed > map.Count)
        {
            return originalSeed;
        }
        else
        {
            return map[originalSeed];
        }
    }
}
