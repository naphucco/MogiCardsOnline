using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Piles
{
    public Pile mogiPile;
    public Pile bonusPile;

    public void CreateRandomPile()
    {
        mogiPile.CreateRandomPile(CardData.Instance.allMogi);
        bonusPile.CreateRandomPile(CardData.Instance.allBonus);
    }
}
