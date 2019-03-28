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

    public bool PutOnBoard(string cardName)
    {
        BoardSlot[] boardSlot = player.slots.slots;

        for (int i = 0; i < boardSlot.Length; i++)
        {
            if (boardSlot[i] == null)
            {
                boardSlot[i] = new BoardSlot(cardName);
                return true;
            }
        }

        return false;
    }

    public string GetCard(string cardName)
    {
        Card.Type cardType = CardData.Instance.GetCard(cardName, false).type;
        string card = null;
        if(cardType == Card.Type.bonus) card = player.piles.bonusPile.ForceDrawCard(cardName);
        else card = player.piles.mogiPile.ForceDrawCard(cardName);                        
        player.hand.AddToHand(card);
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
                player.hand.AddToHand(card);
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
            player.hand.AddToHand(card);
            if (getByTurn) drawRemainByTurn -= amount;
            return card;
        }

        return card;
    }

    public void SwitchCardPosition(int oldPosition,int newPosition)
    {
        player.hand.SwitchCardPosition(oldPosition, newPosition);
    }

    public void UseCard(int positionInHand)
    {
        player.hand.DrawCard(positionInHand);
    }
}