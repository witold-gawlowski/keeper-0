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
    public List<GameObject> currentShopOfferButtons;
    Dictionary<GameObject, BlockButtonScript> inventoryButtons;

    public void Awake()
    {
        selectedBlockPanelScript.SetBuyEventHandler(blockShopScript.Sell);
        EventManager.onInventoryBlockTap += HandleInventoryBlockButtonTap;
        EventManager.onBlockDeleted += UpdateInventoryBlockCount;
        EventManager.AddListener<UpdateOfferUIEvent>(HandleUpdateOfferEvent);
    }
       
    public void Start()
    {
        CreateInventoryButtons();
    }

    public void HandleUpdateOfferEvent(IEvent evArg)
    {
        UpdateOfferUIEvent evData = evArg as UpdateOfferUIEvent;
        ClearOfferButtons();
        CreateAllShopButtons(evData.cards);
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

    public void DeleteOfferItemButton(Card item)
    {
        foreach(GameObject offerButton in currentShopOfferButtons)
        {
            Card itemTemp = offerButton.GetComponent<BlockButtonScript>().associatedCard;
            if (itemTemp == item)
            {
                Destroy(offerButton);
                currentShopOfferButtons.Remove(offerButton);
                return;
            }
        }
    }

    public void HandleShopBlockButtonTap(Card itemArg)
    {
        selectedBlockPanelScript.gameObject.SetActive(true);
        Sprite correspondingBlockSprite = BlockCodexScript.instance.GetSpriteForPrefab(itemArg.block);
        selectedBlockPanelScript.InitializeShopPanel(itemArg, correspondingBlockSprite);
    }


    public void HandleInventoryBlockButtonTap(GameObject blockPrefabArg)
    {
        selectedBlockPanelScript.gameObject.SetActive(true);
        Sprite correspondingBlockSprite = BlockCodexScript.instance.GetSpriteForPrefab(blockPrefabArg);
        int count = blockManagerScript.GetInventoryBlockCount(blockPrefabArg);
        selectedBlockPanelScript.InitializeInventoryPanel(correspondingBlockSprite, blockPrefabArg, count);
    }


    public void CreateAllShopButtons(List<Card> listOfItems)
    {
        currentShopOfferButtons = new List<GameObject>();
        foreach(Card itemTemp in listOfItems)
        {
            Sprite itemSprite = BlockCodexScript.instance.GetSpriteForPrefab(itemTemp.block);
            GameObject newShopButton = CreateShopButton(itemSprite, itemTemp);
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
        foreach(GameObject blockObjectTemp in BlockCodexScript.instance.blockConfig)
        {
            Sprite blockSprite = BlockCodexScript.instance.GetSpriteForPrefab(blockObjectTemp);
            GameObject newBlockButton = CreateInventoryButton(blockSprite, blockObjectTemp);
            BlockButtonScript newBlockButtonScript = newBlockButton.GetComponent<BlockButtonScript>();
            inventoryButtons.Add(blockObjectTemp, newBlockButtonScript);
        }
    }

    GameObject CreateInventoryButton(Sprite spriteArg, GameObject gameObjectArg)
    {
        GameObject newBlockButton = Instantiate(blockButtonPrefab,  inventoryItemsParent.transform);
        BlockButtonScript newBlockButtonScript = newBlockButton.GetComponent<BlockButtonScript>();
        newBlockButtonScript.InitializeInventoryIIButton(spriteArg, gameObjectArg);
        return newBlockButton;
    }

    GameObject CreateShopButton(Sprite spriteArg, Card shopItem)
    {
        GameObject newBlockButton = Instantiate(blockButtonPrefab, shopItemsParent.transform);
        BlockButtonScript newBlockButtonScript = newBlockButton.GetComponent<BlockButtonScript>();
        newBlockButtonScript.InitializeShopButton(spriteArg, shopItem, HandleShopBlockButtonTap);
        return newBlockButton;
    }

}
