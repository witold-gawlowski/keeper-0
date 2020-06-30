using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
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
