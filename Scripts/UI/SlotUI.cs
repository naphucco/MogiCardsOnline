using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SlotUI
{
    public CardEntity cardEntity;
    public Transform slotTransform;

    public void InsertToSlot(CardEntity card)
    {
        cardEntity = card;
        card.motion.MoveToPosition(slotTransform.position, 2, false, 20);
    }
    public void PutOnTopOfMogi(CardEntity card, MogiEntity mogi)
    {
        if (CardData.Instance.GetCard(card.name, false).features != null)
        {
            CardData.Instance.GetCard(card.name, false).features.CashSkill();
        }
        else
        {
            Debug.Log("not have features");
        }
    }
}