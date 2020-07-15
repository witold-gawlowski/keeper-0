// Simple script that lets you create a new
// Scene, create a cube and an empty game object in the Scene
// Save the Scene and close the editor

using UnityEditor;
using System.IO;
using UnityEngine;

#if UNITY_EDITOR


public class CardCreator
{
    [MenuItem("My Scripts/Create Cards")]
    static void EditorPlaying()
    {
        string path = UnityEngine.Application.dataPath + "/Prefabs/Blocks";
        DirectoryInfo dir = new DirectoryInfo(path);
        FileInfo[] info = dir.GetFiles("*.prefab");
        foreach (FileInfo f in info)
        {
            Debug.Log(f.Name);
            string s = "Assets/Prefabs/Blocks/"+ f.Name;


            GameObject pgo = AssetDatabase.LoadAssetAtPath(s, typeof(GameObject)) as GameObject;
            

            Card asset = UnityEngine.ScriptableObject.CreateInstance<Card>();
            asset.block = pgo;
            asset.cashCost = 200;
            asset.gemCost = 2;
            asset.quantity = 3;
            AssetDatabase.CreateAsset(asset, "Assets/Cards/" + f.Name.Replace(".prefab", "") + ".asset");
            AssetDatabase.SaveAssets();
        }
   
    }
}
    #endif