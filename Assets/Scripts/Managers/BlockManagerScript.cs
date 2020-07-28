using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockManagerScript : MonoBehaviour
{
    public static BlockManagerScript ins;

    public BlockShopScript blockShopScript;
    public BlocksUIScript blocksUIScript;
    public BlockShuffleContainer blockShuffleContainer;
    Dictionary<GameObject, int> blockInventory;
    public BlockUIQueue blockUIQueue;
    public LevelManagerScript levelManagerScript;
    public int offersPerRound;
    public int seed = 101;


    EventManager.EventDelegate deleteEventDelegate;

    public void Awake()
    {
        ins = this;
        blockInventory = new Dictionary<GameObject, int>();
        EventManager.AddListener<SelectedBlockPanelScript.DeleteButtonEvent>(DeleteEventHandler);
    }

    public void DeleteEventHandler(IEvent e)
    {
        var  a = e as SelectedBlockPanelScript.DeleteButtonEvent;
        RemoveBlock(a.blockPrefab);
    }

    public bool IsInventoryEmpty()
    {
        foreach(KeyValuePair<GameObject, int> kvp in blockInventory)
        {
            if (kvp.Value > 0)
            {
                return false;
            }
        }
        return true;
    }

    public void OnStartBuilding(GameObject level)
    {
        blockShuffleContainer.Initialize(seed);
        List<GameObject> blocksQueue = blockShuffleContainer.GetBlocks();
        List<BlockUIQueue.BlockUIData> blockUIData = new List<BlockUIQueue.BlockUIData>();
        for(int i=0; i<blocksQueue.Count; i++) {
            bool isFirst = i == 0;
            GameObject blockQueueObject = blocksQueue[i];
            Sprite tempSprite = BlockCodexScript.instance.GetSpriteForPrefab(blockQueueObject);
            BlockScript blockQueueObjectScript = blockQueueObject.GetComponent<BlockScript>();
            int blockRewardValue = Mathf.RoundToInt(levelManagerScript.GetReturnValue(level) * blockQueueObjectScript.value);
            BlockUIQueue.BlockUIData tempBlockUIData = new BlockUIQueue.BlockUIData(tempSprite, blockRewardValue);
            blockUIData.Add(tempBlockUIData);
        }
        blockUIQueue.Init(blockUIData);
    }

    public int GetTotalInventoryArea()
    {
        int result = 0;
        foreach(KeyValuePair<GameObject, int> bpo in blockInventory){
            BlockScript bs = bpo.Key.GetComponent<BlockScript>();
            if (bs == null)
            {
                Debug.Log("missing blockscript!");
            }
            else
            {
                result += bs.GetArea() * bpo.Value;
            }
        }
        return result;
    }


    public int GetInventoryBlockCount(GameObject blockObject)
    {
        if (blockInventory.ContainsKey(blockObject))
        {
            return blockInventory[blockObject];
        }
        return 0;
    }

    public void RemoveBlock(GameObject blockType)
    {
        blockInventory[blockType] = blockInventory[blockType] - 1;
        int blockCountTemp = GetInventoryBlockCount(blockType);
        EventManager.RaiseOnBlockDeleted(blockType, blockCountTemp);
    }

    public void RemoveBlocks(List<GameObject> blocksArg)
    {
        foreach (GameObject blockObject in blocksArg)
        {
            RemoveBlock(blockObject);
        }
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
