using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChompleteLevelCheatScript : MonoBehaviour
{
    public LevelMoneyManagerScript levelMoneyManagerScript;
    void OnGUI()
    {
        //Delete all of the PlayerPrefs settings by pressing this Button
        if (GUI.Button(new Rect(100, 200, 200, 60), "Complete Level"))
        {
            levelMoneyManagerScript.BlockPlacedEventHandler(1000, 1000);
        }
    }
}
