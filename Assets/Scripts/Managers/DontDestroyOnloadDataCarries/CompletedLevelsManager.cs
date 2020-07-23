﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateCompletedLevelsUIEvent: IEvent {
    public List<int> levels;
    public UpdateCompletedLevelsUIEvent(List<int> levelsArg)
    {
        levels = levelsArg;
    }
}

public class CompletedLevelsManager : MonoBehaviour
{
    List<int> levels;
    const int bigPrime = int.MaxValue;
    
    void Awake()
    {
        Load();
        if (RunResultScript.instance != null && RunResultScript.instance.completed)
        {
            RegisterLevel(RunResultScript.instance.runNumber);
        }
        EventManager.SendEvent(new UpdateCompletedLevelsUIEvent(levels));
    }

    public bool IsTutorialRangeCompleted(int tutorialRangeMaxLevel)
    {
        for(int i=1; i<=tutorialRangeMaxLevel; i++)
        {
            if (!levels.Contains(i))
            {
                return false;
            }
        }
        return true;
    }

    public int GetIncompleteTutorialLevel(int tutorialRangeMaxLevel)
    {
        for (int i = 1; i <= tutorialRangeMaxLevel; i++)
        {
            if (!levels.Contains(i))
            {
                return i;
            }
        }
        return 0;
    }


    public int GetLevelsFootprint()
    {
        int result = 1;
        foreach (int levelNumber in levels)
        {
            result *= levelNumber;
            result %= bigPrime;
        }
        return result;
    }
    public void RegisterLevel(int intArg)
    {
        if (!levels.Contains(intArg))
        {
            levels.Add(intArg);
            Save();
            EventManager.SendEvent(new UpdateCompletedLevelsUIEvent(levels));
        }
    }

    public bool IsSeedCompleted(int seedArg)
    {
        if (levels.Contains(seedArg))
        {
            return true;
        }
        return false;
    }

    public List<int> GetLevels()
    {
        return levels;
    }

    void Save()
    {
        PlayerPrefs.SetString("completedLevels", ToString());
        PlayerPrefs.Save();
    }

    void Load()
    {
        string initString = "";
        if (PlayerPrefs.HasKey("completedLevels"))
        {
            initString = PlayerPrefs.GetString("completedLevels");
        }
        Initialize(initString);
    }

    public void Initialize(string sArg)
    {
        levels = new List<int>();
        if (sArg != "")
        {
            string[] words = sArg.Split(';');
            foreach (string intString in words)
            {
                levels.Add(int.Parse(intString));
            }
        }
    }

    public override string ToString()
    {
        string result = "";
        foreach(int i in levels)
        {
            result += i + ";"; 
        }
        result = result.TrimEnd(new char[] { ';' });
        return result;
    }

}
