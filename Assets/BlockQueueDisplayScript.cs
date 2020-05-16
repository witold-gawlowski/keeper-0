using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockQueueDisplayScript : MonoBehaviour
{
    public int numberOfDisplayedBlocks = 3;
    public BlockShuffleContainer blocksQueue;
    public BlockManagerScript blockManager;
    public List<GameObject> displaySprites;

    public void CreateQueue()
    {
        foreach(GameObject blockSpriteRendererObject in displaySprites)
        {
            blockSpriteRendererObject.SetActive(false);
        }
        List<GameObject> topBlocks = blocksQueue.Top(numberOfDisplayedBlocks);
        int displaySpritesIndex = 0;
        foreach(GameObject blockObject in topBlocks)
        {
            Sprite blockSprite = blockManager.GetSpriteForPrefab(blockObject);
            GameObject blockSpriteRendererObject = displaySprites[displaySpritesIndex];
            blockSpriteRendererObject.SetActive(true);
            SpriteRenderer blockSR = blockSpriteRendererObject.GetComponent<SpriteRenderer>();
            blockSR.sprite = blockSprite;
        }
    }

    public void Pop()
    {

    }
}
