using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButtonScript : MonoBehaviour
{
    private System.Action<GameObject> onTapHandler;
    private System.Action<GameObject> runTapHandler;
    public GameObject associatedLevel;
    public delegate void LevelTapDelegate(GameObject levelToRun);
    public delegate void TryBuyDelegate(int price);
    public TryBuyDelegate TryBuyEvent;
    public LevelTapDelegate LevelTapEvent;
    public GameObject priceText;
    public GameObject returnValueText;
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

    public void OnBuy()
    {
        onTapHandler = runTapHandler;
        priceText.SetActive(false);
    }

    public void Initialize(int cost, float returnValueArg, GameObject level, System.Action<GameObject> buyTapHandlerArg,
        System.Action<GameObject> runTapHandlerArg)
    {
        priceText.SetActive(true);
        returnValueText.GetComponent<Text>().text = "x" + returnValueArg.ToString();
        associatedLevel = level;
        priceText.GetComponent<Text>().text = "$"+cost.ToString();
        onTapHandler = buyTapHandlerArg;
        runTapHandler = runTapHandlerArg;
    }
    
   

}
