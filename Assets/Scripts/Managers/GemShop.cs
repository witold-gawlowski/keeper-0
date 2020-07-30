using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopCardTappedEvent : IEvent {
    public Card card; public ShopCardTappedEvent(Card cArg) { card = cArg; }
}

public class GemShop : MonoBehaviour
{
    public List<Card> cards;
    int gems;
    int startingGems = 5;
    public CompletedLevelsManager completedLevelsManager;
    public CardCodex cardCodex;
    public Inventory inventory;
    public MainMenuScript mainMenuScript;
    public GemShopUIScript ui;

    void Awake()
    {
        EventManager.AddListener<ShopCardTappedEvent>(CardSoldEventHandler);
       
    }

    void Start()
    {
        SetupGemCount();
        ui.UpdateButtonDisability(gems);
        ui.CreateButtons(cards);
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
                
            }
            SaveGems();
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

    void SaveGems()
    {
        PlayerPrefs.SetInt("gems", gems);
    }

    void CardSoldEventHandler(IEvent evArg)
    {
        ShopCardTappedEvent ev = evArg as ShopCardTappedEvent;
        TrySell(ev.card);
    }

    private void TrySell(Card cardArg)
    {
        if (gems >= cardArg.gemCost)
        {
            gems -= cardArg.gemCost;
            cards.Remove(cardArg);
            inventory.Add(cardArg);
            SaveGems();
            mainMenuScript.UpdateGems(gems);
            ui.UpdateButtonDisability(gems);
        }
    }
}
