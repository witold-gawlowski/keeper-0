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
            currentChunkText += givenNumber + " ";
            int chunkNumber = ChunkNumber(givenNumber);
            if (chunkNumber != ChunkNumber(givenNumber - 1))
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
