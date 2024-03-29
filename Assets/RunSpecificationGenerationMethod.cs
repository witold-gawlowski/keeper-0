﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RunSpecificationGenerationMethod : ScriptableObject
{
    public abstract void FillInSpecification(ref RunSpecification specification, MapParams[] mapTypes, int seed);
}
