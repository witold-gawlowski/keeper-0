using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ShopCardTappedEvent : IEvent {
    public Card card; public ShopCardTappedEvent(Card cArg) { card = cArg; }
}
public class UpdateGemShopUIEvent : IEvent {
    public List<Card> cards; public int gems; public UpdateGemShopUIEvent(List<Card> cArg, int gemsArg) { cards = cArg; gems = gemsArg; }
}
public class GemShop : MonoBehaviour
{
    public List<Card> cards;
    int numberOfCardsInOffer = 5;
    public int gems = 5;
    public CompletedLevelsManager completedLevelsManager;

    void Awake()
    {
        EventManager.AddListener<ShopCardTappedEvent>(CardSoldEventHandler);
    }
    
    void CardSoldEventHandler(IEvent evArg)
    {
        ShopCardTappedEvent ev = evArg as ShopCardTappedEvent;
        TrySell(ev.card);
    }

    public void LoadGems(int gemsArg)
    {
        gems = gemsArg;
    }

    public void CreateOffer()
    {
        int completedLevelsFootprint = completedLevelsManager.GetCompletedLevelsFootprint();
        Randomizer r = new Randomizer(completedLevelsFootprint);
        for(int i=0; i<numberOfCardsInOffer; i++)
        {
            int quantity = new[] {1, 3, 5 }[r.Range(0, 3)];
            int cashCost = new[] { 100, 200, 300, 500 }[r.Range(0, 4)];
            int gemCost = new[] { 2, 3, 5, 7 }[r.Range(0, 4)];
            GameObject block = BlockCodexScript.instance.GetRandomBlock(r);
            Card newCard = new Card(block, quantity, cashCost, gemCost);
            cards.Add(newCard);
        }
    }

    void Start()
    {
        UpdateGemShopUIEvent awakeEvent = new UpdateGemShopUIEvent(cards, gems);
        EventManager.SendEvent(awakeEvent);
    }

    private void TrySell(Card cardArg)
    {
        if (gems >= cardArg.gemCost)
        {
            gems -= cardArg.gemCost;
            cards.Remove(cardArg);
            EventManager.SendEvent(new UpdateGemShopUIEvent(cards, gems));
        }
    }
}
