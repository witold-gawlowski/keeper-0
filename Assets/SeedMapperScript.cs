using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedMapperScript : MonoBehaviour
{
    [System.Serializable]
    public class IndexedSpecification
    {
        public int index;
        public RunSpecification spec;
    }

    public class IndexedNumericalSeed
    {
        public int index;
        public int seed;
    }

    public List<IndexedSpecification> indexedSpecifications;
    public List<IndexedNumericalSeed> indexedNumericalSeeds;

    public System.Object GetSeed(int indexArg)
    {
        foreach (IndexedSpecification spec in indexedSpecifications)
        {
            if (spec.index == indexArg)
            {
                return spec.spec;
            }
        }
        int? result = indexArg;
        foreach (IndexedNumericalSeed seed in indexedNumericalSeeds)
        {
            if (seed.index == indexArg)
            {
                result = seed.seed;
                return result;
            }
        }
        return result;
    }

}

