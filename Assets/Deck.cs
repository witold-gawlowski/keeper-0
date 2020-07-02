using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CardMovedToInventoryEvent : IEvent {public Card c; public CardMovedToInventoryEvent(Card cArg) { c = cArg; } }
public class UpdateDeckUIEvent : IEvent { public List<Card> cards; public UpdateDeckUIEvent(List<Card> cArg) { cards = cArg; } }
public class Deck : MonoBehaviour
{
    public List<Card> cards;

    public List<Card> queue;

    private void Awake()
    {
        EventManager.AddListener<CardMovedToDeckEvent>(CardMovedToDeckEventDispatcher);
        EventManager.AddListener<CardMovedToInventoryEvent>(CardMovedToInventoryEventDispatcher);
    }

    private void Start()
    {
        UpdateDeckUIEvent awakeEvent = new UpdateDeckUIEvent(cards);
        EventManager.SendEvent(awakeEvent);
    }

    public void CardMovedToDeckEventDispatcher(IEvent evArg)
    {
        CardMovedToDeckEvent evData = evArg as CardMovedToDeckEvent;
        Add(evData.c);
    }

    public void CardMovedToInventoryEventDispatcher(IEvent evArg)
    {
        CardMovedToInventoryEvent evData = evArg as CardMovedToInventoryEvent;
        Remove(evData.c);
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
        EventManager.SendEvent(new UpdateDeckUIEvent(cards));
    }
    void Remove(Card cardArg)
    {
        cards.Remove(cardArg);
        EventManager.SendEvent(new UpdateDeckUIEvent(cards));
    }


}
