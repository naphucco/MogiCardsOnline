using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New card", menuName = "Data/MogiCard")]
public class MogiCard : Card
{
    public int attackPoint = 1;
    public int level = 1; //we use dif card, for dif level 
    public int hp = 10;

    //I think both different levels are different cards for easy making

    public MogiCard Clone()
    {
        MogiCard card = new MogiCard();

        card.attackPoint = this.attackPoint;
        card.level = this.level;
        card.hp = this.hp;
        card.cardInfo = this.cardInfo;
        card.manaCost = this.manaCost;
        card.features = this.features;

        return card;
    }
}
