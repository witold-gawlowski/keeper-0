using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GridLayoutSetup : MonoBehaviour
{
    GridLayoutGroup gridLayout;
    // Start is called before the first frame update
    IEnumerator Setup()
    {
        yield return new WaitForEndOfFrame();
        float buttonDiameter = GlobalUIScript.instance.GetButtonDiameter();
        gridLayout.cellSize = new Vector2(buttonDiameter, buttonDiameter);
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
