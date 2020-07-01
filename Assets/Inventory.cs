using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CardMovedToDeckEvent : IEvent { Card c; public CardMovedToDeckEvent(Card cArg) { c = cArg; } }
public class InventoryAwakeEvent : IEvent { public List<Card> cards; public InventoryAwakeEvent(List<Card> cArg) { cards = cArg; } }
public class Inventory : MonoBehaviour
{
    public List<Card> cards;

    private void Awake()
    {

    }

    private void Start()
    {
        InventoryAwakeEvent awakeEvent = new InventoryAwakeEvent(cards);
        EventManager.SendEvent(awakeEvent);
    }


    void Add(Card cardArg)
    {
        cards.Add(cardArg);
    }
    void Remove(Card cardArg)
    {
        cards.Remove(cardArg);
    }
}
