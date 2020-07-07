using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SnapshotCreatorScript : MonoBehaviour
{

    public System.Action<Sprite> finishedGeneratingSnapshotEvent;
    public SpriteRenderer spriteRenderer;
    public Texture2D mapTexture;
    public Texture2D backgroundTexture;
    public ProceduralMap map;

    private void Awake()
    {
        map.finishedGeneratingMapEvent += OnFishedGeneratingMap;
    }

    public Sprite GetLevelSprite()
    {
        return spriteRenderer.sprite;
    }

    public Sprite GetBackgroundSprite()
    {
        return Sprite.Create(backgroundTexture, new Rect(0, 0, map.GetWidth(), map.GetHeight()), new Vector2(0.5f, 0.5f));
    }

    private void OnFishedGeneratingMap()
    {
        CreateTexture();
        spriteRenderer.sprite = Sprite.Create(mapTexture, new Rect(0, 0, map.GetWidth(), map.GetHeight()), new Vector2(0.5f, 0.5f));
        if (finishedGeneratingSnapshotEvent != null)
        {
            finishedGeneratingSnapshotEvent(spriteRenderer.sprite);
        }
    }

    public void CreateTexture()
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
