using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompletedLevelsManager : MonoBehaviour
{
    List<int> levels;
    const int bigPrime = int.MaxValue;

    public void RegisterLevel(int intArg)
    {
        levels.Add(intArg);
    }

    public int GetCompletedLevelsFootprint()
    {
        int result = 1;
        foreach(int levelNumber in levels)
        {
            result *= levelNumber;
            result %= bigPrime;
        }
        return result;
    }

    public List<int> GetLevels()
    {
        return levels;
    }

    public void FromString(string sArg)
    {
        string[] words = sArg.Split(';');
        foreach(string intString in words)
        {
            levels.Add(int.Parse(intString));
        }
    }

    public override string ToString()
    {
        string result = "";
        foreach(int i in levels)
        {
            result += i + ";"; 
        }
        return result;
    }
}
