using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunSpecificationFactory : MonoBehaviour
{
    public MapParams[] mapTypes;
    private RunSpecification _product;

    public RunSpecificationGenerationMethod method;

    public RunSpecification Get(int seed)
    {
        _product = ScriptableObject.CreateInstance<RunSpecification>();
        method.FillInSpecification(ref  _product, mapTypes, seed);
        return _product;
    }

}
