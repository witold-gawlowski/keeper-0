using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedBlockPanelScript : MonoBehaviour
{

    public System.Action<BlockShopScript.Item> BuyButtonEvent;
    public Image blockImage;
    public Text costText;
    public Text countText;
    public BlockShopScript.Item associatedShopItem;

    
    public void Initialize(BlockShopScript.Item itemArg, Sprite spriteArg)
    {
        blockImage.sprite = spriteArg;
        costText.text = "Price: $"+itemArg.price.ToString();
        countText.text = "Count: "+itemArg.count.ToString();
        associatedShopItem = itemArg;
    }

    public void SetBuyEventHandler(System.Action<BlockShopScript.Item> handlerArg)
    {
        BuyButtonEvent = null;
        BuyButtonEvent += handlerArg;
    }

    public void OnCancelButtonTap()
    {
        gameObject.SetActive(false);
    }

    public void OnBuyButtonTap()
    {
        gameObject.SetActive(false);
        BuyButtonEvent(associatedShopItem);
    }
}
