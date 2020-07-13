using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CardMovedToDeckEvent : IEvent {public Card c; public CardMovedToDeckEvent(Card cArg) { c = cArg; } }
public class UpdateInventoryUIEvent : IEvent { public List<Card> cards; public UpdateInventoryUIEvent(List<Card> cArg) { cards = cArg; } }
public class Inventory : MonoBehaviour
{
    List<Card> cards;
    public List<Card> startingCards;

    private void Awake()
    {
        Load();
        //EventManager.AddListener<ShopCardTappedEvent>(NewCardEventDispatcher);
        EventManager.AddListener<CardMovedToDeckEvent>(CardMovedToDeckEventDispatcher);
        EventManager.AddListener<CardMovedToInventoryEvent>(CardMovedToInventoryEventDispatcher);
    }

    public void Load()
    {
        if (PlayerPrefs.HasKey("inventory"))
        {
            string inventoryString = PlayerPrefs.GetString("inventory");
            FromString(inventoryString);
        }
        else
        {
            cards = startingCards;
        }
    }

    public void Save()
    {
        PlayerPrefs.SetString("inventory", ToString());
        PlayerPrefs.Save();
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

    //void NewCardEventDispatcher(IEvent evArg)
    //{
    //    ShopCardTappedEvent ev = evArg as ShopCardTappedEvent;
    //    Add(ev.card);
    //}

    private void Start()
    {
        UpdateInventoryUIEvent awakeEvent = new UpdateInventoryUIEvent(cards);
        EventManager.SendEvent(awakeEvent);
    }

    public void FromString(string sourceArg)
    {
        if (sourceArg != "")
        {
            cards = new List<Card>();
            string[] words = sourceArg.Split(';');
            foreach (string s in words)
            {
                Card newCard = CardCodex.instance.GetCardByID(s);
                cards.Add(newCard);
            }
        }
    }

    public override string ToString()
    {
        string result = "";
        foreach(Card c in cards)
        {
            result += c.id + ";";
        }
        result = result.TrimEnd(new char[] { ';' });
        return result;
    }
    
    public void Add(Card cardArg)
    {
        cards.Add(cardArg);
        EventManager.SendEvent(new UpdateInventoryUIEvent(cards));
        Save();
    }

    void Remove(Card cardArg)
    {
        cards.Remove(cardArg);
        EventManager.SendEvent(new UpdateInventoryUIEvent(cards));
        Save();
    }
}
