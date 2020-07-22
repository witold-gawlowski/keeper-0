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
        SetupGemCount();
        SetupShopCards();
        UpdateUI();
    }

    void SetupGemCount()
    {
        if(RunResultScript.instance == null)
        {
            if (PlayerPrefs.HasKey("gems"))
            {
                gems = PlayerPrefs.GetInt("gems");
            }
            else
            {
                gems = startingGems;
                SaveGems();
            }
        }
        else
        {
            gems = PlayerPrefs.GetInt("gems");
            if (RunResultScript.instance.completed)
            {
                gems += RunResultScript.instance.gems;
                SaveGems();
            }
        }
    }
    void SetupShopCards()
    {
        if (RunResultScript.instance == null)
        {
            if (PlayerPrefs.HasKey("gemShop"))
            {
                LoadShopCards();
            }
            else
            {
                int footprint = completedLevelsManager.GetLevelsFootprint();
                CreateOffer(footprint);
                SaveShopCards();
            }
        }
        else
        {
            if (RunResultScript.instance.completed)
            {
                int footprint = completedLevelsManager.GetLevelsFootprint();
                CreateOffer(footprint);
                SaveShopCards();
            }
            else
            {
                LoadShopCards();
            }
        }
    }

    public void LoadShopCards()
    {
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

    void SaveGems()
    {
        PlayerPrefs.SetInt("gems", gems);
    }

    public void SaveShopCards()
    {
        PlayerPrefs.SetString("gemShop", ToString());
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
            SaveGems();
            SaveShopCards();
            EventManager.SendEvent(new UpdateGemShopUIEvent(cards, gems));
        }
    }
}
