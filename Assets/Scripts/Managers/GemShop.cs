using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopCardTappedEvent : IEvent {
    public Card card; public ShopCardTappedEvent(Card cArg) { card = cArg; }
}

public class GemShop : MonoBehaviour
{
    public List<Card> _cards;
    private int _gems;

    public int startingGems = 5;
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
        _cards = cardCodex.GetCards();
        SetupGemCount();
        mainMenuScript.UpdateGems(_gems);
        ui.CreateButtons(_cards);
        ui.UpdateButtonDisability(_gems);
    }

    void SetupGemCount()
    {
        if(RunResultScript.instance == null)
        {
            if (PlayerPrefs.HasKey("gems"))
            {
                _gems = PlayerPrefs.GetInt("gems");
            }
            else
            {
                _gems = startingGems;
                
            }
            SaveGems();
        }
        else
        {
            _gems = PlayerPrefs.GetInt("gems");
            if (RunResultScript.instance.completed)
            {
                _gems += RunResultScript.instance.gems;
                SaveGems();
            }
        }
    }

    void SaveGems()
    {
        PlayerPrefs.SetInt("gems", _gems);
    }

    void CardSoldEventHandler(IEvent evArg)
    {
        ShopCardTappedEvent ev = evArg as ShopCardTappedEvent;
        TrySell(ev.card);
    }

    private void TrySell(Card cardArg)
    {
        if (_gems >= cardArg.gemCost)
        {
            _gems -= cardArg.gemCost;
            _cards.Remove(cardArg);
            inventory.Add(cardArg);
            SaveGems();
            mainMenuScript.UpdateGems(_gems);
            ui.UpdateButtonDisability(_gems);
        }
    }
}
