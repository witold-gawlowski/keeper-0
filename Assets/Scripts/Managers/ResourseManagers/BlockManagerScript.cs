using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockManagerScript : MonoBehaviour
{
    Dictionary<GameObject, Sprite> blockImages;
    public List<GameObject> blockConfig;
    public BlockShopScript blockShopScript;
    public BlocksUIScript blocksUIScript;
    public BlockShuffleContainer blockShuffleContainer;
    Dictionary<GameObject, int> blockInventory;
    public BlockUIQueue blockUIQueue;
    public LevelManagerScript levelManagerScript;
    public int offersPerRound;
    public int seed = 101;

    public void Awake()
    {
        InitializeSprites();
        blockInventory = new Dictionary<GameObject, int>();
    }

    void InitializeSprites()
    {
        blockImages = new Dictionary<GameObject, Sprite>();
        foreach(GameObject blockObjectTemp in blockConfig)
        {
            string blockPrefabName = blockObjectTemp.name;
            Sprite blockSprite = Resources.Load<Sprite>("Blocks/" + blockPrefabName);
            blockImages.Add(blockObjectTemp, blockSprite);
        }
    }

    public void OnStartBuilding(GameObject level)
    {
        blockShuffleContainer.Initialize(seed);
        List<GameObject> blocksQueue = blockShuffleContainer.GetBlocks();
        List<BlockUIQueue.BlockUIData> blockUIData = new List<BlockUIQueue.BlockUIData>();
        foreach(GameObject blockQueueObject in blocksQueue)
        {
            Sprite tempSprite = GetSpriteForPrefab(blockQueueObject);
            BlockScript blockQueueObjectScript = blockQueueObject.GetComponent<BlockScript>();
            int blockRewardValue = Mathf.RoundToInt(levelManagerScript.GetReturnValue(level) * blockQueueObjectScript.value);
            BlockUIQueue.BlockUIData tempBlockUIData = new BlockUIQueue.BlockUIData(tempSprite, blockRewardValue);
            blockUIData.Add(tempBlockUIData);
        }
        blockUIQueue.Init(blockUIData);

    }


    public Sprite GetSpriteForPrefab(GameObject blockPrefab)
    {
        if (!blockImages.ContainsKey(blockPrefab))
        {
            print("missing  image!");
        }
        return blockImages[blockPrefab];
    }


    public List<GameObject> GetBlockTypes()
    {
        var result = new List<GameObject>();
        foreach(GameObject blockObjectTemp in blockConfig)
        {
            result.Add(blockObjectTemp);
        }
        return result;
    }

    public void RemoveBlock(GameObject blockType)
    {
        blockInventory[blockType] = blockInventory[blockType] - 1;
    }

    public void IncreaseInventoryBlockCount(GameObject blockType, int blockCount)
    {
        if (blockInventory.ContainsKey(blockType))
        {
            blockInventory[blockType] = blockInventory[blockType] + blockCount;
        }
        else
        {
            blockInventory[blockType] = blockCount;
        }
        blocksUIScript.UpdateInventoryBlockCount(blockType, blockInventory[blockType]);
    }

    public List<GameObject> GetIndividualBlockList()
    {
        List<GameObject> result = new List<GameObject>();
        foreach(KeyValuePair<GameObject, int> inventoryItem in blockInventory)
        {
            for(int i=0; i<inventoryItem.Value; i++)
            {
                result.Add(inventoryItem.Key);
            }
        }
        return result;
    }

}
