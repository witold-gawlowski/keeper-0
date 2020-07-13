using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    public List<Card> cards;

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
    }

    public void Shuffle()
    {
        queue = new List<Card>(cards);
        queue.Shuffle();
    }

    public bool IsDeckEmpty()
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
    }
    public void Remove(Card cardArg)
    {
        cards.Remove(cardArg);
        EventManager.SendEvent(new UpdateDeckUIEvent(cards));
    }

    public void FromString(string sourceArg)
    {
        string[] words = sourceArg.Split(';');
        foreach (string s in words)
        {
            cards.Add(new Card(s));
        }
    }

    public override string ToString()
    {
        string result = "";
        foreach (Card c in cards)
        {
            result += c.ToString() + ";";
        }
        result.TrimEnd(';');
        return result;
    }

}
