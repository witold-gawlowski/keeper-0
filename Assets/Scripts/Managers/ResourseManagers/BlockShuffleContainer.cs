using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockShuffleContainer : MonoBehaviour
{
    private List<GameObject> blocks;
    public BlockManagerScript blockManagerScript;
    public BlockUIQueue blockUIQueue;
    private List<GameObject> pendingRemoval;

    public void Initialize()
    {
        pendingRemoval = new List<GameObject>();
        blocks = blockManagerScript.GetIndividualBlockList();
        Shuffle();
    }

    public void Shuffle()
    {
        int n = blocks.Count;
        for (int i = n - 1; i >= 1; i--)
        {
            int j = Random.Range(0, i + 1);
            GameObject temp = blocks[i];
            blocks[i] = blocks[j];
            blocks[j] = temp;
        }
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
        foreach(GameObject blockObject in pendingRemoval)
        {
            blockManagerScript.RemoveBlock(blockObject);
        }
    }

    public List<GameObject> GetBlocks()
    {
        return blocks;
    } 

    public GameObject Top()
    { 
        if (blocks.Count == 0)
        {
            return null;
        }
        return blocks[0];
    }

}
