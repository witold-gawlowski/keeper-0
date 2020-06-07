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
            LevelTypeScriptableObjectScript levelType = types[i];
            float weightAtCurrentLevel = levelType.weightVsLevelCurve.Evaluate(currentLevel);
            if (true)
            {
                roster.Add(levelType);
                probabilityWeightTotal += weightAtCurrentLevel;
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
