using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
[CustomEditor(typeof(BlockScript))]
public class BlockScriptEditor : Editor
{
    int tileSize = 9;
    int imageSize = 100;
    Color separatorColor = new Color(0.9f, 0.9f, 0.9f, 1);
    Vector2Int pixelShift = -Vector2Int.one * 4;
    public List<Vector2Int> relativeTilePositions;
    Vector2Int middleTilePosition;

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

    Vector2Int GetTilePositionInPixels(Vector2Int tileRelativePosition)
    {
        return (tileRelativePosition - middleTilePosition) * tileSize + pixelShift + Vector2Int.one * imageSize/2;
    }
   

    void DrawTiles(ref Texture2D textureArg)
    {
        foreach (Vector2Int v2 in relativeTilePositions)
        {
            Vector2Int tilePosition = GetTilePositionInPixels(v2);
            DrawTile(ref textureArg, tilePosition);
        }
    }

    void DrawTile(ref Texture2D textureArg, Vector2Int positionArg)
    {
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
        int n = relativeTilePositions.Count;
        for(int i=0; i<n-1; i++)
        {
            for (int j = i+1; j < n; j++)
            {
                Vector2Int relPosI = relativeTilePositions[i];
                Vector2Int relPosJ = relativeTilePositions[j];

                if(Mathf.Abs(relPosI.x -  relPosJ.x)+Mathf.Abs(relPosI.y - relPosJ.y)==1)
                {
                    DrawSingleSeparator(ref textureArg, relPosJ, relPosI);
                }
            }
        }
    }


    void DrawSingleSeparator(ref Texture2D textureArg, Vector2Int tilePositionA, Vector2Int tilePositionB)
    {
      
        Vector2Int leftBottomCornerPositionAPixelCoords = GetTilePositionInPixels(tilePositionA);
        Vector2Int leftBottomCornerPositionBPixelCoords = GetTilePositionInPixels(tilePositionB);

        //swap A and B so that A  has always greater coords than B
        if (leftBottomCornerPositionAPixelCoords.x < leftBottomCornerPositionBPixelCoords.x ||
            leftBottomCornerPositionAPixelCoords.y < leftBottomCornerPositionBPixelCoords.y)
        {
            Vector2Int temp = leftBottomCornerPositionBPixelCoords;
            leftBottomCornerPositionBPixelCoords = leftBottomCornerPositionAPixelCoords;
            leftBottomCornerPositionAPixelCoords = temp;
        }

        Debug.Log(leftBottomCornerPositionAPixelCoords);
        Vector2Int AtoBUnitVector = (leftBottomCornerPositionBPixelCoords - leftBottomCornerPositionAPixelCoords) / tileSize;
        Vector2Int AtoBMirroredUnitVector = new Vector2Int(AtoBUnitVector.y, AtoBUnitVector.x);

        for (int i = 0; i < 2; i++)
        {
            for(int j=0;  j<tileSize; j++)
            {
                Vector2Int temp = AtoBMirroredUnitVector * j - AtoBUnitVector * i;

                textureArg.SetPixel(-temp.x + leftBottomCornerPositionAPixelCoords.x, -temp.y + leftBottomCornerPositionAPixelCoords.y, separatorColor); 
            }
        }
    }

    void ClearTexture(ref Texture2D texArg)
    {
        for (int i = 0; i < imageSize; i++)
        {
            for(int j=0; j<imageSize; j++)
            {
                texArg.SetPixel(i, j, Color.clear);
            }
        }
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("Draw block icon"))
        {
            Texture2D tex = new Texture2D(imageSize, imageSize, TextureFormat.ARGB32, false);
            ClearTexture(ref tex);
            int middleTileIndex = GetMiddleTileIndex();
            middleTilePosition = relativeTilePositions[middleTileIndex];
            DrawTiles(ref  tex);
            DrawSeparators(ref tex);

            tex.Apply();

            byte[] bytes = tex.EncodeToPNG();
            Object.DestroyImmediate(tex);

            string blockName = (target as BlockScript).name;
            File.WriteAllBytes("D:/Documents/Keeper-0/Assets/Resources/Blocks/"+ blockName+".png", bytes);


        }
    }
}