using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CardMovedToInventoryEvent : IEvent { Card c; public CardMovedToInventoryEvent(Card cArg) { c = cArg; } }
public class DeckAwakeEvent : IEvent { public List<Card> cards; public DeckAwakeEvent(List<Card> cArg) { cards = cArg; } }
public class Deck : MonoBehaviour
{
    public List<Card> cards;

    private void Start()
    {
        DeckAwakeEvent awakeEvent = new DeckAwakeEvent(cards);
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
