using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SnapshotCreatorScript : MonoBehaviour
{

    public System.Action<Sprite> finishedGeneratingSnapshotEvent;
    public SpriteRenderer spriteRenderer;
    public Texture2D mapTexture;
    public ProceduralMap map;

    private void Awake()
    {
        map.finishedGeneratingMapEvent += OnFishedGeneratingMap;
    }

    public Sprite GetLevelSprite()
    {
        return spriteRenderer.sprite;
    }

    private void OnFishedGeneratingMap()
    {
        CreateTexture();
        spriteRenderer.sprite = Sprite.Create(mapTexture, new Rect(0, 0, map.GetWidth(), map.GetHeight()), new Vector2(0.5f, 0.5f));
        finishedGeneratingSnapshotEvent(spriteRenderer.sprite);
    }

    public void CreateTexture()
    { 
        int w = map.GetWidth();
        int h = map.GetHeight();
        mapTexture = new Texture2D(w, h);
        mapTexture.filterMode = FilterMode.Point;
        Color color = Color.white;
        for (int x=0; x<w; x++)
        {
            for(int y=0; y<h; y++)
            {
                if (map.map[x, y] == 0)
                {
                    color.a = 0;
                }
                else
                {
                    color.a = 1;
                }
                mapTexture.SetPixel(x, y, color);
            }
        }
        mapTexture.Apply();
    }
}
