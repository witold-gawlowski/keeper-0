using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CardSoldEvent : IEvent { public Card card; public CardSoldEvent(Card cArg) { card = cArg; } }
public class UpdateGemShopUIEvent : IEvent { public List<Card> cards; public UpdateGemShopUIEvent(List<Card> cArg) { cards = cArg; } }
public class GemShop : MonoBehaviour
{
    public List<Card> cards;

    void Awake()
    {
        EventManager.AddListener<CardSoldEvent>(CardSoldeventHandler);
    }
    
    void CardSoldeventHandler(IEvent evArg)
    {
        CardSoldEvent ev = evArg as CardSoldEvent;
        Sell(ev.card);
    }

    void Start()
    {
        UpdateGemShopUIEvent awakeEvent = new UpdateGemShopUIEvent(cards);
        EventManager.SendEvent(awakeEvent);
    }

    private void Sell(Card cardArg)
    {
        cards.Remove(cardArg);
        EventManager.SendEvent(new UpdateGemShopUIEvent(cards));

    }
}
