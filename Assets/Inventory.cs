using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Card> cards;
    void Add(Card cardArg)
    {
        cards.Add(cardArg);
    }
    void Remove(Card cardArg)
    {
        cards.Remove(cardArg);
    }
}
