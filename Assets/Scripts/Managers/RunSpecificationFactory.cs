using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunSpecificationFactory : MonoBehaviour
{
    public MapParams[] mapTypes;
    private Randomizer _r;
    private RunSpecification _product;

    public RunSpecificationGenerationMethod method;

    public RunSpecification Get(int seed)
    {
        _r = new Randomizer(seed);
        _product = ScriptableObject.CreateInstance<RunSpecification>();
        method.FillInSpecification(_product, mapTypes);
        return _product;
    }

}
