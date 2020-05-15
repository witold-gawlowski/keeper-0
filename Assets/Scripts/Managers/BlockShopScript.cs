using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockShopScript : MonoBehaviour
{
    public class Item
    {
        public GameObject blockPrefab{ get; set; }
        public int count { get; set; }
        public int price { get; set; }
    }

    List<Item> offer;
    public accountManager accountManager;
    public BlockManagerScript blockManagerScript;
    public BlocksUIScript blocksUIScript;
    List<GameObject> blockTypes;
    public int offerCount = 4;
    public int minBlocksInOffer = 1;
    public int maxBlocksInOffer = 4;
    public int bundleDefaultPrice = 300;

    private void Awake()
    {
        blockTypes = blockManagerScript.GetBlockTypes();
    }

    public void RerollOffer()
    {
        offer = new List<Item>();
        for(int i=0; i<offerCount; i++)
        {
            int blocksCountTemp = Random.Range(minBlocksInOffer, maxBlocksInOffer + 1);
            int randomTypeIndexTemp = Random.Range(0, blockTypes.Count);
            GameObject blockPrefabTemp = blockTypes[randomTypeIndexTemp];
            offer.Add(new Item() {
                blockPrefab = blockPrefabTemp,
                count = blocksCountTemp,
                price = bundleDefaultPrice
            });
        }
    }

    public void OnNewRoundStart()
    {
        RerollOffer();
        blocksUIScript.ClearOfferButtons();
        blocksUIScript.CreateAllShopButtons(offer);
    }
   
    public void Buy(Item item)
    {
        if (accountManager.TryPay(item.price))
        {
            offer.Remove(item);
            blockManagerScript.IncreaseInventoryBlockCount(item.blockPrefab, item.count);
            blocksUIScript.DeleteOfferItemButton(item);
        }
    }
}
