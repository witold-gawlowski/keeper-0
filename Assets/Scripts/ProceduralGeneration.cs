using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralMap : Map
{
    private int _currentDFSComponent;
    private Dictionary<int, int> _componentSizes;
    private int[,] _tempMap;
    private int[,] _components;

    private int _seed;
    private float _gem_density;
    private MapParams _mParams;

    public void Init(MapParams mParams, float gem_density)
    {
        _mParams = mParams;
        _gem_density = gem_density;
    }

    public void Generate()
    {
        _width = _mParams.width;
        _height = _mParams.height;
        int maxCompNum = -1;
        do
        {
            GameOfLife();
            GetComponents();
            GetComponentSizes();
            if (_mParams.removeSecondaryCaves)
            {
                RemoveSecondaryComponents();
            }
            maxCompNum = GetMaxComponentNumber();
        } while (_mParams.minimalMaxCaveSize > _componentSizes[maxCompNum]);
        AddGems();
    }

    private void GetComponents()
    {
        _components = new int[_width, _height];
        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                _components[i, j] = -1;
            }
        }
        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                if (_components[i, j] == -1)
                {
                    DFS(i, j);
                    _currentDFSComponent++;
                }
            }
        }

    }

    private void GetComponentSizes()
    {
        _componentSizes = new Dictionary<int, int>();
        _componentSizes.Add(-1, 0);
        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                int component = _components[i, j];
                if (component != -1)
                {
                    if (_componentSizes.ContainsKey(component))
                    {
                        _componentSizes[component]++;
                    }
                    else
                    {
                        _componentSizes.Add(component, 1);
                    }
                }
            }
        }
    }

    private  int GetMaxComponentNumber()
    {

        int maxComponentNumber = -1;
        foreach (KeyValuePair<int, int> pair in _componentSizes)
        {
            if (pair.Value > _componentSizes[maxComponentNumber])
            {
                maxComponentNumber = pair.Key;
            }
        }
        return maxComponentNumber;
    }

    private void RemoveSecondaryComponents()
    {
        int maxComponentNumber = GetMaxComponentNumber();
        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                if (_map[i, j] == 0)
                {
                    if (_components[i, j] != maxComponentNumber)
                    {
                        _map[i, j] = 1;
                    }
                }
            }
        }
    }

    private void AddGems()
    {
        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                if (_map[i, j] == 0)
                {
                    _map[i, j] = Random.Range(0.0f, 1.0f) < _gem_density ? 0 : 4;
                }
            }
        }
    }

    public void GameOfLife()
    {
        _map = new int[_width, _height];
        _tempMap = new int[_width, _height];
        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                _tempMap[i, j] = 0;
                _map[i, j] = Random.Range(0.0f, 1.0f) > _mParams.initialDensity ? 0 : 1;
            }
        }
        for (int i = 0; i < _width; i++)
        {
            _map[i, 0] = 1;
            _map[i, _height - 1] = 1;
        }
        for (int i = 0; i < _height; i++)
        {
            _map[0, i] = 1;
            _map[_width - 1, i] = 1;
        }

        for (int w = 0; w < _mParams.steps; w++)
        {
            for (int i = 1; i < _width - 1; i++)
            {
                for (int j = 1; j < _height - 1; j++)
                {
                    int deadCells = 0;
                    for (int k = -1; k <= 1; k++)
                    {
                        for (int q = -1; q <= 1; q++)
                        {
                            if (_map[i + k, j + q] == 0)
                            {
                                deadCells++;
                            }
                        }
                    }
                    if (_map[i, j] == 0)
                    {
                        _tempMap[i, j] = 0;
                        if (deadCells < _mParams.steps)
                        {
                            _tempMap[i, j] = 1;
                        }
                    }
                    else
                    {
                        _tempMap[i, j] = 1;
                        if (deadCells > _mParams.steps)
                        {
                            _tempMap[i, j] = 0;
                        }
                    }
                }
            }
            for (int i = 1; i < _width - 1; i++)
            {
                for (int j = 1; j < _height - 1; j++)
                {
                    _map[i, j] = _tempMap[i, j];
                }
            }
        }


    }

    private void DFS(int i, int j)
    {
        if (_map[i, j] == 1)
        {
            return;
        }
        _components[i, j] = _currentDFSComponent;
        int[] dx = { -1, 1, 0, 0 };
        int[] dy = { 0, 0, 1, -1 };
        for (int l = 0; l < 4; l++)
        {
            int newi = i + dx[l];
            int newj = j + dy[l];
            if (newi < 0 || newj < 0 || newi == _width || newj == _height)
            {
                continue;
            }

            if (_components[newi, newj] == -1)
            {
                DFS(newi, newj);
            }
        }
    }


}
