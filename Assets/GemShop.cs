using System.Collections;
using System.Collections.Generic;
using UnityEngine;
class CardSoldEvent : IEvent { Card c; public CardSoldEvent(Card cArg) { c = cArg; } }
public class GemShop : MonoBehaviour
{
    public List<Card> cards;

    public void Sell(Card cardArg)
    {
        cards.Remove(cardArg);
        EventManager.SendEvent(new CardSoldEvent(cardArg));
    }
}
