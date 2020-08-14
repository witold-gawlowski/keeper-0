using System.Collections;
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
    [System.Serializable]
    public class LevelThreshold
    {
        public int maxLevel;
        public int value;
    }

    List<int> _levels;

    [SerializeField]
    List<LevelThreshold> _thresholds;
    
    const int bigPrime = int.MaxValue;
    
    void Awake()
    {
        Load();
        if (RunResultScript.instance != null && RunResultScript.instance.completed)
        {
            RegisterLevel(RunResultScript.instance.runNumber);
        }
        EventManager.SendEvent(new UpdateCompletedLevelsUIEvent(_levels));
    }

    #region public
    public int Count()
    {
        return _levels.Count;
    }
   
    public int MaxLevel()
    {
        int result = _thresholds[0].maxLevel;
        for(int i=1; i<_thresholds.Count; i++)
        {
            if(_thresholds[i].value > Count())
            {
                break;
            }
            result = _thresholds[i].maxLevel;
        }
        return result;
    }

    public int NearestThreshold()
    {
        int i = 0;
        for (; i<_thresholds.Count; i++)
        {
            if(_thresholds[i].value > Count())
            {
                break;
            }
        }
        return _thresholds[i].value;
    }

    public bool IsLevelUnlocked(int number)
    {
        if (number <= MaxLevel())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Initialize(string sArg)
    {
        _levels = new List<int>();
        if (sArg != "")
        {
            string[] words = sArg.Split(';');
            foreach (string intString in words)
            {
                _levels.Add(int.Parse(intString));
            }
        }
    }

    public override string ToString()
    {
        string result = "";
        foreach (int i in _levels)
        {
            result += i + ";";
        }
        result = result.TrimEnd(new char[] { ';' });
        return result;
    }

    public bool IsTutorialRangeCompleted(int tutorialRangeMaxLevel)
    {
        for(int i=1; i<=tutorialRangeMaxLevel; i++)
        {
            if (!_levels.Contains(i))
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
            if (!_levels.Contains(i))
            {
                return i;
            }
        }
        return 0;
    }

    //public int GetLevelsFootprint()
    //{
    //    int result = 1;
    //    foreach (int levelNumber in _levels)
    //    {
    //        result *= levelNumber;
    //        result %= bigPrime;
    //    }
    //    return result;
    //}

    public void RegisterLevel(int intArg)
    {
        if (!_levels.Contains(intArg))
        {
            _levels.Add(intArg);
            Save();
            EventManager.SendEvent(new UpdateCompletedLevelsUIEvent(_levels));
        }
    }

    public bool IsSeedCompleted(int seedArg)
    {
        if (_levels.Contains(seedArg))
        {
            return true;
        }
        return false;
    }

    public List<int> GetLevels()
    {
        return _levels;
    }

    #endregion public

    #region private
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

    #endregion private

}
