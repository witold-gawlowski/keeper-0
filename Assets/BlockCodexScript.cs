using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCodexScript : MonoBehaviour
{
    public static BlockCodexScript instance;

    Dictionary<GameObject, Sprite> blockImages;
    public List<GameObject> blockConfig;
    Dictionary<string, GameObject> dictionary;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        InitializeSprites();
        InitializeDictionary();
    }

    public GameObject GetRandomBlock(Randomizer rArg)
    {
        int numberOfBlocks = blockConfig.Count;
        int randomIndex = rArg.Range(0, numberOfBlocks);
        return blockConfig[randomIndex];
    }

    public Sprite GetSpriteForPrefab(GameObject blockPrefab)
    {
        if (!blockImages.ContainsKey(blockPrefab))
        {
            print("missing  image!");
        }
        return blockImages[blockPrefab];
    }

    public List<GameObject> GetBlockTypes()
    {
        var result = new List<GameObject>();
        foreach (GameObject blockObjectTemp in blockConfig)
        {
            result.Add(blockObjectTemp);
        }
        return result;
    }

    public GameObject GetBlockObjectForName(string nameArg)
    {
        return dictionary[nameArg];
    }

    void InitializeDictionary()
    {
        foreach (GameObject blockObjectTemp in blockConfig)
        {
            dictionary.Add(blockObjectTemp.name, blockObjectTemp.gameObject);
        }
    }

    void InitializeSprites()
    {
        blockImages = new Dictionary<GameObject, Sprite>();
        foreach (GameObject blockObjectTemp in blockConfig)
        {
            string blockPrefabName = blockObjectTemp.name;
            Sprite blockSprite = Resources.Load<Sprite>("Blocks/" + blockPrefabName);
            blockImages.Add(blockObjectTemp, blockSprite);
        }
    }

}
