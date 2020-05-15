using UnityEngine;

[CreateAssetMenu(fileName = "LevelType", menuName = "ScriptableObjects/LevelType", order = 1)]
public class LevelTypeScriptableObjectScript: ScriptableObject
{
    public string levelName = "LevelName";

    [Header("Map Generation Parameters")]
    public int width = 20;
    public int height = 30;
    public int steps = 3;
    public int deathLimit = 2;
    public int lifeLimit = 4;
    public float initialDensity = 0.5f;
    [Space(20)]

    [Header("Occurence Parameters")]
    public float probabilityWeight = 1;
    public int firstAppearanceLevel = 1;
    [Space(20)]

    [Header("Others")]
    public int price;
    
}