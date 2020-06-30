using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CardSoldEvent : IEvent { Card c; public CardSoldEvent(Card cArg) { c = cArg; } }
public class GemShopAwakeEvent : IEvent { public List<Card> cards; public GemShopAwakeEvent(List<Card> cArg) { cards = cArg; } }
public class GemShop : MonoBehaviour
{

    public List<Card> cards;

    private void Awake()
    {
        
    }


    private void Start()
    {
        GemShopAwakeEvent awakeEvent = new GemShopAwakeEvent(cards);
        EventManager.SendEvent(awakeEvent);
    }

    public void Sell(Card cardArg)
    {
        cards.Remove(cardArg);
        EventManager.SendEvent(new CardSoldEvent(cardArg));
    }
}
