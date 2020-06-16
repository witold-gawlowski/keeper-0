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
    public List<Vector2> NumberOfBlocksInBundleByCathegory;
    public List<Vector2> BundlePriceByCathegory;
    public List<Vector2> NumberOfBundlesByCathegory;
    public List<Vector2> NumberOfBlockTypesByCathegory;
    List<GameObject> blockTypes;
    public int offerCount = 4;
    public int minBlocksInOffer = 1;
    public int maxBlocksInOffer = 4;
    public int bundleDefaultPrice = 300;
    int maxCathegory = 3;
    public List<List<GameObject>> cathegoryTypes;

    private void Awake()
    {
        blockTypes = blockManagerScript.GetBlockTypes();
        InitCathegoryTypes();
    }

    List<GameObject> GetRandomSubset(List<GameObject> set, int subsetSize)
    {
        List<GameObject> result = new List<GameObject>();
        for(int i=0; i<subsetSize; i++)
        {
            int randomIndex = Random.Range(0, set.Count);
            GameObject extracted = set[randomIndex];
            set.RemoveAt(randomIndex);
            result.Add(extracted);
        }
        return result;
    }

    List<GameObject> FilterBlockTypes(int cathegoryArg)
    {
        List<GameObject> result = new List<GameObject>();
        foreach (GameObject blockTypeTemp in blockTypes)
        {
            BlockScript blockScriptTemp = blockTypeTemp.GetComponent<BlockScript>();
            if (blockScriptTemp.blockCathegory == cathegoryArg)
            {
                result.Add(blockTypeTemp);
            }
        }
        return result;
    }

    void InitCathegoryTypes()
    {
        cathegoryTypes = new List<List<GameObject>>();
        for (int i=0; i<=maxCathegory; i++)
        {
            int numberOfBlockTypes =
                Tools.RefinedGauss(NumberOfBlocksInBundleByCathegory[i].x, NumberOfBlocksInBundleByCathegory[i].y, 1);
            List<GameObject> cathegoryBlockTypes = FilterBlockTypes(i);
            List<GameObject> randomCathegoryBlockTypes = GetRandomSubset(cathegoryBlockTypes, numberOfBlockTypes);
            cathegoryTypes.Add(randomCathegoryBlockTypes);
        }
    }

    public void RerollOffer()
    {
        offer = new List<Item>();
        for (int cathegory = 0; cathegory <= maxCathegory; cathegory++)
        {
            int numberOfBundles =
                Tools.RefinedGauss(NumberOfBundlesByCathegory[cathegory].x, NumberOfBundlesByCathegory[cathegory].y, 0);
            for(int j = 0; j<numberOfBundles; j++)
            {
                int numberOfTypesForCathegory = cathegoryTypes[cathegory].Count;
                int typeIndex = Random.Range(0, numberOfTypesForCathegory);
                GameObject bundlePrefabTemp = cathegoryTypes[cathegory][typeIndex];
                int bundlePriceTemp = Tools.RefinedGauss(BundlePriceByCathegory[cathegory].x, BundlePriceByCathegory[cathegory].y);
                int numberOfBlocksInBundleTemp = Tools.RefinedGauss(NumberOfBlocksInBundleByCathegory[cathegory].x,
                                                                NumberOfBlocksInBundleByCathegory[cathegory].y);
                offer.Add(new Item()
                {
                    blockPrefab = bundlePrefabTemp,
                    count = numberOfBlocksInBundleTemp,
                    price = bundlePriceTemp
                });
            }
        }
    }

    //public void RerollOffer()
    //{
    //    offer = new List<Item>();
    //    for(int i=0; i<offerCount; i++)
    //    {
    //        int blocksCountTemp = Random.Range(minBlocksInOffer, maxBlocksInOffer + 1);
    //        int randomTypeIndexTemp = Random.Range(0, blockTypes.Count);
    //        GameObject blockPrefabTemp = blockTypes[randomTypeIndexTemp];
    //        offer.Add(new Item() {
    //            blockPrefab = blockPrefabTemp,
    //            count = blocksCountTemp,
    //            price = bundleDefaultPrice
    //        });
    //    }
    //}

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
