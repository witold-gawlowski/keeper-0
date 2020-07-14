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
    List<Card> cards;
    const int bigPrime = int.MaxValue;
    int numberOfCardsInOffer = 5;
    int gems;
    int startingGems = 5;
    public CompletedLevelsManager completedLevelsManager;
    public CardCodex cardCodex;
    public Inventory inventory;

    void Awake()
    {
        EventManager.AddListener<ShopCardTappedEvent>(CardSoldEventHandler);

       
    }

    void Start()
    {
        if (RunResultScript.instance != null)
        {
            RunFinishedEventHandler();
        }

        Load();
        if (cards == null)
        {
            List<int> completedLevels = completedLevelsManager.GetLevels();
            int completedLevelsFootprint = GetLevelsFootprint(completedLevels);
            CreateOffer(completedLevelsFootprint);
        }
        UpdateUI();
    }

    public void Load()
    {
        if (PlayerPrefs.HasKey("gems"))
        {
            int gems = PlayerPrefs.GetInt("gems");
            LoadGems(gems);
        }
        else
        {
            gems = startingGems;
        }

        if (PlayerPrefs.HasKey("gemShop"))
        {
            string inventoryString = PlayerPrefs.GetString("gemShop");
            FromString(inventoryString);
        }
    }

    public void FromString(string sourceArg)
    {
        cards = new List<Card>();
        if (sourceArg != "")
        {
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
        foreach (Card c in cards)
        {
            result += c.id + ";";
        }
        result = result.TrimEnd(new char[] { ';' });
        return result;
    }

    public void Save()
    {
        PlayerPrefs.SetInt("gems", gems);
        PlayerPrefs.SetString("gemShop", ToString());
        PlayerPrefs.Save();
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

    public void RunFinishedEventHandler()
    {
        int completedLevelNumber = RunResultScript.instance.runNumber;
        bool isFinishedLevelCompleted = RunResultScript.instance.completed;
        int footprint = GetLatestFootprint(completedLevelNumber, isFinishedLevelCompleted);
        CreateOffer(footprint);
        UpdateUI();
    }

    public void CreateOffer(int completedLevelsFootprint)
    {
        cards = new List<Card>();
        Randomizer r = new Randomizer(completedLevelsFootprint);
        for(int i=0; i<numberOfCardsInOffer; i++)
        {
            Card randomCard = cardCodex.GetRandomCard(r);
            cards.Add(randomCard);
        }
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
            inventory.Add(cardArg);
            EventManager.SendEvent(new UpdateGemShopUIEvent(cards, gems));
            Save();
        }
    }
}
