using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SlotUI
{
    public CardMotion cardMotion;
    public Transform slotTransform;

    public void InsertToSlot(CardMotion card)
    {
        cardMotion = card;
        card.MoveToPosition(slotTransform.position, 2, false, 20);
    }
    public void PutOnTopOfMogi(CardMotion card, CardMotion mogi)
    {
        card.MoveToPosition(mogi.cardTran.position, 1000, true, 20, async () =>
        {
            await new WaitForSeconds(0.5f);

            if (CardData.Instance.GetCard(card.name, false).features != null)
            {
                CardData.Instance.GetCard(card.name, false).features.CashSkill();
            }
            else
            {
                Debug.Log("not have features");
            }
        });
    }
}