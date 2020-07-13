using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistanceManagerScript : MonoBehaviour
{
    public Inventory inventory;
    public Deck deck;
    public GemShop gemShop;

    public void LoadInventory()
    {
        string inventoryString = PlayerPrefs.GetString("inventory");
        inventory.FromString(inventoryString);
    }

    public void LoadDeck()
    {
        string deckString = PlayerPrefs.GetString("deck");
        deck.FromString(deckString);
    }

    public void SaveInventory()
    {
        PlayerPrefs.SetString("inventory", inventory.ToString());
        PlayerPrefs.Save();
    }
    public void SaveDeck()
    {
        PlayerPrefs.SetString("deck", deck.ToString());
        PlayerPrefs.Save();
    }

    public void LoadGems()
    {
        int gems = PlayerPrefs.GetInt("gems");
        gemShop.LoadGems(gems);
    }

    public void SaveGems()
    {
        PlayerPrefs.SetInt("gems", gemShop.gems);
        PlayerPrefs.Save();
    }
}
