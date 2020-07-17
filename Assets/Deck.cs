using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    List<Card> cards;
    public List<Card> startingCards;
    public List<Card> queue;

    public static Deck instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        instance.Load();
    }

    public void Load()
    {
        if (PlayerPrefs.HasKey("deck"))
        {

            string deckString = PlayerPrefs.GetString("deck");
            FromString(deckString);
        }
        else
        {
            if (startingCards == null)
            {
                cards = new List<Card>();
            }
            else
            {
                cards = startingCards;
            }
        }
    }

    public List<Card> GetCards()
    {
        return cards;
    }

    public void Save()
    {
        PlayerPrefs.SetString("deck", ToString());
        PlayerPrefs.Save();
    }

    public void Shuffle()
    {
        queue = new List<Card>(cards);
        queue.Shuffle();
    }

    public bool IsDeckEmpty()
    {
        return cards.Count == 0;
    }

    public bool IQueueEmpty()
    {
        return queue.Count == 0;
    }

    public Card Draw()
    {
        Card result = queue[0];
        queue.RemoveAt(0);
        return result;
    }

    public void Add(Card cardArg)
    {
        cards.Add(cardArg);
        EventManager.SendEvent(new UpdateDeckUIEvent(cards));
        Save();
    }
    public void Remove(Card cardArg)
    {
        cards.Remove(cardArg);
        EventManager.SendEvent(new UpdateDeckUIEvent(cards));
        Save();
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

}
