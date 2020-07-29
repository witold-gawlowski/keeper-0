﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDrawer : MonoBehaviour
{
    public GameObject levelPrefab;
    public LevelTypeScriptableObjectScript[] levelTypes;
    public int numberOfTypes;
    int displayedTypeCount;
    Randomizer randomizer;
    public int seed = 101;
    private void Awake()
    {
        displayedTypeCount = 0;
        
    }
    private void Update()
    {
        if(randomizer == null)
        {
            randomizer = new Randomizer(seed);
        }
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            transform.GetChild(Mathf.Abs(displayedTypeCount % numberOfTypes)).gameObject.SetActive(false);
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
                proceduralMap.Initialize(randomizer, type, 1, 1, false);
                levelTypeGameObject.SetActive(false);
                levelTypeGameObject.name = type.name;
            }
            numberOfTypes = levelTypes.Length;
            displayedTypeCount = -1;
            transform.GetChild(0).gameObject.SetActive(true);
        }


    }



}
