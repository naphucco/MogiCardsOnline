using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New card", menuName = "Data/BonusCard")]
public class BonusCard : Card
{
    public BonusCard Clone()
    {
        BonusCard card = new BonusCard();
        
        card.cardInfo = this.cardInfo;
        card.manaCost = this.manaCost;
        card.features = this.features;

        return card;
    }
}
