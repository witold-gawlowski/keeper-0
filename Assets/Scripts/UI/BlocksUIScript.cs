using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlocksUIScript : MonoBehaviour
{

    public class BlockUIData
    {
        public GameObject associatedBlockPrefab;
        public Sprite blockSprite;
        public int blockCount;
        public int bundlePrice;
        public bool owned;

        public BlockUIData(GameObject associatedBlockPrefabArg,
            Sprite blockSpriteArg,
            int blockCountArg,
            bool ownedArg = true,
            int bundlePriceArg = 0)
        {
            associatedBlockPrefab = associatedBlockPrefabArg;
            blockSprite = blockSpriteArg;
            blockCount = blockCountArg;
            owned = ownedArg;
            bundlePrice = bundlePriceArg;
        }
    }
    public GameObject blockButtonPrefab;
    public GameObject shopItemsParent;
    public GameObject inventoryItemsParent;
    public SelectedBlockPanelScript selectedBlockPanelScript;
    public BlockManagerScript blockManagerScript;
    public BlockShopScript blockShopScript;
    List<GameObject> currentShopOfferButtons;
    Dictionary<GameObject, BlockButtonScript> inventoryButtons;

    public void Awake()
    {
        selectedBlockPanelScript.SetBuyEventHandler(blockShopScript.Buy);
        CreateInventoryButtons();
    }

    public void ClearOfferButtons()
    {
        if (currentShopOfferButtons != null)
        {
            foreach (GameObject shopOfferObject in currentShopOfferButtons)
            {
                Destroy(shopOfferObject);
            }
        }
    }

    public void DeleteOfferItemButton(BlockShopScript.Item item)
    {
        foreach(GameObject offerButton in currentShopOfferButtons)
        {
            BlockShopScript.Item itemTemp = offerButton.GetComponent<BlockButtonScript>().associatedItem;
            if (itemTemp == item)
            {
                currentShopOfferButtons.Remove(offerButton);
                Destroy(offerButton);
                return;
            }
        }
    }

    public void HandleBlockButtonTap(BlockShopScript.Item itemArg)
    {
        selectedBlockPanelScript.gameObject.SetActive(true);
        Sprite correspondingBlockSprite = blockManagerScript.GetSpriteForPrefab(itemArg.blockPrefab);
        selectedBlockPanelScript.Initialize(itemArg, correspondingBlockSprite);
    }

    public void CreateAllShopButtons(List<BlockShopScript.Item> listOfItems)
    {
        currentShopOfferButtons = new List<GameObject>();
        foreach(BlockShopScript.Item itemTemp in listOfItems)
        {
            Sprite itemSprite = blockManagerScript.GetSpriteForPrefab(itemTemp.blockPrefab);
            GameObject newShopButton = CreateButton(itemSprite, itemTemp.count, itemTemp, itemTemp.price);
            currentShopOfferButtons.Add(newShopButton);
        }
    }

    public void UpdateInventoryBlockCount(GameObject blockTypeArg, int newCountArg)
    {
        BlockButtonScript correspondingButtonScript = inventoryButtons[blockTypeArg];
        if (newCountArg == 0)
        {
            correspondingButtonScript.gameObject.SetActive(false);
        }
        else
        {
            correspondingButtonScript.gameObject.SetActive(true);
            correspondingButtonScript.UpdateCount(newCountArg);
        }
    }

    public void CreateInventoryButtons()
    {
        print("create inventory buttons");
        inventoryButtons = new Dictionary<GameObject, BlockButtonScript>();
        foreach(BlockManagerScript.InitialBlockConfigData blockData in blockManagerScript.blockConfig)
        {
            GameObject newBlockButton = CreateButton(blockData.blockSprite, 0);
            BlockButtonScript newBlockButtonScript = newBlockButton.GetComponent<BlockButtonScript>();
            inventoryButtons.Add(blockData.blockObject, newBlockButtonScript);
        }
    }

    GameObject CreateButton(
        Sprite spriteArg,
        int blockCountArg,
        BlockShopScript.Item shopItem = null,
        int priceArg = 0)
    {
        GameObject newBlockButton = Instantiate(
            blockButtonPrefab,
            shopItem != null ? shopItemsParent.transform : inventoryItemsParent.transform);
        BlockButtonScript newBlockButtonScript = newBlockButton.GetComponent<BlockButtonScript>();
        newBlockButtonScript.Initialize(
              spriteArg,
              blockCountArg,
              shopItem,
              priceArg);
        if(shopItem != null)
        {
            newBlockButtonScript.SetTapHandler(HandleBlockButtonTap);
        }
        return newBlockButton;
    }

}
