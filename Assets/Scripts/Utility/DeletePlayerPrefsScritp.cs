using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeletePlayerPrefsScritp : MonoBehaviour
{
    
    void OnGUI()
    {
        //Delete all of the PlayerPrefs settings by pressing this Button
        if (GUI.Button(new Rect(100, 200, 200, 60), "Delete"))
        {
            PlayerPrefs.DeleteAll();
        }
    }
    
}
