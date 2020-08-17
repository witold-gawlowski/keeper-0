using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SnapshotCreatorScript : MonoBehaviour
{
    public Texture2D mapTexture;
    public Texture2D backgroundTexture;
    public ProceduralMap map;

    public void CreateTextures()
    { 
        int w = map.GetWidth();
        int h = map.GetHeight();
        mapTexture = new Texture2D(w, h);
        mapTexture.filterMode = FilterMode.Point;
        backgroundTexture = new Texture2D(w, h);
        backgroundTexture.filterMode = FilterMode.Point;
        Color color;
        for (int x=0; x<w; x++)
        {
            for(int y=0; y<h; y++)
            {
                if (map.map[x, y] == 0)
                {
                    color = Color.clear;
                }
                else if(map.map[x,y] == 4)
                {
                    color = Color.green;
                }
                else
                {
                    color = Color.black;
                }
                mapTexture.SetPixel(x, y, color);
                backgroundTexture.SetPixel(x, y, Color.white);
            }
        }
        mapTexture.Apply();
        backgroundTexture.Apply();
    }
}
