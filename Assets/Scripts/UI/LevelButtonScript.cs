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
    public GameObject completionThresholdText;
    public GameObject returnValueText;
    public Image levelImage;
    public GameObject rawRewardText;
    public GameObject persistence;
    public RectTransform parentMask;
    public RectTransform levelImageRectTransform;

    public void OnTap()
    {
        onTapHandler(associatedLevel);
    }

    public GameObject GetAssociatedLevel()
    {
        return associatedLevel;
    }

    private void Start()
    {
        StartCoroutine(SetMapImageSize());
    }

    public IEnumerator SetMapImageSize()
    {
        yield return new WaitForEndOfFrame();
        int width = levelImage.sprite.texture.width;
        int height = levelImage.sprite.texture.height;
        float parentWidth = parentMask.rect.width;
        float parentHeight = parentMask.rect.height;
        if (width < height)
        {
            print(" w < h ");
            levelImageRectTransform.sizeDelta = new Vector2(parentWidth, height * parentWidth / width);
        }
        else
        {
            levelImageRectTransform.sizeDelta = new Vector2(parentHeight / height * width, parentHeight);
        }
    }

    public void SetSprite(Sprite spriteArg)
    {

        levelImage.sprite = spriteArg;
    }

    public void UpdatePersistence(int valueArg)
    {
        persistence.GetComponent<Text>().text = "T" + valueArg.ToString(); 
    }

    public void UpdateRawReward(int valueArg)
    {
        rawRewardText.GetComponent<Text>().text = "$" + valueArg.ToString();
    }

    public void OnBuy()
    {
        onTapHandler = runTapHandler;
        priceText.SetActive(false);
    }

    public void Initialize(int cost,
        float returnValueArg,
        float completionThresholdArg,
        GameObject level,
        System.Action<GameObject> buyTapHandlerArg,
        System.Action<GameObject> runTapHandlerArg, 
        int rawRewardArg,
        int persistenceArg)
    {
        priceText.SetActive(true);
        returnValueText.GetComponent<Text>().text = "x" + returnValueArg.ToString();
        completionThresholdText.GetComponent<Text>().text = completionThresholdArg.ToString("0.00");
        associatedLevel = level;
        priceText.GetComponent<Text>().text = "$"+cost.ToString();
        onTapHandler = buyTapHandlerArg;
        runTapHandler = runTapHandlerArg;
        rawRewardText.GetComponent<Text>().text = "$" + rawRewardArg.ToString();
        persistence.GetComponent<Text>().text = "T" + persistenceArg.ToString();
    }
    
   

}
