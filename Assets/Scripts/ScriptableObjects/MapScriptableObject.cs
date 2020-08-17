using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Map", menuName = "ScriptableObjects/Map", order = 1)]
public class MapScriptableObject : ScriptableObject
{
    public int width;
    public int height;
    public int gemID = 3;
    public int rockID = 1;
    public string mapDescription;
    public Texture2D mapImage;

    
}
