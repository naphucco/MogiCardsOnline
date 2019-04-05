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

    //get mogi has not yet attacked
    public List<MogiEntity> MogiInBoardNotAttacked(bool isController)
    {
        List<MogiEntity> mogis = MogiInBoard(isController);
        List<MogiEntity> notAttack = new List<MogiEntity>();

        for (int i = 0; i < mogis.Count; i++)
        {
            if (!mogis[i].hadAttack)
            {
                notAttack.Add(mogis[i]);
            }
        }

        return notAttack;
    }

    public List<MogiEntity> CardInBoard(bool isController)
    {
        SlotUI[] slots = null;
        List<MogiEntity> cardMogis = new List<MogiEntity>();

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
            if (slots[i].cardEntity != null)
            {
                cardMogis.Add((MogiEntity)slots[i].cardEntity);
            }
        }

        return cardMogis;
    }

    public List<MogiEntity> MogiInBoard(bool isController)
    {
        SlotUI[] slots = null;
        List<MogiEntity> cardMogis = new List<MogiEntity>();

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
            if (slots[i].cardEntity != null)
            {
                if (CardData.Instance.GetCard(slots[i].cardEntity.name, false).type == Card.Type.mogi)
                {
                    cardMogis.Add((MogiEntity)slots[i].cardEntity);
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
            if (slots[i].cardEntity == null)
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
            if (slots[i].cardEntity == null)
            {
                return slots[i];
            }
        }

        return null;
    }

    public bool InsertToSlot(CardEntity card)
    {
        SlotUI[] slots = null;

        if (card.isController)
        {
            slots = controllerSlotUIs;
        }
        else
        {
            slots = opponentSlotUIs;
        }

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].cardEntity == null)
            {
                slots[i].InsertToSlot(card);
                return true;
            }
        }
        
        return false;
    }

    public SlotUI PutOnTopMogi(CardEntity moveCard, MogiEntity mogiCard)
    {
        SlotUI[] slots = null;

        if (moveCard.isController)
        {
            slots = controllerSlotUIs;
        }
        else
        {
            slots = opponentSlotUIs;
        }

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].cardEntity == mogiCard)
            {
                slots[i].PutOnTopOfMogi(moveCard, mogiCard);
                return slots[i];
            }
        }

        return null;
    }
}