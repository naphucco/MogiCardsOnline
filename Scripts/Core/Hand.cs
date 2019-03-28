using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Hand
{
    public List<string> cards;

    public void Clear()
    {
        cards = null;
    }

    public void AddToHand(string card)
    {
        if (card != null)
        {
            if (cards == null) cards = new List<string>();
            this.cards.Add(card);
        }
    }

    public void AddToHand(List<string> cards)
    {
        if (cards == null) cards = new List<string>();
        if (this.cards == null) this.cards = new List<string>();
        this.cards.AddRange(cards);
    }

    public void DrawCard(int position)
    {
        cards.RemoveAt(position - 1);
    }

    public void SwitchCardPosition(int oldPosition, int newPosition)
    {
        string temp = cards[newPosition];
        cards[newPosition] = cards[oldPosition];
        cards[oldPosition] = temp;
    }
}
