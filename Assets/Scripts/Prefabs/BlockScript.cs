using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockScript : MonoBehaviour
{
    public int blockCathegory = 0;
    public List<Vector2Int> relativeTilePositions;
    public GameObject tile;
    public int value;
    public Color placedColor2 = new Color(0.6f, 0.2f, 0.2f, 1.0f);
    public Color hangingColor = new Color(0.8f, 0.2f, 0.2f, 1.0f);
    public Vector3 geometricMiddlePosition;
    public bool isStarter = false;
    public bool isDigger = false;

    void Start()
    {
        if (relativeTilePositions[0] != Vector2.zero)
        {
            //throw new System.Exception();
        }
        foreach(Vector2 v2 in relativeTilePositions)
        {
            Instantiate(tile, new Vector3(v2.x, v2.y, 0) + transform.position, Quaternion.identity, transform);
        }
        geometricMiddlePosition = GetGeometricMiddle();
    }

    public Vector2Int GetBoxDiameters()
    {
        int maxX=-100, maxY=-100, minX=100, minY=100;
        int tx, ty;
        for (int i = 0; i < relativeTilePositions.Count; i++)
        {
            tx = relativeTilePositions[i].x;
            ty = relativeTilePositions[i].y;

            if (maxX < tx)
            {
                maxX = tx;
            }
            if (maxY < ty)
            {
                maxY = ty;
            }
            if(minX > tx)
            {
                minX = tx;
            }
            if(minY > ty)
            {
                minY = ty;
            }
        }
        return new Vector2Int(maxX-minX, maxY-minY);
    }

    public void ChangeColorToSnappedColor()
    {
        foreach(SpriteRenderer sr  in GetComponentsInChildren<SpriteRenderer>())
        {
            sr.color = placedColor2;
        }
    }

    public void ChangeColorToHangingColor()
    {
        foreach (SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>())
        {
            sr.color = hangingColor;
        }
    }

    Vector2 GetGeometricMiddle()
    {
        Vector2 result = Vector2.zero;
        for (int i = 0; i < relativeTilePositions.Count; i++)
        {
            result += relativeTilePositions[i];
        }
        return result/relativeTilePositions.Count;
    }

    public void Rotate()
    {
        for(int i=0; i<relativeTilePositions.Count; i++)
        {
            Vector2Int newPos = relativeTilePositions[i];
            newPos.x = -relativeTilePositions[i].y;
            newPos.y = relativeTilePositions[i].x;
            relativeTilePositions[i] = newPos;
        }
        geometricMiddlePosition = GetGeometricMiddle();
    }

    public int GetArea()
    {
        return relativeTilePositions.Count;
    }

    public void UpdateLooks()
    {
        for(int i =0;  i<relativeTilePositions.Count; i++)
        {
            Transform tileTransform = transform.GetChild(i);
            tileTransform.localPosition = new Vector3(relativeTilePositions[i].x, relativeTilePositions[i].y, 0);
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        foreach(Vector2Int v2i in relativeTilePositions)
        {
            Gizmos.DrawCube(new Vector3(v2i.x, v2i.y) + transform.position, Vector3.one);
        }
    }
}
