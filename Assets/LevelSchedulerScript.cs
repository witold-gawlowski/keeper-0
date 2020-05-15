using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSchedulerScript : MonoBehaviour
{
    public LevelTypeScriptableObjectScript[] types;

    public LevelManagerScript levelManagerScript;
    public GlobalManagerScript globalManager;

    public LevelTypeScriptableObjectScript GetLevel()
    {
        int currentLevel = globalManager.GetCurrentLevel();
        float probabilityWeightTotal = 0;
        List<LevelTypeScriptableObjectScript> roster = new List<LevelTypeScriptableObjectScript>();
        for(int i=0; i<types.Length; i++)
        {
            if (types[i].firstAppearanceLevel <= currentLevel)
            {
                roster.Add(types[i]);
                probabilityWeightTotal += types[i].probabilityWeight;
            }
        }


        for(int i=0; i<roster.Count; i++)
        {

        }



        return null;
    }

}
