using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedScript : MonoBehaviour
{
    public int nominalSeed;
    public System.Object seed;
    public bool alreadyCompleted;
    public static SeedScript instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
