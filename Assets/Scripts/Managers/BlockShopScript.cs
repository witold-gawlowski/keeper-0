using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockShopScript : MonoBehaviour
{


    List<Card> offer;
    public accountManager accountManager;
    public BlockManagerScript blockManagerScript;
    public BlocksUIScript blocksUIScript;
    public List<Tools.Distribution> NumberOfBlocksInBundleByCathegory;
    public List<Vector2> BundlePriceByCathegory;
    public List<Tools.Distribution> NumberOfBundlesByCathegory;
    public List<Tools.Distribution> NumberOfBlockTypesByCathegory;
    List<GameObject> blockTypes;
    public int offerCount = 4;
    public int minBlocksInOffer = 1;
    public int maxBlocksInOffer = 4;
    public int bundleDefaultPrice = 300;
    int maxCathegory = 3;
    public List<List<GameObject>> cathegoryTypes;
    Randomizer randomizer;
    Deck deck;

    void Awake()
    {
        deck = FindObjectOfType<Deck>();
        randomizer = new Randomizer(blockManagerScript.seed);
        blockTypes = blockManagerScript.GetBlockTypes();
        InitCathegoryTypes();
    }

    List<GameObject> GetRandomSubset(List<GameObject> set, int subsetSize)
    {
        List<GameObject> result = new List<GameObject>();
        for(int i=0; i<subsetSize; i++)
        {
            int randomIndex = randomizer.Range(0, set.Count);
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
        for (int i=0; i<maxCathegory; i++)
        {
            int numberOfBlockTypes = Tools.RandomFromDistribution(NumberOfBlocksInBundleByCathegory[i], randomizer);
            List<GameObject> cathegoryBlockTypes = FilterBlockTypes(i);
            List<GameObject> randomCathegoryBlockTypes = GetRandomSubset(cathegoryBlockTypes, numberOfBlockTypes);
            cathegoryTypes.Add(randomCathegoryBlockTypes);
        }
    }

    void RerollOffer()
    {
        offer = new List<Card>();
        for (int cathegory = 0; cathegory < maxCathegory; cathegory++)
        {
            int numberOfBundles = Tools.RandomFromDistribution(NumberOfBundlesByCathegory[cathegory], randomizer);
            for(int j = 0; j<numberOfBundles; j++)
            {
                int numberOfTypesForCathegory = cathegoryTypes[cathegory].Count;
                int typeIndex = randomizer.Range(0, numberOfTypesForCathegory);
                GameObject bundlePrefabTemp = cathegoryTypes[cathegory][typeIndex];
                int bundlePriceTemp = Tools.RandomIntegerFromGaussianWithThreshold(randomizer, BundlePriceByCathegory[cathegory].x, BundlePriceByCathegory[cathegory].y);
                int numberOfBlocksInBundleTemp = Tools.RandomFromDistribution(NumberOfBlocksInBundleByCathegory[cathegory], randomizer); 
                offer.Add(new Card()
                {
                    block = bundlePrefabTemp,
                    quantity = numberOfBlocksInBundleTemp,
                    cashCost = bundlePriceTemp
                });
            }
        }
    }

    //public void RerollOffer()
    //{
    //    offer = new List<Item>();
    //    for(int i=0; i<offerCount; i++)
    //    {
    //        int blocksCountTemp = randomizer.Range(minBlocksInOffer, maxBlocksInOffer + 1);
    //        int randomTypeIndexTemp = randomizer.Range(0, blockTypes.Count);
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
   
    public void Buy(Card item)
    {
        if (accountManager.TryPay(item.cashCost))
        {
            offer.Remove(item);
            blockManagerScript.IncreaseInventoryBlockCount(item.block, item.quantity);
            blocksUIScript.DeleteOfferItemButton(item);
        }
    }
}
