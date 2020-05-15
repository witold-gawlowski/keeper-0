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
        levelsUIScript.UpdateFunds(money);
    }
    public bool TryPay(int price)
    {
        if (money >= price)
        {
            money -= price;
            levelsUIScript.UpdateFunds(money);
            return true;
        }
        return false;
    }

    public void AddFunds(int value)
    {
        money += value;
        levelsUIScript.UpdateFunds(money);
    }

    public void GetCash(int price)
    {
        money += price;
        levelsUIScript.UpdateFunds(money);
    }
}
