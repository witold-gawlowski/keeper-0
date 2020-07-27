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
    public GameObject emptyInventoryInfoText;
    public GameObject emptyShopInfoText;
    bool dirty;
    Dictionary<GameObject, BlockButtonScript> inventoryButtons;

    public void Awake()
    {
        selectedBlockPanelScript.SetBuyEventHandler(blockShopScript.Sell);
        EventManager.onInventoryBlockTap += HandleInventoryBlockButtonTap;
        EventManager.onBlockDeleted += UpdateInventoryBlockCount;
        EventManager.AddListener<UpdateOfferUIEvent>(HandleUpdateShopOfferEvent);
        dirty = false;
    }

    private void OnEnable()
    {
        if (dirty)
        {
            UpdateEmptyInventoryInfoVisibility();
            UpdateEmptyShopInfoVisibility();
            dirty = false;
        }
    }

    void Start()
    {
        InitializeInventoryButtons();
    }

    public void HandleUpdateShopOfferEvent(IEvent evArg)
    {
        UpdateOfferUIEvent evData = evArg as UpdateOfferUIEvent;
        ClearShopButtons();
        CreateShopButtons(evData.cards);
    }
    public void DeleteShopItemButton(Card item)
    {
        foreach(Transform offerButton in shopItemsParent.transform)
        {
            Card itemTemp = offerButton.GetComponent<BlockButtonScript>().associatedCard;
            if (itemTemp == item)
            {
                Destroy(offerButton.gameObject);
            }
        }
        if (this.isActiveAndEnabled)
        {
            StartCoroutine(UpdateEmptyShopInfoVisibility());
        }
        else
        {
            dirty = true;
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
    public void CreateShopButtons(List<Card> listOfItems)
    {
        foreach(Card itemTemp in listOfItems)
        {
            Sprite itemSprite = BlockCodexScript.instance.GetSpriteForPrefab(itemTemp.block);
            GameObject newShopButton = CreateShopButton(itemSprite, itemTemp);
        }
    }
    //public void RemoveUsedBlocks()
    //{
    //    foreach(KeyValuePair<GameObject, BlockButtonScript> keyValuePair in inventoryButtons)
    //    {
    //        UpdateInventoryBlockCount(keyValuePair.Key, blockManagerScript.GetInventoryBlockCount(keyValuePair.Key));
    //    }
    //}
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
        if (this.isActiveAndEnabled)
        {
            StartCoroutine(UpdateEmptyInventoryInfoVisibility());
        }
        else
        {
            dirty = true;
        }
    }
    IEnumerator UpdateEmptyShopInfoVisibility()
    {
        yield return new WaitForEndOfFrame();
        if (shopItemsParent.transform.childCount == 0)
        {
            emptyShopInfoText.SetActive(true);
        }
        else
        {
            emptyShopInfoText.SetActive(false);
        }
    }
    IEnumerator UpdateEmptyInventoryInfoVisibility()
    {
        yield return new WaitForEndOfFrame();
        foreach(Transform t in inventoryItemsParent.transform)
        {
            if (t.gameObject.activeInHierarchy)
            {
                emptyInventoryInfoText.SetActive(false);
                yield break;
            }
        }
        emptyInventoryInfoText.SetActive(true);
    }
    void InitializeInventoryButtons()
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
    void ClearShopButtons()
    {
        foreach (Transform shopOfferObject in shopItemsParent.transform)
        {
            Destroy(shopOfferObject.gameObject);
        }
        StartCoroutine(UpdateEmptyShopInfoVisibility());
    }
    GameObject CreateShopButton(Sprite spriteArg, Card shopItem)
    {
        GameObject newBlockButton = Instantiate(blockButtonPrefab, shopItemsParent.transform);
        BlockButtonScript newBlockButtonScript = newBlockButton.GetComponent<BlockButtonScript>();
        newBlockButtonScript.InitializeShopButton(spriteArg, shopItem, HandleShopBlockButtonTap);
        StartCoroutine(UpdateEmptyShopInfoVisibility());
        return newBlockButton;
    }

}
