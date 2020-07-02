using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CardMovedToInventoryEvent : IEvent {public Card c; public CardMovedToInventoryEvent(Card cArg) { c = cArg; } }
public class UpdateDeckUIEvent : IEvent { public List<Card> cards; public UpdateDeckUIEvent(List<Card> cArg) { cards = cArg; } }
public class DeckManager : MonoBehaviour
{
    static public DeckManager instance;
    Deck deck;

    private void Awake()
    {
        deck = FindObjectOfType<Deck>();    
        EventManager.AddListener<CardMovedToDeckEvent>(CardMovedToDeckEventDispatcher);
        EventManager.AddListener<CardMovedToInventoryEvent>(CardMovedToInventoryEventDispatcher);
    }

    private void Start()
    {
        UpdateDeckUIEvent awakeEvent = new UpdateDeckUIEvent(deck.cards);
        EventManager.SendEvent(awakeEvent);
    }

    public void CardMovedToDeckEventDispatcher(IEvent evArg)
    {
        CardMovedToDeckEvent evData = evArg as CardMovedToDeckEvent;
        deck.Add(evData.c);
    }

    public void CardMovedToInventoryEventDispatcher(IEvent evArg)
    {
        CardMovedToInventoryEvent evData = evArg as CardMovedToInventoryEvent;
        deck.Remove(evData.c);
    }

    


}
