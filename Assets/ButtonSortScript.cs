﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSortScript : MonoBehaviour
{
    public GameObject buttonsParent;
    public void Sort()
    {
        foreach (Transform buttonTransform in buttonsParent.transform)
        {
            BlockButtonScript blockButtonScript = buttonTransform.GetComponent<BlockButtonScript>();
            if (blockButtonScript)
            {
                if (blockButtonScript.associatedItem != null)
                {
                    buttonTransform.SetAsFirstSibling();
                }
            }
        }
        foreach (Transform buttonTransform in buttonsParent.transform)
        {
            if (buttonTransform.GetComponent<LevelButtonScript>())
            {
                buttonTransform.SetAsFirstSibling();
            }            
        }
    }
}