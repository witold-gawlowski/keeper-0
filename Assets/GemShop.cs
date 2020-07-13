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
    const int bigPrime = int.MaxValue;
    int numberOfCardsInOffer = 5;
    public int gems = 5;
    public CompletedLevelsManager completedLevelsManager;

    void Awake()
    {
        EventManager.AddListener<ShopCardTappedEvent>(CardSoldEventHandler);
        EventManager.AddListener<RunFinishedEvent>(RunFinishedEventHandler);
    }

    void Start()
    {
        UpdateUI();
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

    public int GetLevelsFootprint(List<int> levelsArg)
    {
        int result = 1;
        foreach (int levelNumber in levelsArg)
        {
            result *= levelNumber;
            result %= bigPrime;
        }
        return result;
    }

    int GetLatestFootprint(int latestLevelNumberArg, bool completedArg)
    {
        List<int> completedLevelsTemp = completedLevelsManager.GetLevels();
        if (completedArg && completedLevelsTemp.Contains(latestLevelNumberArg))
        {
            completedLevelsTemp.Add(latestLevelNumberArg);
        }
        int result = GetLevelsFootprint(completedLevelsTemp);
        return result;
    }

    public void RunFinishedEventHandler(IEvent evArg)
    {
        RunFinishedEvent evData = evArg as RunFinishedEvent;
        int completedLevelNumber = evData.runNumber;
        bool isFinishedLevelCompleted = evData.completed;
        int footprint = GetLatestFootprint(completedLevelNumber, isFinishedLevelCompleted);
        CreateOffer(footprint);
    }

    public void CreateOffer(int completedLevelsFootprint)
    {
        cards.Clear();
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
        UpdateUI();
    }

    void UpdateUI()
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
