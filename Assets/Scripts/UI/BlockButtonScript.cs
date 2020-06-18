using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockButtonScript : MonoBehaviour
{
    public System.Action<BlockShopScript.Item> buttonTapEvent;
    public BlockShopScript.Item associatedItem;
    public Text costText;
    public Text countText;
    public GameObject blockImagePrefab;
    public GameObject imagesParent;

    Sprite blockSprite;

    public void Initialize(
        Sprite spriteArg,
        int countArg,
        BlockShopScript.Item itemArg = null,
        int costArg = 0)
    {
        blockSprite = spriteArg;
        UpdateCount(countArg);
        if (itemArg == null)
        {
            costText.enabled = false;
        }
        else
        {
            associatedItem = itemArg;
            costText.text = "$" + costArg.ToString();
            costText.enabled = true;
        }
        if (countArg == 0)
        {
            gameObject.SetActive(false);
        }
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
            blockImage.sprite = blockSprite;
            RectTransform newImagerectTransform = newImageObject.GetComponent<RectTransform>();
            newImagerectTransform.pivot = Vector2.one * (0.5f + 0.1f * (i-newCount/2));
            Color baseColor = blockImage.color;
            float baseColorHue, basecolorSaturation, baseColorBrightness;
            Color.RGBToHSV(baseColor, out baseColorHue, out basecolorSaturation, out baseColorBrightness);
            blockImage.color = Color.HSVToRGB(baseColorHue, basecolorSaturation, baseColorBrightness/(i+1));
        }
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
