using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardCodex : MonoBehaviour
{
    public List<Card> cards;
    public Card GetRandomCard(Randomizer rArg)
    {
        int numberOfCards = cards.Count;
        int randomIndex = rArg.Range(0, numberOfCards);
        Card result = cards[randomIndex];
        return result;
    }
}
