using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Map))]
public class MapEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        //GUI.DrawTexture();
        GUILayout.Label("Editing the map via text format.");
        string mapString = EditorGUILayout.TextField("Enter", Selection.activeGameObject.name);
        GUILayout.Button("Load map from text.");
        GUILayout.Button("Generate map string encoding.");
        EditorGUILayout.TextArea("MapString encoding here.");
    }
}
