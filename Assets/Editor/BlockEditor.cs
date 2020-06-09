using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
[CustomEditor(typeof(BlockScript))]
public class BlockScriptEditor : Editor
{

    public List<Vector2Int> relativeTilePositions;

    private void OnEnable()
    {
        relativeTilePositions = (target as BlockScript).relativeTilePositions;
    }

    public Vector2 GetBlockMiddlePosition()
    {
        Vector2Int firstPoint = relativeTilePositions[0];
        float minx = firstPoint.x, maxx  = firstPoint.x, miny = firstPoint.y,  maxy = firstPoint.y;
        foreach(Vector2Int v2i in relativeTilePositions)
        {
            if(v2i.x < minx)
            {
                minx = v2i.x;
            }
            if(v2i.x > maxx)
            {
                maxx = v2i.x;
            }
            if(v2i.y < miny)
            {
                miny = v2i.y;
            }
            if(v2i.y > maxy)
            {
                maxy = v2i.y;
            }
        }
        return new Vector2((minx+maxx)/2, (miny+maxy)/2);
    }

    int GetMiddleTileIndex()
    {
        int result = 0;
        float minDist = 99999;
        Vector2 middleTilePosition = GetBlockMiddlePosition();
        int n = relativeTilePositions.Count;
        for(int i=0; i<n; i++)
        {
            Vector2 currentTileMiddlePosition = relativeTilePositions[i];
            float dist = Vector2.Distance(middleTilePosition, currentTileMiddlePosition);
            if (dist < minDist)
            {
                result = i;
                minDist = dist;
            }
        }
        return result;
    }

    void DrawTiles(ref Texture2D textureArg)
    {
        int middleTileIndex = GetMiddleTileIndex();
        Vector2Int middleTilePosition = relativeTilePositions[middleTileIndex];
        int tileSize = 9;
        Vector2Int pixelShift = -Vector2Int.one * 4;
        foreach (Vector2Int v2 in relativeTilePositions)
        {
            Vector2Int tilePosition = (v2 - middleTilePosition) * tileSize + pixelShift + Vector2Int.one*50;
            DrawTile(ref textureArg, tilePosition);
        }
    }

    void DrawTile(ref Texture2D textureArg, Vector2Int positionArg)
    {
        int tileSize = 9;
        for(int i=0; i<tileSize; i++)
        {
            for (int j = 0; j<tileSize; j++)
            {
                textureArg.SetPixel(positionArg.x + i, positionArg.y + j, Color.white);
            }
        }
    }

    void DrawSeparators(ref Texture2D textureArg)
    {
        
    }

    void DrawSingleSeparator(ref Texture2D textureArg, Vector2Int middlePositionA, Vector2Int middlePositionB)
    {
        int tileSize = 9;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("Draw block icon"))
        {
            int width = 100;
            int height =100;
            Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);

            DrawTiles(ref  tex);
            tex.Apply();

            byte[] bytes = tex.EncodeToPNG();
            Object.DestroyImmediate(tex);

            File.WriteAllBytes(Application.dataPath + "/Sprites/SavedScreen.png", bytes);
        }
    }
}