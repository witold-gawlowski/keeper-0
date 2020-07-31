using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GridLayoutSetup : MonoBehaviour
{
    GridLayoutGroup gridLayout;
    public float totalButtonScreenFraction = 0.7f;
    public float paddingFraction = 0.08f;
    IEnumerator Setup()
    {
        yield return new WaitForEndOfFrame();
        float buttonDiameter = GetButtonDiameter();
        gridLayout.cellSize = new Vector2(buttonDiameter, buttonDiameter);
        int padding = Mathf.RoundToInt(paddingFraction * Screen.width);
        RectOffset newPadding = new RectOffset(padding, padding, padding, padding);
        gridLayout.padding = newPadding;
        gridLayout.spacing = Vector2.one*((Screen.width - padding*2) * (1 - totalButtonScreenFraction)/3);
    }

    public float GetButtonDiameter()
    {
        return ((Screen.width - paddingFraction*Screen.width * 2) * totalButtonScreenFraction) / 4;
    }

    private void Awake()
    {
        gridLayout = GetComponent<GridLayoutGroup>();
    }

    void Start()
    {
        StartCoroutine(Setup());
    }

}
