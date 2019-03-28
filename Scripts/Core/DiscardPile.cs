using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DiscardPile
{
    //There is no unlock card feature, and load card    
    public List<string> cardInPile;

    public void Clear()
    {
        cardInPile = null;
    }

    //add to discardPile
    public void Discard(string cardName)
    {
        if (cardInPile == null) cardInPile = new List<string>();
        cardInPile.Add(cardName);
    }

    public List<string> TakeAllCard()
    {
        List<string> cardInPile = Helper.Shuffle(this.cardInPile);
        if(this.cardInPile != null) this.cardInPile.Clear();
        return cardInPile;
    }
}