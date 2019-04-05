using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardEntity : MonoBehaviour
{
    public abstract CardDisplay display { get; set; }
    public abstract CardMotion motion { get; set; }
    public abstract Card info { get; set; }
    public abstract bool isController { get; set; }
    public status curStatus { get; set; }
    public enum status { none, inHand, inSlot, attacking }

    public virtual void Init(Card info, CardDisplay display, CardMotion motion, bool isController)
    {
        curStatus = status.none;
        CardBehaviour.Instance.AddNewCard(this);
    }

    public void MoveToSlot()
    {
        if (BoardUI.Instance.InsertToSlot(this))
        {
            curStatus = status.inSlot;
            HandUI.Instance.RemoveCard(this);
        }
    }

    private Action onDiscard;

    public void AddDiscardEvent(Action onDiscard)
    {
        this.onDiscard += onDiscard;
    }

    protected void DisCard()
    {
        CardBehaviour.Instance.RemoveCard(this);

        if (CardBehaviour.Instance.selectingCard == this)
        {
            CardBehaviour.Instance.selectingCard = null;
        }

        DiscardPile discardPile = null;

        if (isController)
        {
            if (info.type == Card.Type.bonus)
                discardPile = Game.Instance.controller.piles.bonusPile.discardPile;
            else if (info.type == Card.Type.mogi)
                discardPile = Game.Instance.controller.piles.mogiPile.discardPile;
        }
        else
        {
            if (info.type == Card.Type.bonus)
                discardPile = Game.Instance.opponent.piles.bonusPile.discardPile;
            else if (info.type == Card.Type.mogi)
                discardPile = Game.Instance.opponent.piles.mogiPile.discardPile;
        }

        discardPile.Discard(name);

        onDiscard?.Invoke();
    }
}
