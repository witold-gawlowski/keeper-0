using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class CompletedLevelsUIScript : MonoBehaviour
{
    public GameObject levelsChunkPrefab;
    public GameObject chunksParentObject;
    public int chunkSize = 44;
    private void Awake()
    {
        EventManager.AddListener<UpdateCompletedLevelsUIEvent>(UpdateCompletedLevelsUIEventDispatcher);
    }

    public void UpdateCompletedLevelsUIEventDispatcher(IEvent evArg)
    {
        UpdateCompletedLevelsUIEvent evData = evArg as UpdateCompletedLevelsUIEvent;
        UpdateLevelChunks(evData.levels);
    }

    int ChunkNumber(int nArg)
    {
        return (nArg - 1) / chunkSize;
    }

    public void UpdateLevelChunks(List<int> levelsArg)
    {
        levelsArg.Sort();
        int numberOfLevels = levelsArg.Count;
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
