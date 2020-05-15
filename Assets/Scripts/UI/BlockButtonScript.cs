using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockButtonScript : MonoBehaviour
{
    public System.Action<BlockShopScript.Item> buttonTapEvent;
    public Image blockImage;
    public BlockShopScript.Item associatedItem;
    public Text costText;
    public Text countText;
    public Image padlock;

    public void Initialize(
        Sprite spriteArg,
        int countArg,
        BlockShopScript.Item itemArg = null,
        int costArg = 0)
    {
        blockImage.sprite = spriteArg;
        UpdateCount(countArg);
        if (itemArg == null)
        {
            padlock.enabled = false;
            costText.enabled = false;
        }
        else
        {
            associatedItem = itemArg;
            costText.text = "$" + costArg.ToString();
            padlock.enabled = true;
            costText.enabled = true;
        }
    }

    public void UpdateCount(int newCount)
    {
        countText.text = newCount.ToString();
    }

    public void SetTapHandler(System.Action<BlockShopScript.Item> tapHandler)
    {
        buttonTapEvent += tapHandler;
    }

    public void OnTap()
    {
        if (associatedItem != null)
        {
            buttonTapEvent(associatedItem);
        }
    }
}
