using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedBlockPanelScript : MonoBehaviour
{
   public class DeleteButtonEvent : IEvent {
        public GameObject blockPrefab;
        public DeleteButtonEvent(GameObject blockPrefabArg)
        {
            blockPrefab = blockPrefabArg;
        }
    }

    public System.Action<Card> BuyButtonEvent;
    public Image blockImage;
    public Text costText;
    public Text countText;
    public Card associatedShopItem;
    public GameObject associatedPrefab;
    public GameObject deleteButton;
    public GameObject buyButton;

    private void Awake()
    {
        EventManager.onBlockDeleted += UpdateCount; 
    }

    public void InitializeShopPanel(Card itemArg, Sprite spriteArg)
    {
        blockImage.sprite = spriteArg;
        costText.text = "Price: $"+itemArg.cashCost.ToString();
        countText.text = "Count: "+itemArg.quantity.ToString();
        associatedShopItem = itemArg;
        buyButton.SetActive(true);
        deleteButton.SetActive(false);
    }

    public void InitializeInventoryPanel(Sprite spriteArg, GameObject prefabObjectArg, int countArg)
    {
        UpdateCount(null,countArg);
        associatedPrefab = prefabObjectArg; ;
        blockImage.sprite = spriteArg;
        costText.enabled = false;
        buyButton.SetActive(false);
        deleteButton.SetActive(true);
    }

    public void UpdateCount(GameObject dummy, int countArg)
    {
        countText.text = "Count: " + countArg;
    }

    public void SetBuyEventHandler(System.Action<Card> handlerArg)
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

    public void OnDeleteButtonTap()
    {
        EventManager.SendEvent(new DeleteButtonEvent(associatedPrefab));
    }
}
