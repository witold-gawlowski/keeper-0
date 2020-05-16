using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBlockScript : MonoBehaviour
{
    public Text nominalValueText;
    public void UpdateNominalValueText(int value)
    {
        nominalValueText.text = "$" + value.ToString();
    }
}
