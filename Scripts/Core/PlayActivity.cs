using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//activity of player in battle
//[Serializable]
public class PlayActivity
{
    public int actionPoints;
    public int healthPoints;

    private int drawRemainByTurn;
    private Player player;

    public PlayActivity(Player player)
    {
        healthPoints = 4;
        this.player = player;
    }

    public void StartTurn()
    {
        actionPoints = 4;
        drawRemainByTurn = 999;
    }    

    public string GetCard(string cardName)
    {
        Card.Type cardType = CardData.Instance.GetCard(cardName, false).type;
        string card = null;
        if(cardType == Card.Type.bonus) card = player.piles.bonusPile.ForceDrawCard(cardName);
        else card = player.piles.mogiPile.ForceDrawCard(cardName);                        
        return card;
    }

    public List<string> GetMogiCard(int amount,bool getByTurn)
    {
        List<string> card = new List<string>();

        if (amount > 0)
        {
            if (!getByTurn || drawRemainByTurn > 0)
            {
                card = player.piles.mogiPile.DrawCard(amount);
                if (getByTurn) drawRemainByTurn -= amount;
                return card;
            }
        }

        return card;
    }

    public List<string> GetBonusCard(int amount, bool getByTurn)
    {
        List<string> card = new List<string>();

        if (!getByTurn || drawRemainByTurn > 0)
        {
            card = player.piles.bonusPile.DrawCard(amount);
            if (getByTurn) drawRemainByTurn -= amount;
            return card;
        }

        return card;
    }    
}