using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockScript : MonoBehaviour
{
    public List<Vector2Int> relativeTilePositions;
    public GameObject tile;
    public int value;
    // Start is called before the first frame update
    void Start()
    {
        if (relativeTilePositions[0] != Vector2.zero)
        {
            throw new System.Exception();
        }
        foreach(Vector2 v2 in relativeTilePositions)
        {
            Instantiate(tile, new Vector3(v2.x, v2.y, 0) + transform.position, Quaternion.identity, transform);
        }
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
        UpdateLooks();
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
