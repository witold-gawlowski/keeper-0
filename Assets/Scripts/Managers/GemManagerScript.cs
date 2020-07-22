using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemManagerScript : MonoBehaviour
{
    int totalGems;
    public void AddGem(int countArg)
    {
        totalGems += countArg;
    }
    public int GetGems()
    {
        return totalGems;
    }
}
