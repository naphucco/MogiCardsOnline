using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//we have two player
[Serializable]
public class Player
{
    public PlayActivity activity;
    public Piles piles;

    public void Reset()
    {
        if(activity == null) activity =  new PlayActivity(this);
        piles.CreateRandomPile();
    }
}