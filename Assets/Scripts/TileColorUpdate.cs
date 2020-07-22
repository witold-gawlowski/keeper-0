using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileColorUpdate : MonoBehaviour
{
    static List<Vector3> positions;
    public Color fromColor;
    public Color toColor;
    public SpriteRenderer sr;
    Transform parentTransform;
    float dist;
    static int universalIndex;
    static int positionsCount;
    private void Awake()
    {
        parentTransform = GetComponentInParent<Transform>();
        universalIndex = 0;
        positionsCount = positions.Count;
    }

    void Update()
    {
        dist = (parentTransform.position - positions[(universalIndex++)%positionsCount]).magnitude;
        sr.color = Color.Lerp(toColor, fromColor, dist / 30);
    }
}
