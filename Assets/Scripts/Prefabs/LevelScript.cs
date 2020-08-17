using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelScript : MonoBehaviour
{

    public SpriteRenderer spriteRenderer;
    public SpriteRenderer backGroundSpriteRenderer;
    public SpriteRenderer spriteRenderer;
    public ProceduralMap proceduralMap;
    public SnapshotCreatorScript snapshotCreatorScript;


    public Sprite GetLevelSprite()
    {
        return spriteRenderer.sprite;
    }

    public Sprite GetBackgroundSprite()
    {
        return Sprite.Create(backgroundTexture, new Rect(0, 0, map.GetWidth(), map.GetHeight()), new Vector2(0.5f, 0.5f));
    }

    public void SetDisplayed(bool value)
    {
        spriteRenderer.enabled = value;
    }

    private void SetupSprites()
    {
        spriteRenderer.sprite = Sprite.Create(mapTexture, new Rect(0, 0, map.GetWidth(), map.GetHeight()), new Vector2(0.5f, 0.5f));
        backGroundSpriteRenderer.sprite = Sprite.Create(backgroundTexture, new Rect(0, 0, map.GetWidth(), map.GetHeight()), new Vector2(0.5f, 0.5f));
    }
}
