using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardCodex : MonoBehaviour
{
    public static CardCodex instance;
    public List<Card> cards;
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
        Init();
    }

    void Init()
    {
        dictionary = new Dictionary<string, Card>();
        foreach (Card c in cards)
        {
            dictionary.Add(c.id, c);
        }
    }

    public Card GetCardByID(string idArg)
    {
        return dictionary[idArg];
    }

    public Card GetRandomCard(Randomizer rArg)
    {
        int numberOfCards = cards.Count;
        int randomIndex = rArg.Range(0, numberOfCards);
        Card result = cards[randomIndex];
        return result;
    }
}
