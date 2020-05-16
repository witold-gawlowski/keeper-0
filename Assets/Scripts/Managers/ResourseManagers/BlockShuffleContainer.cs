using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockShuffleContainer : MonoBehaviour
{
    private List<GameObject> blocks;
    public BlockManagerScript blockManagerScript;
    
    public void Initialize()
    {
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
        blockManagerScript.RemoveBlock(Top());
        blocks.RemoveAt(0);
        return;
    }

    public List<GameObject> Top(int count)
    {
        List<GameObject> result = new List<GameObject>();
        for(int i=0; i<count && i < result.Count; i++)
        {
            result.Add(blocks[i]);
        }
        return result;
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
