using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockManagerScript : MonoBehaviour
{
    [System.Serializable]
    public class InitialBlockConfigData
    {
        public GameObject blockObject;
        public Sprite blockSprite;
        public int startCount;
        public InitialBlockConfigData(
            GameObject blockObjectArg,
            Sprite blockImageArg,
            int startCountArg = 0)
        {
            blockObject = blockObjectArg;
            blockSprite = blockImageArg;
            startCount = startCountArg;
        }
    }

    public List<InitialBlockConfigData> blockConfig;
    public BlockShopScript blockShopScript;
    public BlocksUIScript blocksUIScript;
    public BlockShuffleContainer blockShuffleContainer;
    Dictionary<GameObject, int> blockInventory;
    public BlockUIQueue blockUIQueue;
    public LevelManagerScript levelManagerScript;

    public int offersPerRound;

    public void Awake()
    {
        InitializeInventory();
    }

    public void OnStartBuilding(GameObject level)
    {
        blockShuffleContainer.Initialize();
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


    public void InitializeInventory()
    {
        blockInventory = new Dictionary<GameObject, int>();
        foreach(InitialBlockConfigData blockInitData in blockConfig)
        {
            blockInventory.Add(blockInitData.blockObject, blockInitData.startCount);
        }
    }

    public Sprite GetSpriteForPrefab(GameObject blockPrefab)
    {
        foreach(InitialBlockConfigData blockConfigData in blockConfig)
        {
            if(blockConfigData.blockObject == blockPrefab)
            {
                return blockConfigData.blockSprite;
            }
        }
        return null;
    }

    public void UpdateInventoryUI()
    {
        foreach(KeyValuePair<GameObject, int> inventoryElement in blockInventory)
        {
            blocksUIScript.UpdateInventoryBlockCount(inventoryElement.Key, inventoryElement.Value);
        }
    }

    public List<GameObject> GetBlockTypes()
    {
        var result = new List<GameObject>();
        foreach(InitialBlockConfigData blockData in blockConfig)
        {
            result.Add(blockData.blockObject);
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
