using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralMap : MonoBehaviour
{
    public System.Action finishedGeneratingMapEvent;
    public GameObject square;
    public GameObject levelSpriteObject;
    public int[,] map;
    public ProceduralMap boundaryMap;
    public ProceduralMap contentsMap;
    int[,] tempMap;
    int[,] components;
    Dictionary<int, int> componentSizes;

    int width;
    int height;
    int steps;
    float initialDensity;
    int dieLimit;
    int spawnLimit;

    int currentDFSComponent;

    public void Initialize(Randomizer rArg, LevelTypeScriptableObjectScript levelParamsArg)
    {
        dieLimit = levelParamsArg.deathLimit;
        spawnLimit = levelParamsArg.lifeLimit;
        initialDensity = levelParamsArg.initialDensity;
        steps = levelParamsArg.steps;
        width = levelParamsArg.width;
        height = levelParamsArg.height;
        levelSpriteObject.transform.localPosition = GetLevelCenterPosition();

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

        finishedGeneratingMapEvent();
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

    public void GetComponents()
    {
        components = new int[width, height];
        for(int i=0; i< width; i++)
        {
            for(int j=0; j<height; j++)
            {
                components[i, j] = -1;
            }
        }
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (components[i, j] == -1)
                {
                    dfs(i, j);
                    currentDFSComponent++;
                }
            }
        }

    }

    void GetComponentSizes()
    {
        componentSizes = new Dictionary<int, int>();
        componentSizes.Add(-1, 0);
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                int component = components[i, j];
                if(component != -1)
                {
                    if(componentSizes.ContainsKey(component))
                    {
                        componentSizes[component]++;
                    }
                    else
                    {
                        componentSizes.Add(component, 1);
                    }
                }
            }
        }
    }

    int GetMaxComponentNumber()
    {
        
        int maxComponentNumber = -1;
        foreach(KeyValuePair<int, int>  pair in componentSizes)
        {
            if(pair.Value > componentSizes[maxComponentNumber])
            {
                maxComponentNumber = pair.Key;
            }
        }
        return maxComponentNumber;
    }

    void RemoveSecondaryComponents()
    {
        int maxComponentNumber = GetMaxComponentNumber();
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if(map[i, j] == 0)
                {
                    if(components[i, j] != maxComponentNumber)
                    {
                        map[i, j] = 1;
                    }
                }
            }
        }
    }

    void dfs(int i, int j)
    {
        if(map[i, j] == 1)
        {
            return;
        }
        components[i, j] = currentDFSComponent;
        int[] dx = { -1, 1, 0, 0 };
        int[] dy = { 0, 0, 1, -1 };
        for(int l = 0; l<4; l++)
        {
            int newi = i + dx[l];
            int newj = j + dy[l];
            if(newi<0 || newj<0  || newi == width || newj == height)
            {
                continue;
            }

            if (components[newi, newj]  == -1)
            {
                dfs(newi, newj);
            }
        }
    }


    public void Generate(Randomizer rArg)
    {
        map = new int[width, height];
        tempMap = new int[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                tempMap[i, j] = 0;
                map[i, j] = rArg.Range(0.0f, 1.0f) > initialDensity ? 0 : 1;
            }
        }
        for (int i = 0; i < width; i++)
        {
            map[i, 0] = 1;
            map[i, height - 1] = 1;
        }
        for (int i = 0; i < height; i++)
        {
            map[0, i] = 1;
            map[width - 1, i] = 1;
        }

        for (int w = 0; w < steps; w++)
        {
            for (int i = 1; i < width - 1; i++)
            {
                for (int j = 1; j < height - 1; j++)
                {
                    int deadCells = 0;
                    for (int k = -1; k <= 1; k++)
                    {
                        for (int q = -1; q <= 1; q++)
                        {
                            //if (q == 0 && k == 0)
                            //{
                            //    break;
                            //}
                            if (map[i + k, j + q] == 0)
                            {
                                if (boundaryMap == null || boundaryMap.map[i + k, j + q] == 0)
                                {
                                    deadCells++;
                                }
                            }
                        }
                    }
                    if (map[i, j] == 0)
                    {
                        tempMap[i, j] = 0;
                        if (deadCells < spawnLimit)
                        {
                            tempMap[i, j] = 1;
                        }
                    }
                    else
                    {
                        tempMap[i, j] = 1;
                        if (deadCells > dieLimit)
                        {
                            tempMap[i, j] = 0;
                        }
                    }
                }
            }
            for (int i = 1; i < width - 1; i++)
            {
                for (int j = 1; j < height - 1; j++)
                {
                    map[i, j] = tempMap[i, j];
                }
            }
        }

  
    }

    public void ClearBlocks()
    {
        print("cleaerblocks!");
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
            }
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


    public bool AreFree(List<Vector2Int> blockGeometry, Vector2Int blockPosition, bool alsoCheckForNeighbor = false)
    {
        bool hasNeighbor = false;
        for(int i=0; i<blockGeometry.Count; i++)
        {
            Vector2Int tilePosition = blockGeometry[i] + blockPosition;
            if(tilePosition.x<0 || tilePosition.x>=width || tilePosition.y<0 || tilePosition.y >= height)
            {
                return false;
            }
            else if (map[tilePosition.x, tilePosition.y] == 1 || map[tilePosition.x, tilePosition.y] == 3)
            {
                return false;
            }
            if(!hasNeighbor && map[tilePosition.x, tilePosition.y] == 2)
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
