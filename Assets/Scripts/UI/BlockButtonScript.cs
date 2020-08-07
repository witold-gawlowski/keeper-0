using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class BlockButtonScript : MonoBehaviour
{
    enum ButtonType {Gemshop, InventoryI, InventoryII, Shop, Deck};
    ButtonType type;
    public System.Action<Card> buttonTapEvent;
    public Card associatedCard;
    public GameObject associatedBlockPrefab;
    public TextMeshProUGUI costText;
    public Text countText;
    public GameObject blockImagePrefab;
    public GameObject imagesParent;
    public TextMeshProUGUI gemCost;
    public GameObject overlayImage;
    public List<float> blockBrightnesDecay;
    public Button _button;
    public List<float> pivotShiftTable = new List<float>() {0, 0.06f, 0.11f, 0.15f, 0.18f, 0.20f, 0.22f, 0.24f, 0.25f, 0.26f};
    public RectTransform _rectTransform;
    public Image background;

    Sprite blockSprite;

    void Awake()
    {

    }

    public void SetDisabled(bool value)
    {
        overlayImage.SetActive(value);
        _rectTransform.localScale = Vector3.one * (value ? 0.9f : 1.0f);
        _button.interactable = !value;
    }

    public void InitializeInventoryIIButton(
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

    public void InitializeInventoryIButton(Sprite spriteArg, Card cardArg)
    {
        type = ButtonType.InventoryI;
        blockSprite = spriteArg;
        UpdateCount(cardArg.quantity);
        gemCost.gameObject.SetActive(false);
        associatedCard = cardArg;
        costText.text = "$" + cardArg.cashCost;
    }

    public void InitializeDeckButton(Sprite spriteArg, Card cardArg)
    {
        type = ButtonType.Deck;
        blockSprite = spriteArg;
        UpdateCount(cardArg.quantity);
        gemCost.gameObject.SetActive(false);
        associatedCard = cardArg;
        costText.text = "$" + cardArg.cashCost;
    }

    public void InitializeGemShopButton(Sprite spriteArg, Card cardArg)
    {
        type = ButtonType.Gemshop;
        blockSprite = spriteArg;
        UpdateCount(cardArg.quantity);
        gemCost.text = "<sprite=\"Gem2\" index=0>" + cardArg.gemCost.ToString();
        gemCost.gameObject.SetActive(true);
        associatedCard = cardArg;
        costText.text = "$" + cardArg.cashCost;
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
            //newImageObject.transform.SetAsFirstSibling();
            Image blockImage = newImageObject.GetComponent<Image>();
            Outline outline = newImageObject.GetComponent<Outline>();
            RectTransform rectTransformTemp = newImageObject.GetComponent<RectTransform>();
            //float buttonDiameter = Tools.GetButtonDiameter();
            //rectTransformTemp.sizeDelta = new Vector2(buttonDiameter, buttonDiameter);
            blockImage.sprite = blockSprite;
            RectTransform newImagerectTransform = newImageObject.GetComponent<RectTransform>();
            newImagerectTransform.pivot = Vector2.one * (0.5f + pivotShiftTable[i]-pivotShiftTable[(newCount-1)/2]);
            Color baseColor = blockImage.color;
            float baseColorHue, basecolorSaturation, baseColorBrightness;
            Color.RGBToHSV(baseColor, out baseColorHue, out basecolorSaturation, out baseColorBrightness);
            blockImage.color = Color.HSVToRGB(baseColorHue, basecolorSaturation, baseColorBrightness-blockBrightnesDecay[newCount-i-1]);
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
            EventManager.SendEvent(new ShopCardTappedEvent(associatedCard));
        }
        
        if(type == ButtonType.Deck)
        {
            EventManager.SendEvent(new CardMovedToInventoryEvent(associatedCard));
        }

        if(type == ButtonType.InventoryI)
        {
            EventManager.SendEvent(new CardMovedToDeckEvent(associatedCard));
        }
    }
}
