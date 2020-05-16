using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawnerScript : MonoBehaviour
{
    public BlockShuffleContainer blockFeeder;
    [HideInInspector]
    public GameObject nextBlock;

    public void OnStartBuilding() {
        
    }

    public void ClearAllBlocks()
    {
        foreach(Transform blockTransform in transform)
        {
            Destroy(blockTransform.gameObject);
        }
    }

    public void SpawnNextBlock()
    {
        GameObject nextBlockPrefab = blockFeeder.Top();
        if (nextBlockPrefab != null)
        {
            nextBlock = Instantiate(nextBlockPrefab, transform.position, Quaternion.identity);
            nextBlock.transform.parent = transform;
        }
    }
}
