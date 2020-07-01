using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSortScript : MonoBehaviour
{
    public GameObject inventoryButtonsParent;
    public void Sort()
    {
        foreach (Transform buttonTransform in inventoryButtonsParent.transform)
        {
            BlockButtonScript blockButtonScript = buttonTransform.GetComponent<BlockButtonScript>();
            if (blockButtonScript)
            {
                if (blockButtonScript.associatedCard != null)
                {
                    buttonTransform.SetAsFirstSibling();
                }
            }
        }
        foreach (Transform buttonTransform in inventoryButtonsParent.transform)
        {
            if (buttonTransform.GetComponent<LevelButtonScript>())
            {
                buttonTransform.SetAsFirstSibling();
            }            
        }
    }
}
