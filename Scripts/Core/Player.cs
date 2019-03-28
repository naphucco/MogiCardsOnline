using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//we have two player
[Serializable]
public class Player
{
    public PlayActivity activity;
    public BoardSlots slots;
    public Hand hand;
    public Piles piles;

    public void Reset()
    {
        if(activity == null) activity =  new PlayActivity(this);
        slots.Clear();
        hand.Clear();
        piles.CreateRandomPile();
    }
}