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
        EventManager.onInventoryBlockTap += HandleInventoryBlockButtonTap;
    }

    public void Start()
    {
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

    public void HandleShopBlockButtonTap(BlockShopScript.Item itemArg)
    {
        selectedBlockPanelScript.gameObject.SetActive(true);
        Sprite correspondingBlockSprite = blockManagerScript.GetSpriteForPrefab(itemArg.blockPrefab);
        selectedBlockPanelScript.InitializeShopPanel(itemArg, correspondingBlockSprite);
    }


    public void HandleInventoryBlockButtonTap(GameObject blockPrefabArg)
    {
        selectedBlockPanelScript.gameObject.SetActive(true);
        Sprite correspondingBlockSprite = blockManagerScript.GetSpriteForPrefab(blockPrefabArg);
        int count = blockManagerScript.GetInventoryBlockCount(blockPrefabArg);
        selectedBlockPanelScript.InitializeInventoryPanel(correspondingBlockSprite, blockPrefabArg, count);
    }


    public void CreateAllShopButtons(List<BlockShopScript.Item> listOfItems)
    {
        currentShopOfferButtons = new List<GameObject>();
        foreach(BlockShopScript.Item itemTemp in listOfItems)
        {
            Sprite itemSprite = blockManagerScript.GetSpriteForPrefab(itemTemp.blockPrefab);
            GameObject newShopButton = CreateShopButton(itemSprite,itemTemp);
            currentShopOfferButtons.Add(newShopButton);
        }
    }

    public void RemoveUsedBlocks()
    {
        foreach(KeyValuePair<GameObject, BlockButtonScript> keyValuePair in inventoryButtons)
        {
            UpdateInventoryBlockCount(keyValuePair.Key, blockManagerScript.GetInventoryBlockCount(keyValuePair.Key));
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
        inventoryButtons = new Dictionary<GameObject, BlockButtonScript>();
        foreach(GameObject blockObjectTemp in blockManagerScript.blockConfig)
        {
            Sprite blockSprite = blockManagerScript.GetSpriteForPrefab(blockObjectTemp);
            GameObject newBlockButton = CreateInventoryButton(blockSprite, blockObjectTemp);
            BlockButtonScript newBlockButtonScript = newBlockButton.GetComponent<BlockButtonScript>();
            inventoryButtons.Add(blockObjectTemp, newBlockButtonScript);
        }
    }

    GameObject CreateInventoryButton(Sprite spriteArg, GameObject gameObjectArg)
    {
        GameObject newBlockButton = Instantiate(blockButtonPrefab,  inventoryItemsParent.transform);
        BlockButtonScript newBlockButtonScript = newBlockButton.GetComponent<BlockButtonScript>();
        newBlockButtonScript.InitializeInventoryButton(spriteArg, gameObjectArg);
        return newBlockButton;
    }

    GameObject CreateShopButton(Sprite spriteArg, BlockShopScript.Item shopItem)
    {
        GameObject newBlockButton = Instantiate(blockButtonPrefab, shopItemsParent.transform);
        BlockButtonScript newBlockButtonScript = newBlockButton.GetComponent<BlockButtonScript>();
        newBlockButtonScript.InitializeShopButton(spriteArg, shopItem, HandleShopBlockButtonTap);
        return newBlockButton;
    }

}
