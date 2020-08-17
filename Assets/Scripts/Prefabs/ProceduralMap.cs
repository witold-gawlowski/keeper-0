using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Map : MonoBehaviour
{
    public GameObject levelSpriteObject;
    public GameObject levelBackgroundSpriteObject;
    public int[,] map;
    int[,] tempMap;
    int[,] components;
    Dictionary<int, int> componentSizes;

    int width;
    int height;
    public int seed;

    public void Initialize(Randomizer rArg, LevelTypeScriptableObjectScript levelParamsArg, int currentRoundArg, int totalRoundCount, bool alreadyCompltedArg)
    {
        currentRoundNumber = currentRoundArg;
        maxRoundNumber = totalRoundCount;
        dieLimit = levelParamsArg.deathLimit;
        spawnLimit = levelParamsArg.lifeLimit;
        initialDensity = levelParamsArg.initialDensity;
        steps = levelParamsArg.steps;
        width = levelParamsArg.width;
        alreadyCompleted = alreadyCompltedArg;
        height = levelParamsArg.height;
        levelSpriteObject.transform.localPosition = GetLevelCenterPosition();
        levelBackgroundSpriteObject.transform.localPosition = GetLevelCenterPosition();
        int maxCompNum = -1;
        do {
            Generate(rArg);
            GetComponents();
            GetComponentSizes();
            if (levelParamsArg.removeSecondaryCaves)
            {
                RemoveSecondaryComponents();
            }
            maxCompNum = GetMaxComponentNumber();
        } while (levelParamsArg.minimalMaxCaveSize > componentSizes[maxCompNum]);

        if (!alreadyCompleted)
        {
            CreateGems(rArg);
        }
    }

    public Vector3 GetLevelCenterPosition()
    {
        return new Vector3(width / 2.0f - 0.5f, height / 2.0f - 0.5f, 0);
    }

    public int GetWidth()
    {
        return width;
    }

    public int GetHeight()
    {
        return height;
    }

    public int GetFreeArea()
    {
        int result = 0;
        for(int i=0; i<width; i++)
        {
            for(int j=0; j<height; j++)
            {
                if(map[i, j] == 0)
                {
                    result++;
                }
            }
        }
        return result;
    }

    public void SaveToScriptableObject()
    {
        MapScriptableObject mapSO = ScriptableObject.CreateInstance<MapScriptableObject>();
        mapSO.height = height;
        mapSO.width = width;

        string description = "";
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    description += map[i, j].ToString() + ", ";
                }
            }
            description = description.TrimEnd(',');
        }

        mapSO.mapDescription = description;

        AssetDatabase.CreateAsset(mapSO, "Assets/Resources/Maps/NewMap.asset");
        AssetDatabase.SaveAssets();

    }

   
    public void ClearBlocks()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if(map[i, j] == 3)
                {
                    map[i, j] = 0;
                }
            }
        }
    }

    public void Block(List<Vector2Int> blockGeometry, Vector2Int blockPosition)
    {
        int[] dx = {-1, 1, 0, 0 };
        int[] dy = { 0, 0, -1, 1 };
        for (int i = 0; i < blockGeometry.Count; i++)
        {
            Vector2Int tilePosition = blockGeometry[i] + blockPosition;
            int tx = tilePosition.x;
            int ty = tilePosition.y;
            map[tx, ty] = 3;
            for(int j=0; j<4; j++)
            {
                if(map[tx+dx[j], ty + dy[j]] == 0)
                {
                    map[tx + dx[j], ty + dy[j]] = 2;
                }
                if (map[tx + dx[j], ty + dy[j]] == 4)
                {
                    map[tx + dx[j], ty + dy[j]] = 5;
                }
            }
        }
    }

    void PrintMe()
    {
        for (int i = 0; i < width; i++)
        {
            string s = "";
            for (int j = 0; j < height; j++)
            {
                s += map[i, j];
            }
            print(s + "/n");
        }
    }
    //make sure it  works before using
    public int OverlapCount(List<Vector2Int> blockGeometry, Vector2Int blockPosition)
    {
        int result = 0;
        for (int i = 0; i < blockGeometry.Count; i++)
        {
            Vector2Int tilePosition = blockGeometry[i] + blockPosition;
            if (map[tilePosition.x, tilePosition.y] == 1)
            {
                result++;
            }
        }
        return result;
    }

    public int CountGems(List<Vector2Int> blockGeometry, Vector2Int blockPosition)
    {
        int result = 0;
        for (int i = 0; i < blockGeometry.Count; i++)
        {
            Vector2Int tilePosition = blockGeometry[i] + blockPosition;
            if (map[tilePosition.x, tilePosition.y] == 4 || map[tilePosition.x, tilePosition.y] == 5)
            {
                result++;
            }
        }
        return result;
    }


    public bool AreFree(List<Vector2Int> blockGeometry, Vector2Int blockPosition, bool alsoCheckForNeighbor = false, bool blockIsdigger = false)
    {
        bool hasNeighbor = false;
        for(int i=0; i<blockGeometry.Count; i++)
        {
            Vector2Int tilePosition = blockGeometry[i] + blockPosition;
            if(tilePosition.x<0 || tilePosition.x>=width || tilePosition.y<0 || tilePosition.y >= height)
            {
                return false;
            }
            else if ((map[tilePosition.x, tilePosition.y] == 1 && blockIsdigger == false)|| map[tilePosition.x, tilePosition.y] == 3)
            {
                return false;
            }
            if(!hasNeighbor && (map[tilePosition.x, tilePosition.y] == 2 || map[tilePosition.x, tilePosition.y] == 5))
            {
                hasNeighbor = true;
            }
        }
        if (alsoCheckForNeighbor)
        {
            if (hasNeighbor)
            {
                return true;
            }
            return false;
        }
        return true;
    }

    
}
