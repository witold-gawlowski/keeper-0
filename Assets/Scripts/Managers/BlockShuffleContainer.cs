using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockShuffleContainer : MonoBehaviour
{
    public class BlockSortingElement
    {
        public GameObject block;
        public float sortingValue;
    }
    private List<BlockSortingElement> blocks;
    public BlockManagerScript blockManagerScript;
    public GlobalManagerScript globalManager;
    public BlockUIQueue blockUIQueue;
    public BuildingUIScript buildingUIScript;
    private List<GameObject> pendingRemoval;
    private Dictionary<GameObject, int> blockTypeCounter;
    public LevelMoneyManagerScript levelMoneyManager;
    List<float> blockSortingValues;
    int seed;
    int bigPrime = 2147483647;
    

    public void Initialize(int seedArg)
    {
        seed = seedArg;
        blockTypeCounter = new Dictionary<GameObject, int>();
        pendingRemoval = new List<GameObject>();
        blocks = new List<BlockSortingElement>();
        List<GameObject> blockGameObjects = blockManagerScript.GetIndividualBlockList();
        foreach(GameObject blockGameObject in blockGameObjects)
        {
            BlockSortingElement newBlockSortingElement = new BlockSortingElement();
            newBlockSortingElement.block = blockGameObject;
            blocks.Add(newBlockSortingElement);
        }
        Shuffle();
    }

    int NumberOfProcessedBlocks(GameObject blockType)
    {
        if (blockTypeCounter.ContainsKey(blockType))
        {
            return blockTypeCounter[blockType];
        }
        return 0;
    }

    void RegisterBlock(GameObject blockType)
    {
        if (blockTypeCounter.ContainsKey(blockType))
        {
            blockTypeCounter[blockType]++;
        }
        else
        {
            blockTypeCounter.Add(blockType, 0);
        }
    }

    public class Comp : IComparer<BlockSortingElement>
    {
        public int Compare(BlockSortingElement x, BlockSortingElement y)
        {
            if(x.sortingValue < y.sortingValue)
            {
                return 1;
            }
            if(x.sortingValue > y.sortingValue)
            {
                return -1;
            }
            return 0;
        }
    }

    public void Shuffle()
    {
        int n = blocks.Count;
        for (int i = 0; i < n; i++)
        {
            GameObject currentBlockTemp = blocks[i].block;
            int numberOfBlocksOfCurrentBlockType = NumberOfProcessedBlocks(currentBlockTemp);
            int localSeed = (seed * numberOfBlocksOfCurrentBlockType * globalManager.GetCurrentLevel()) % bigPrime;
            blocks[i].sortingValue = Tools.SeededRandom(localSeed);
            RegisterBlock(currentBlockTemp);
        }
        blocks.Sort(new Comp());

    }

    public void Pop()
    {
        pendingRemoval.Add(Top());
        blocks.RemoveAt(0);
        blockUIQueue.Next();
        return;
    }

    public void OnLevelCompleted()
    {
        blockManagerScript.RemoveBlocks(pendingRemoval);
    }

    public List<GameObject> GetBlocks()
    {
        List<GameObject> result = new List<GameObject>();
        for(int i=0; i<blocks.Count; i++){
            result.Add(blocks[i].block);
        }
        return result;
    } 

    public GameObject Top()
    { 
        if (blocks.Count == 0)
        {
            return null;
        }
        return blocks[0].block;
    }

}
