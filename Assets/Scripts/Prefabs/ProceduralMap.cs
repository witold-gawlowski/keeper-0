using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralMap : MonoBehaviour
{
    public System.Action finishedGeneratingMapEvent;
    public GameObject square;
    public GameObject levelSpriteObject;
    public int[,] map;
    int[,] tempMap;

    int width;
    int height;
    int steps;
    float initialDensity;
    int dieLimit;
    int spawnLimit;

    public void Initialize(LevelTypeScriptableObjectScript levelParamsArg)
    {
        dieLimit = levelParamsArg.deathLimit;
        spawnLimit = levelParamsArg.lifeLimit;
        initialDensity = levelParamsArg.initialDensity;
        steps = levelParamsArg.steps;
        width = levelParamsArg.width;
        height = levelParamsArg.height;
        Generate();
        levelSpriteObject.transform.localPosition = GetLevelCenterPosition();
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

    public void Generate()
    {
        map = new int[width, height];
        tempMap = new int[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                map[i, j] = Random.value > initialDensity ? 0 : 1;
                tempMap[i, j] = 0;
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
                            if (q == 0 && k == 0)
                            {
                                break;
                            }
                            if (map[i + k, j + q] == 0)
                            {
                                deadCells++;
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

    

    public void Block(List<Vector2Int> blockGeometry, Vector2Int blockPosition)
    {
        int[] dx = {-1, 1, 0, 0 };
        int[] dy = { 0, 0, -1, 1 };
        for (int i = 0; i < blockGeometry.Count; i++)
        {
            Vector2Int tilePosition = blockGeometry[i] + blockPosition;
            int tx = tilePosition.x;
            int ty = tilePosition.y;
            map[tx, ty] = 1;
            for(int j=0; j<4; j++)
            {
                if(map[tx+dx[j], ty + dy[j]] == 0)
                {
                    map[tx + dx[j], ty + dy[j]] = 2;
                }
            }
        }
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
            else if (map[tilePosition.x, tilePosition.y] == 1)
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
