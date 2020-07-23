using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicColorScript : MonoBehaviour
{
    public List<Vector2> positions;
    public Color fromColor;
    public Color toColor;
    public SpriteRenderer[] childTiles;
    public float dist;
    public float  gradientSlope = 10;
    public float gradientZero = 3;
    public GameObject block;

    public void Init()
    {
        positions = new List<Vector2>();
    }

    public void SetBlock(GameObject blockArg)
    {
        block = blockArg;
        block.GetComponent<BlockScript>().dynamicColorScript = this;

        //childTiles = block.GetComponentsInChildren<SpriteRenderer>();
        //Debug.Log(block.name + " child count " + block.transform.childCount);
    }

    void Update()
    {
        dist = 1000;
        foreach (Vector3 v3 in positions)
        {
            dist = Mathf.Min(dist, (block.transform.position - v3).magnitude);
        }
        Color newCOlor = Color.Lerp(toColor, fromColor, (dist-gradientZero) / gradientSlope);
        foreach (SpriteRenderer sr in childTiles)
        {
            sr.color = newCOlor;
        }
    }

    public void RegisterTiles(List<Vector2Int> relativeTilePositionsArg, Vector2Int blockAbsolutePositionArg)
    {
        foreach(Vector2Int v2  in relativeTilePositionsArg)
        {
            positions.Add(v2 + blockAbsolutePositionArg);
        }
    }
}
