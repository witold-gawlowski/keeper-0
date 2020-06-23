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
    public GameObject associatedPrefab;
    public GameObject deleteButton;
    public GameObject buyButton;

    
    public void InitializeShopPanel(BlockShopScript.Item itemArg, Sprite spriteArg)
    {
        blockImage.sprite = spriteArg;
        costText.text = "Price: $"+itemArg.price.ToString();
        countText.text = "Count: "+itemArg.count.ToString();
        associatedShopItem = itemArg;
        buyButton.SetActive(true);
        deleteButton.SetActive(false);
    }

    public void InitializeInventoryPanel(Sprite spriteArg, GameObject prefabObjectArg, int countArg)
    {
        countText.text = "Count: " + countArg;
        associatedPrefab = prefabObjectArg; ;
        blockImage.sprite = spriteArg;
        costText.enabled = false;
        buyButton.SetActive(false);
        deleteButton.SetActive(true);
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
