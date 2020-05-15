using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButtonScript : MonoBehaviour
{
    private System.Action<GameObject> onTapHandler;
    public GameObject associatedLevel;
    public delegate void LevelTapDelegate(GameObject levelToRun);
    public delegate void TryBuyDelegate(int price);
    public TryBuyDelegate TryBuyEvent;
    public LevelTapDelegate LevelTapEvent;
    public GameObject padlock;
    public GameObject priceText;
    public Image levelImage;

    public void OnTap()
    {
        onTapHandler(associatedLevel);
    }

    public GameObject GetAssociatedLevel()
    {
        return associatedLevel;
    }

    public void SetSprite(Sprite spriteArg)
    {
        levelImage.sprite = spriteArg;
    }



    public void Initialize(int cost, GameObject level, System.Action<GameObject> onTapHandlerArg, bool locked)
    {
        if (locked)
        {
            padlock.SetActive(true);
            priceText.SetActive(true);
        }
        else
        {
            padlock.SetActive(false);
            priceText.SetActive(false);
        }
        associatedLevel = level;
        priceText.GetComponent<Text>().text = "$"+cost.ToString();
        onTapHandler = onTapHandlerArg;
    }
    
   

}
