using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelScript : MonoBehaviour
{

    public SpriteRenderer backGroundSpriteRenderer;
    public SpriteRenderer spriteRenderer;
    public Map proceduralMap;
    public GameObject levelSpriteObject;
    public GameObject levelBackgroundSpriteObject;
    public void Initialize()
    {
        levelSpriteObject.transform.localPosition = proceduralMap.GetCenterPosition();
        levelBackgroundSpriteObject.transform.localPosition = proceduralMap.GetCenterPosition();
    }
    public Sprite GetLevelSprite()
    {
        return spriteRenderer.sprite;
    }
    //public Sprite GetBackgroundSprite()
    //{
    //    return Sprite.Create(backgroundTexture, new Rect(0, 0, proceduralMap.GetWidth(), proceduralMap.GetHeight()), new Vector2(0.5f, 0.5f));
    //}
    public void SetDisplayed(bool value)
    {
        spriteRenderer.enabled = value;
    }
    //private void SetupSprites()
    //{
    //    spriteRenderer.sprite = Sprite.Create(
    //        mapTexture, 
    //        new Rect(0, 0, proceduralMap.GetWidth(), proceduralMap.GetHeight()), 
    //        new Vector2(0.5f, 0.5f)
    //        );
    //    backGroundSpriteRenderer.sprite = Sprite.Create(
    //        backgroundTexture, 
    //        new Rect(0, 0, proceduralMap.GetWidth(), proceduralMap.GetHeight()),
    //        new Vector2(0.5f, 0.5f)
    //        );
    //}
}
