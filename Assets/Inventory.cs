using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CardMovedToDeckEvent : IEvent {public Card c; public CardMovedToDeckEvent(Card cArg) { c = cArg; } }
public class UpdateInventoryUIEvent : IEvent { public List<Card> cards; public UpdateInventoryUIEvent(List<Card> cArg) { cards = cArg; } }
public class Inventory : MonoBehaviour
{
    public List<Card> cards;

    private void Awake()
    {
        EventManager.AddListener<ShopCardTappedEvent>(NewCardEventDispatcher);
        EventManager.AddListener<CardMovedToDeckEvent>(CardMovedToDeckEventDispatcher);
        EventManager.AddListener<CardMovedToInventoryEvent>(CardMovedToInventoryEventDispatcher);
    }

    public void CardMovedToDeckEventDispatcher(IEvent evArg)
    {
        CardMovedToDeckEvent evData = evArg as CardMovedToDeckEvent;
        Remove(evData.c);
    }

    public void CardMovedToInventoryEventDispatcher(IEvent evArg)
    {
        CardMovedToInventoryEvent evData = evArg as CardMovedToInventoryEvent;
        Add(evData.c);
    }

    void NewCardEventDispatcher(IEvent evArg)
    {
        ShopCardTappedEvent ev = evArg as ShopCardTappedEvent;
        Add(ev.card);
    }

    private void Start()
    {
        UpdateInventoryUIEvent awakeEvent = new UpdateInventoryUIEvent(cards);
        EventManager.SendEvent(awakeEvent);
    }

    public void Load(string sourceArg)
    {

    }

    public override string ToString()
    {
        return "";
    }


    void Add(Card cardArg)
    {
        cards.Add(cardArg);
        EventManager.SendEvent(new UpdateInventoryUIEvent(cards));
    }

    void Remove(Card cardArg)
    {
        cards.Remove(cardArg);
        EventManager.SendEvent(new UpdateInventoryUIEvent(cards));
    }
}
