using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardCodex : MonoBehaviour
{
    public static CardCodex instance;
    public string resourcesCardFolderName = "Cards";
    public List<Card> _cards;
    Dictionary<string, Card> dictionary;
    

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        LoadCards();
        InitDictionary();
    }

    void LoadCards()
    {
        Debug.Log("loading cards  to card codex");
        _cards = new List<Card>(Resources.LoadAll<Card>(resourcesCardFolderName));
    }

    void InitDictionary()
    {
        dictionary = new Dictionary<string, Card>();
        foreach (Card c in _cards)
        {
            if (dictionary.ContainsKey(c.id))
            {
                Debug.Log("doubled cards in the card codex!");
            }
            dictionary.Add(c.id, c);
        }
    }

    public List<Card> GetCards()
    {
        return _cards;
    }
        
    public Card GetCardByID(string idArg)
    {
        if (!dictionary.ContainsKey(idArg))
        {
            Debug.Log("coundnt find card of id: " + idArg + " in the dictionary");
            return null;
        }
        return dictionary[idArg];
    }

    public Card GetRandomCard(Randomizer rArg)
    {
        int numberOfCards = _cards.Count;
        int randomIndex = rArg.Range(0, numberOfCards);
        Card result = _cards[randomIndex];
        return result;
    }
}
