﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class CompletedLevelsUIScript : MonoBehaviour
{
    public GameObject levelsChunkPrefab;
    public GameObject chunksParentObject;
    public GameObject noLevelsText;
    public int chunkSize = 44;
    private void Awake()
    {
        EventManager.AddListener<UpdateCompletedLevelsUIEvent>(UpdateCompletedLevelsUIEventDispatcher);
    }

    public void UpdateCompletedLevelsUIEventDispatcher(IEvent evArg)
    {
        UpdateCompletedLevelsUIEvent evData = evArg as UpdateCompletedLevelsUIEvent;
        UpdateUI(evData.levels);
    }

    int ChunkNumber(int nArg)
    {
        return (nArg - 1) / chunkSize;
    }

    void UpdateUI(List<int> levelsArg)
    {
        foreach (Transform t in chunksParentObject.transform)
        {
            Destroy(t.gameObject);
        }
        if (levelsArg.Count == 0)
        {
            noLevelsText.SetActive(true);
        }
        else
        {
            noLevelsText.SetActive(false);
            UpdateLevelChunks(levelsArg);
        }
    }

    public void UpdateLevelChunks(List<int> levelsArg)
    {
        int numberOfLevels = levelsArg.Count;
        levelsArg.Sort();
        string currentChunkText = "";
        List<string> textChunks = new List<string>();
        for(int i=0; i<numberOfLevels; i++)
        {
            int givenNumber = levelsArg[i];
            int chunkNumber = ChunkNumber(givenNumber);
            currentChunkText += givenNumber + " ";
            bool endChunk = (i == numberOfLevels - 1) || (chunkNumber != ChunkNumber(levelsArg[i+1]));
            if (endChunk)
            {
                textChunks.Add(currentChunkText);
                currentChunkText = "";
            }
        }


        foreach(string sChunk in textChunks)
        {
            GameObject chunkObject = Instantiate(levelsChunkPrefab, chunksParentObject.transform);
            Text chunkText = chunkObject.GetComponent<Text>();
            chunkText.text = sChunk;
        }
    }

}
