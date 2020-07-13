using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompletedLevelsManager : MonoBehaviour
{
    List<int> levels;
 

    public void RegisterLevel(int intArg)
    {
        levels.Add(intArg);
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
