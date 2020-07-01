using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CardMovedToInventoryEvent : IEvent { Card c; public CardMovedToInventoryEvent(Card cArg) { c = cArg; } }
public class DeckAwakeEvent : IEvent { public List<Card> cards; public DeckAwakeEvent(List<Card> cArg) { cards = cArg; } }
public class Deck : MonoBehaviour
{
    public List<Card> cards;

    public List<Card> queue;

    private void Start()
    {
        DeckAwakeEvent awakeEvent = new DeckAwakeEvent(cards);
        EventManager.SendEvent(awakeEvent);
    }

    public void Shuffle()
    {
        queue = new List<Card>(cards);
        queue.Shuffle();
    }

    public bool IsDeckEmpty()
    {
        return queue.Count == 0;
    }

    public Card Draw()
    {
        Card result = queue[0];
        queue.RemoveAt(0);
        return result;
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
