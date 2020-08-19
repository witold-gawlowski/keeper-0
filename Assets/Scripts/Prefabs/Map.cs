using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Map : ScriptableObject
{

    protected int[,] _map;
    protected int _width;
    protected int _height;

    public Vector3 GetCenterPosition()
    {
        return new Vector3(_width / 2.0f - 0.5f, _height / 2.0f - 0.5f, 0);
    }

    public void SetWidth(int widthArg)
    {
        _width = widthArg;
    }

    public void SetHeight(int heightArg)
    {
        _height = heightArg;
    }

    public int GetWidth()
    {
        return _width;
    }

    public int GetHeight()
    {
        return _height;
    }

    public int GetFreeArea()
    {
        int result = 0;
        for(int i=0; i<_width; i++)
        {
            for(int j=0; j<_height; j++)
            {
                if(_map[i, j] == 0)
                {
                    result++;
                }
            }
        }
        return result;
    }
   
    public void ClearBlocks()
    {
        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                if(_map[i, j] == 3)
                {
                    _map[i, j] = 0;
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
            _map[tx, ty] = 3;
            for(int j=0; j<4; j++)
            {
                if(_map[tx+dx[j], ty + dy[j]] == 0)
                {
                    _map[tx + dx[j], ty + dy[j]] = 2;
                }
                if (_map[tx + dx[j], ty + dy[j]] == 4)
                {
                    _map[tx + dx[j], ty + dy[j]] = 5;
                }
            }
        }
    }

    //void PrintMe()
    //{
    //    for (int i = 0; i < width; i++)
    //    {
    //        string s = "";
    //        for (int j = 0; j < height; j++)
    //        {
    //            s += map[i, j];
    //        }
    //        print(s + "/n");
    //    }
    //}
    //make sure it  works before using

    public int OverlapCount(List<Vector2Int> blockGeometry, Vector2Int blockPosition)
    {
        int result = 0;
        for (int i = 0; i < blockGeometry.Count; i++)
        {
            Vector2Int tilePosition = blockGeometry[i] + blockPosition;
            if (_map[tilePosition.x, tilePosition.y] == 1)
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
            if (_map[tilePosition.x, tilePosition.y] == 4 || _map[tilePosition.x, tilePosition.y] == 5)
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
            if(tilePosition.x<0 || tilePosition.x>=_width || tilePosition.y<0 || tilePosition.y >= _height)
            {
                return false;
            }
            else if ((_map[tilePosition.x, tilePosition.y] == 1 && blockIsdigger == false)|| _map[tilePosition.x, tilePosition.y] == 3)
            {
                return false;
            }
            if(!hasNeighbor && (_map[tilePosition.x, tilePosition.y] == 2 || _map[tilePosition.x, tilePosition.y] == 5))
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
