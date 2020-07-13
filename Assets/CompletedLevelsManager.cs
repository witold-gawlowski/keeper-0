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
    List<int> levels;

    void Awake()
    {
        Load();
    }

    void Start()
    {
        EventManager.SendEvent(new UpdateCompletedLevelsUIEvent(levels));
    }

    public void RegisterLevel(int intArg)
    {
        levels.Add(intArg);
        Save();
        EventManager.SendEvent(new UpdateCompletedLevelsUIEvent(levels));
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
