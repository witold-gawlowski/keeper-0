using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BlockUIQueue : MonoBehaviour
{
    public int numberOfDisplayedBlocks;
    public GameObject blockImageObjectPrefab;
    List<Sprite> spriteQueue;

    public void  Init(List<Sprite> spriteQueueArg)
    {
        foreach(Transform t in transform)
        {
            Destroy(t.gameObject);
        }
        spriteQueue = spriteQueueArg;
        CreateDisplayQueue();
    }

    public void CreateDisplayQueue()
    {
        int counter = 0;
        foreach(Sprite tempSprite in spriteQueue)
        {
            GameObject newImageGO = Instantiate(blockImageObjectPrefab, transform);
            Image newImage = newImageGO.GetComponent<Image>();
            newImage.sprite = tempSprite;
            if (counter < numberOfDisplayedBlocks)
            {
                newImageGO.SetActive(true);
            }
            else
            {
                newImageGO.SetActive(false);
            }
            newImageGO.transform.SetAsFirstSibling();
            counter++;
        }
    }

    public void Next()
    {
        int lastChildIndex = transform.childCount - 1;
        Destroy(transform.GetChild(lastChildIndex).gameObject);
        transform.GetChild(Mathf.Max(0, lastChildIndex-numberOfDisplayedBlocks)).gameObject.SetActive(true);
    }
}
