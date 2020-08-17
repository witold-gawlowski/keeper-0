using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ProceduralGeneration
{
    static Map map;
    static int steps;
    static float initialDensity;
    static float initialGemDensity = 0.2f;
    static int dieLimit;
    static int spawnLimit;
    static int maxRoundNumber;
    static int currentRoundNumber;
    static bool alreadyCompleted;
    static int currentDFSComponent;
    static Dictionary<int, int> componentSizes;
    static int[,] tempMap;

    public static void Initialize(Randomizer rArg, LevelTypeScriptableObjectScript levelParamsArg, int currentRoundArg, int totalRoundCount, bool alreadyCompltedArg)
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
        do
        {
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
    public static void GetComponents()
    {
        components = new int[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
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

    void static GetComponentSizes()
    {
        componentSizes = new Dictionary<int, int>();
        componentSizes.Add(-1, 0);
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                int component = components[i, j];
                if (component != -1)
                {
                    if (componentSizes.ContainsKey(component))
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

    int static GetMaxComponentNumber()
    {

        int maxComponentNumber = -1;
        foreach (KeyValuePair<int, int> pair in componentSizes)
        {
            if (pair.Value > componentSizes[maxComponentNumber])
            {
                maxComponentNumber = pair.Key;
            }
        }
        return maxComponentNumber;
    }

    void static RemoveSecondaryComponents()
    {
        int maxComponentNumber = GetMaxComponentNumber();
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (map[i, j] == 0)
                {
                    if (components[i, j] != maxComponentNumber)
                    {
                        map[i, j] = 1;
                    }
                }
            }
        }
    }

    void static dfs(int i, int j)
    {
        if (map[i, j] == 1)
        {
            return;
        }
        components[i, j] = currentDFSComponent;
        int[] dx = { -1, 1, 0, 0 };
        int[] dy = { 0, 0, 1, -1 };
        for (int l = 0; l < 4; l++)
        {
            int newi = i + dx[l];
            int newj = j + dy[l];
            if (newi < 0 || newj < 0 || newi == width || newj == height)
            {
                continue;
            }

            if (components[newi, newj] == -1)
            {
                dfs(newi, newj);
            }
        }
    }

    public static void CreateGems(Randomizer rArg)
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (map[i, j] == 0)
                {
                    map[i, j] = rArg.Range(0.0f, 1.0f) > initialGemDensity * currentRoundNumber / maxRoundNumber ? 0 : 4;
                }
            }
        }
    }

    public static void Generate(Randomizer rArg)
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


}
