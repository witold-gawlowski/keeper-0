using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockButtonScript : MonoBehaviour
{
    public System.Action<Card> buttonTapEvent;
    public Card associatedItem;
    public GameObject associatedBlockPrefab;
    public Text costText;
    public Text countText;
    public GameObject blockImagePrefab;
    public GameObject imagesParent;
    RectTransform rectTransform;

    Sprite blockSprite;

    public void InitializeInventoryButton(
        Sprite spriteArg,
        GameObject gameObjectArg)
    {
        blockSprite = spriteArg;
        costText.enabled = false;
        gameObject.SetActive(false);
        associatedBlockPrefab = gameObjectArg;
        associatedItem = null;
    }

    public void InitializeGemShopButton(Sprite spriteArg, Card cardArg)
    {

    }

    public void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void InitializeShopButton(
        Sprite spriteArg,
        Card itemArg,
        System.Action<Card> tapHandler)
    {
        blockSprite = spriteArg;
        UpdateCount(itemArg.quantity);
        associatedItem = itemArg;
        associatedBlockPrefab = null;
        costText.text = "$" + itemArg.cashCost;
        costText.enabled = true;
        buttonTapEvent += tapHandler;
    }

    public void UpdateCount(int newCount)
    {
        foreach(Transform childTransform in imagesParent.transform)
        {
            Destroy(childTransform.gameObject);
        }

        for(int i=0; i<newCount; i++)
        {
            GameObject newImageObject = Instantiate(blockImagePrefab, imagesParent.transform);
            newImageObject.transform.SetAsFirstSibling();
            Image blockImage = newImageObject.GetComponent<Image>();
            RectTransform rectTransformTemp = newImageObject.GetComponent<RectTransform>();
            float buttonDiameter = GlobalUIScript.instance.GetButtonDiameter();
            //rectTransformTemp.sizeDelta = new Vector2(buttonDiameter, buttonDiameter);
            blockImage.sprite = blockSprite;
            RectTransform newImagerectTransform = newImageObject.GetComponent<RectTransform>();
            newImagerectTransform.pivot = Vector2.one * (0.5f + 0.1f * (i-newCount/2));
            Color baseColor = blockImage.color;
            float baseColorHue, basecolorSaturation, baseColorBrightness;
            Color.RGBToHSV(baseColor, out baseColorHue, out basecolorSaturation, out baseColorBrightness);
            blockImage.color = Color.HSVToRGB(baseColorHue, basecolorSaturation, baseColorBrightness/(i+1));
        }
    }


    public void OnTap()
    {
        if (associatedItem != null)
        {
            buttonTapEvent(associatedItem);
        }

        if(associatedBlockPrefab != null)
        {
            EventManager.RaiseOnInventoryBlockTap(associatedBlockPrefab);
        }
    }
}
