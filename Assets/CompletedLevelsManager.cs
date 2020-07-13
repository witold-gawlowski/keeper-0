using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompletedLevelsManager : MonoBehaviour
{
    List<int> levels;

    private void Awake()
    {
        string initString = "";
        if (PlayerPrefs.HasKey("completedLevels"))
        {
            initString = PlayerPrefs.GetString("completedLevels");
        }
        Initialize(initString);
    }

    private void Start()
    {
        
    }

    public void RegisterLevel(int intArg)
    {
        levels.Add(intArg);
    }

    public List<int> GetLevels()
    {
        return levels;
    }

    public void Initialize(string sArg)
    {
        levels = new List<int>();
        if (sArg != "")
        {
            string[] words = sArg.Split(';');
            foreach (string intString in words)
            {
                print("intString " + intString);
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
        return result;
    }

}
