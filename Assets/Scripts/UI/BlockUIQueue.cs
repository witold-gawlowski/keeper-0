using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BlockUIQueue : MonoBehaviour
{
    public class BlockUIData{
        public Sprite sprite;
        public int valueToDisplay;
        public bool starter;
        public BlockUIData(Sprite spriteArg, int valueToDisplayArg)
        {
            sprite = spriteArg;
            valueToDisplay = valueToDisplayArg;
        }
    }

    public int numberOfDisplayedBlocks;
    public GameObject blockImageObjectPrefab;
    List<BlockUIData> blockUIDataQueue;

    public void  Init(List<BlockUIData> blockUIDataList)
    {
        foreach(Transform t in transform)
        {
            Destroy(t.gameObject);
        }
        blockUIDataQueue = blockUIDataList;
        CreateDisplayQueue();
    }

    public void RotateTop()
    {
        int lastChildIndex = transform.childCount - 1;
        Transform lastChild = transform.GetChild(lastChildIndex);
        RectTransform lastChildRectTransform = lastChild.GetComponent<RectTransform>();
        lastChildRectTransform.Rotate(Vector3.back, -90);
    }

    public void CreateDisplayQueue()
    {
        int counter = 0;
        foreach(BlockUIData tempBlockUIData in blockUIDataQueue)
        {
            Sprite tempSprite = tempBlockUIData.sprite;
            GameObject newImageGO = Instantiate(blockImageObjectPrefab, transform);
            UIBlockScript tempUIBlockScript = newImageGO.GetComponent<UIBlockScript>();
            tempUIBlockScript.UpdateNominalValueText(tempBlockUIData.valueToDisplay);
            tempUIBlockScript.SetBlockSprite(tempSprite);

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
    public void SetTopVisible(bool valueArg)
    {
        int lastChildIndex = transform.childCount - 1;
        transform.GetChild(lastChildIndex).gameObject.SetActive(valueArg);
    }


    public void Next()
    {
        int lastChildIndex = transform.childCount - 1;
        Destroy(transform.GetChild(lastChildIndex).gameObject);
        transform.GetChild(Mathf.Max(0, lastChildIndex - numberOfDisplayedBlocks)).gameObject.SetActive(true);
    }
}
