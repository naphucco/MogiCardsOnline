using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardUI : MonoBehaviour {

    //Use 1 unique instance
    private static BoardUI instance = null;

    public static BoardUI Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<BoardUI>();
            }
            return instance;
        }
    }

    //ara to put card down
    public SpriteRenderer putDownCardArea;

    public SlotUI[] controllerSlotUIs = new SlotUI[5];
    public SlotUI[] opponentSlotUIs = new SlotUI[5];

    public List<CardMotion> MogiInBoard(bool isController)
    {
        SlotUI[] slots = null;
        List<CardMotion> cardMogis = new List<CardMotion>();

        if (isController)
        {
            slots = controllerSlotUIs;
        }
        else
        {
            slots = opponentSlotUIs;
        }

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].cardMotion != null)
            {
                if (CardData.Instance.GetCard(slots[i].cardMotion.name, false).type == Card.Type.mogi)
                {
                    cardMogis.Add(slots[i].cardMotion);
                }
            }
        }

        return cardMogis;
    }

    public bool HaveEmptySlot(bool isController)
    {
        SlotUI[] slots = null;

        if (isController)
        {
            slots = controllerSlotUIs;
        }
        else
        {
            slots = opponentSlotUIs;
        }

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].cardMotion == null)
            {
                return true;
            }
        }

        return false;
    }

    public SlotUI GetFirstEmptySlot(bool isController)
    {
        SlotUI[] slots = null;

        if (isController)
        {
            slots = controllerSlotUIs;
        }
        else
        {
            slots = opponentSlotUIs;
        }

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].cardMotion == null)
            {
                return slots[i];
            }
        }

        return null;
    }

    public bool InsertToSlot(CardMotion card, bool toController)
    {
        SlotUI[] slots = null;

        if (toController)
        {
            slots = controllerSlotUIs;
        }
        else
        {
            slots = opponentSlotUIs;
        }

        if (Game.Instance.controller.activity.PutOnBoard(card.name))
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].cardMotion == null)
                {
                    slots[i].InsertToSlot(card);
                    return true;
                }
            }
        }

        return false;
    }

    public void PutOnTopMogi(CardMotion moveCard, CardMotion mogiCard, bool toController)
    {
        SlotUI[] slots = null;

        if (toController)
        {
            slots = controllerSlotUIs;
        }
        else
        {
            slots = opponentSlotUIs;
        }

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].cardMotion == mogiCard)
            {
                slots[i].PutOnTopOfMogi(moveCard, mogiCard);
            }
        }
    }
}