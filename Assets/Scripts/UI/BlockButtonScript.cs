﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BlockButtonScript : MonoBehaviour
{
    enum ButtonType {Gemshop, InventoryI, InventoryII, Shop, Deck };
    ButtonType type;
    public System.Action<Card> buttonTapEvent;
    public Card associatedCard;
    public GameObject associatedBlockPrefab;
    public Text costText;
    public Text countText;
    public GameObject blockImagePrefab;
    public GameObject imagesParent;
    public TextMeshProUGUI gemCost;
    RectTransform rectTransform;

    Sprite blockSprite;

    public void InitializeInventoryButton(
        Sprite spriteArg,
        GameObject gameObjectArg)
    {
        type = ButtonType.InventoryII;
        blockSprite = spriteArg;
        costText.enabled = false;
        gameObject.SetActive(false);
        associatedBlockPrefab = gameObjectArg;
        associatedCard = null;
    }

    public void InitializeGemShopButton(Sprite spriteArg, Card cardArg)
    {
        type = ButtonType.Gemshop;
        blockSprite = spriteArg;
        UpdateCount(cardArg.quantity);
        gemCost.text = "<sprite=\"Gem2\" index=0>" + cardArg.gemCost.ToString();
        gemCost.gameObject.SetActive(true);
        associatedCard = cardArg;
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
        type = ButtonType.Shop;
        blockSprite = spriteArg;
        UpdateCount(itemArg.quantity);
        associatedCard = itemArg;
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
            float buttonDiameter = Tools.GetButtonDiameter();
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
        if (type == ButtonType.Shop)
        {
            buttonTapEvent(associatedCard);
        }

        if(type == ButtonType.InventoryII)
        {
            EventManager.RaiseOnInventoryBlockTap(associatedBlockPrefab);
        }

        if(type == ButtonType.Gemshop)
        {
            EventManager.SendEvent(new CardSoldEvent(associatedCard));
        }
    }
}
