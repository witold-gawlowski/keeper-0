using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class accountManager : MonoBehaviour
{
    public LevelsUIScript levelsUIScript;
    int money;
    public int startingMoney = 1000;
    int gems = 0;
    private void Awake()
    {
        money = startingMoney;
        gems = 0;
    }
    private void Start()
    {
        EventManager.SendEvent(new TopBarUIUpdateEvent(null, null, null, null, money));
    }
    public bool TryPay(int price)
    {
        if (money >= price)
        {
            money -= price;
            EventManager.SendEvent(new TopBarUIUpdateEvent(null, null, null, null, money));
            return true;
        }
        return false;
    }

    public int GetMoney()
    {
        return money;
    }
    public int GetGems()
    {
        return gems;
    }

    public void AddGems(int value)
    {
        gems += value;
    }

    public void AddFunds(int value)
    {
        money += value;
        EventManager.SendEvent(new TopBarUIUpdateEvent(null, null, null, null, money));
    }
}
