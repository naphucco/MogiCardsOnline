using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawACard : CardFeatures
{
    public override void CashSkill()
    {
        if (TurnManager.Instance.isOpponentTurn)
        {
            PileUI.Instance.DrawCard(false, 1, false, Card.Type.unknow);
        }
        else
        {
            //draw card
            PileUI.Instance.DrawCard(true, 1, false, Card.Type.unknow);
        }

        base.CashSkill();
    }
}
