using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCodexScript : MonoBehaviour
{
    public static BlockCodexScript instance;

    public List<GameObject> _blockConfig;
    private Dictionary<GameObject, Sprite> _blockImages;
    private Dictionary<string, GameObject> _dictionary;

    public string resourcesBlockFolderName = "Blocks";


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

        LoadBlocks();
        InitializeSprites();
        InitializeDictionary();
    }

    void LoadBlocks()
    {
        _blockConfig = new List<GameObject>(Resources.LoadAll<GameObject>(resourcesBlockFolderName));
    }

    public GameObject GetRandomBlock(Randomizer rArg)
    {
        int numberOfBlocks = _blockConfig.Count;
        int randomIndex = rArg.Range(0, numberOfBlocks);
        return _blockConfig[randomIndex];
    }

    public Sprite GetSpriteForPrefab(GameObject blockPrefab)
    {
        if (!_blockImages.ContainsKey(blockPrefab))
        {
            print("missing  image! " + blockPrefab.name);
        }
        return _blockImages[blockPrefab];
    }

    public List<GameObject> GetBlocks()
    {
        return _blockConfig;
    }

    public List<GameObject> GetBlockTypes()
    {
        var result = new List<GameObject>();
        foreach (GameObject blockObjectTemp in _blockConfig)
        {
            result.Add(blockObjectTemp);
        }
        return result;
    }

    public GameObject GetBlockObjectForName(string nameArg)
    {
        return _dictionary[nameArg];
    }

    void InitializeDictionary()
    {
        _dictionary = new Dictionary<string, GameObject>();
        foreach (GameObject blockObjectTemp in _blockConfig)
        {
            if (_dictionary.ContainsKey(blockObjectTemp.name))
            {
                Debug.Log("doubled blocks for " + blockObjectTemp + "!");
            }
            _dictionary.Add(blockObjectTemp.name, blockObjectTemp.gameObject);
        }
    }

    void InitializeSprites()
    {
        _blockImages = new Dictionary<GameObject, Sprite>();
        foreach (GameObject blockObjectTemp in _blockConfig)
        {
            string blockPrefabName = blockObjectTemp.name;
            Sprite blockSprite = Resources.Load<Sprite>("BlockSprites/" + blockPrefabName);
            if(blockSprite == null)
            {   
                Debug.Log("couldnt load sprite for " + blockPrefabName);
            }
            _blockImages.Add(blockObjectTemp, blockSprite);
        }
    }

}
