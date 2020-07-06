using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class accountManager : MonoBehaviour
{
    public LevelsUIScript levelsUIScript;
    int money;
    public int startingMoney = 1000;
    private void Start()
    {
        money = startingMoney;
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

    public void AddFunds(int value)
    {
        money += value;
        EventManager.SendEvent(new TopBarUIUpdateEvent(null, null, null, null, money));
    }
}
