using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSchedulerScript : MonoBehaviour
{
    public LevelTypeScriptableObjectScript[] types;

    public LevelManagerScript levelManagerScript;
    public GlobalManagerScript globalManager;

    public LevelTypeScriptableObjectScript GetNextLevelParams()
    {
        LevelTypeScriptableObjectScript result = null;
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

        float randomFloat = Random.Range(0, probabilityWeightTotal);
        float counter = 0;
        for(int i=0; i<roster.Count; i++)
        {
            counter += roster[i].probabilityWeight;
            if(counter > randomFloat)
            {
                result = roster[i];
                break;
            }
        }

        return result;
    }

}
