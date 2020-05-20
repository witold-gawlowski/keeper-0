using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawnerScript : MonoBehaviour
{
    public BlockShuffleContainer blockFeeder;
    [HideInInspector]
    public GameObject nextBlock;
    public BlockManagerScript blockManager;
    int rotationCountBeforeSpawning;

    public void Awake()
    {
        ResetRotation();
    }

    public void ResetRotation(int dummyArg = 0, int dummyArg2 = 0)
    {
        rotationCountBeforeSpawning = 0;
    }

    public void ClearAllBlocks()
    {
        foreach(Transform blockTransform in transform)
        {
            Destroy(blockTransform.gameObject);
        }
    }

    public void HandleRotEvent()
    {
        rotationCountBeforeSpawning = (rotationCountBeforeSpawning + 1) % 4;
    }

    public void SpawnNextBlock()
    {
        GameObject nextBlockPrefab = blockFeeder.Top();
        if (nextBlockPrefab != null)
        {
            nextBlock = Instantiate(nextBlockPrefab, transform.position, Quaternion.identity);
            BlockScript blockScriptTemp = nextBlock.GetComponent<BlockScript>();
            for(int i=0; i<rotationCountBeforeSpawning; i++)
            {
                blockScriptTemp.Rotate();
            }
            nextBlock.transform.parent = transform;
        }
    }
}
