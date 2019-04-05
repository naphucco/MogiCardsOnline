using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exchange : CardFeatures
{
    public override void CashSkill()
    {
        if (TurnManager.Instance.isOpponentTurn)
        {

        }
        else
        {
            if (Game.Instance.gameType == Game.GameType.botMatch)
            {
                AIExchange();
            }
        }

        base.CashSkill();
    }

    public void AIExchange()
    {
        //simple AI, remove random 
        CardEntity exchangeCard = null;

        List<CardEntity> cards = HandUI.Instance.cardInOpponentHand;
        int cardInOpponentHandCount = cards.Count;
        cards.AddRange(BoardUI.Instance.CardInBoard(false));

        //get exchangeCard
        if (cards.Count > 0)
        {
            int exchangeCardID = Random.Range(0, cards.Count);
            exchangeCard = cards[exchangeCardID];

            //get card in Hand
            if (exchangeCardID < cardInOpponentHandCount)
            {
                HandUI.Instance.RemoveCard(exchangeCard);
            }
            //get card in Board
            else
            {
                //no need to RemoveCard, auto when be destroy

            }
        }
    }
}
