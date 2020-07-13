using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
[CreateAssetMenu(fileName = "Card", menuName = "ScriptableObjects/Card", order = 1)]
public class Card: ScriptableObject
{
    public string id {
        get { return name;}
    }
    public int gemCost;
    public GameObject block;
    public int cashCost;
    public int quantity;

    public Card(GameObject blockArg, int quantityArg, int cashCostArg, int gemCostArg)
    {
        block = blockArg;
        quantity = quantityArg;
        cashCost = cashCostArg;
        gemCost = gemCostArg;
    }

    public Card(string stringArg)
    {
        string[] words = stringArg.Split(',');
        block = BlockCodexScript.instance.GetBlockObjectForName(words[0]);
        quantity = int.Parse(words[1]);
        cashCost = int.Parse(words[2]);
        gemCost = int.Parse(words[3]);
    }

    public override string ToString()
    {
        return block.name + "," + quantity + "," + cashCost + "," + gemCost;
    }
}
