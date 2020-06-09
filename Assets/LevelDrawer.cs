﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDrawer : MonoBehaviour
{
    public GameObject levelPrefab;
    public LevelTypeScriptableObjectScript[] levelTypes;
    public int numberOfTypes;
    int displayedTypeCount;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            transform.GetChild(displayedTypeCount).gameObject.SetActive(false);
            displayedTypeCount = (displayedTypeCount + 1 ) % numberOfTypes;
            transform.GetChild(displayedTypeCount).gameObject.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            foreach(Transform t in transform)
            {
                Destroy(t.gameObject);
            }

            foreach (LevelTypeScriptableObjectScript type in levelTypes)
            {
                GameObject levelTypeGameObject = Instantiate(levelPrefab, transform);
                ProceduralMap proceduralMap = levelTypeGameObject.GetComponent<ProceduralMap>();
                foreach(SpriteRenderer sr in levelTypeGameObject.GetComponentsInChildren<SpriteRenderer>())
                {
                    sr.enabled = true;
                }
                proceduralMap.Initialize(type);
                levelTypeGameObject.SetActive(false);
            }
            numberOfTypes = levelTypes.Length;
            displayedTypeCount = 0;
            transform.GetChild(0).gameObject.SetActive(true);
        }


    }



}