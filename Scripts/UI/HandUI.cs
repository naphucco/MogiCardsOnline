using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandUI : MonoBehaviour {

    private static HandUI instance = null;

    public static HandUI Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<HandUI>();
            }
            return instance;
        }
    }
    
    public List<CardEntity> cardInControllerHand;
    public List<CardEntity> cardInOpponentHand;

    //hand position
    public Vector3 controllerHandPos;
    public Vector3 opponentHandPos;

    private float maxHandWith = 10;
    private float handWith;

    private void Init()
    {
        cardInControllerHand = new List<CardEntity>();
    }

    public List<CardEntity> MogisInHand(bool isController)
    {
        List<CardEntity> cardInHand = null;
        if (isController) cardInHand = cardInControllerHand;
        else cardInHand = cardInOpponentHand;

        List<CardEntity> mogisInhand = new List<CardEntity>();

        for (int i = 0; i < cardInHand.Count; i++)
        {
            if (cardInHand[i].info.type == Card.Type.mogi)
            {
                mogisInhand.Add(cardInHand[i]);
            }
        }

        return mogisInhand;
    }

    public void InsertCard(CardEntity card,bool toController)
    {
        List <CardEntity> cardInHand = null;
        if (toController) cardInHand = cardInControllerHand;
        else cardInHand = cardInOpponentHand;
        cardInHand.Add(card);

        if (cardInHand.Count > 0)
        {
            //chaneg pos of aready card
            for (int i = 0; i < cardInHand.Count; i++)
            {
                CardEntity oldCard = cardInHand[i];
                cardInHand[i].motion.MoveToPosition(GetCardPosition(oldCard, toController), 1 + i * 2, false, 4);
            }
        }
    }

    public void RemoveCard(CardEntity card, bool isController)
    {
        List<CardEntity> cardInHand = null;
        if (isController) cardInHand = cardInControllerHand;
        else cardInHand = cardInOpponentHand;

        cardInHand.Remove(card);

        //chaneg pos of when had remove
        for (int i = 0; i < cardInHand.Count; i++)
        {
            CardEntity cardRemain = cardInHand[i];
            cardInHand[i].motion.MoveToPosition(GetCardPosition(cardRemain, isController), 1 + i * 2, false, 4);
        }
    }

    public Vector3 GetCardPosition(CardEntity card, bool isController)
    {
        List<CardEntity> cardInHand = null;
        if (isController) cardInHand = cardInControllerHand;
        else cardInHand = cardInOpponentHand;

        if (cardInHand.Count == 1)
        {
            if (isController)
                return controllerHandPos;
            else
                return opponentHandPos;
        }
        else
        {
            int cardIndex = cardInHand.Count;

            for (int i = 0; i < cardInHand.Count; i++)
            {
                if (card == cardInHand[i])
                {
                    cardIndex = i;
                    break;
                }
            }

            handWith = 1;

            while (handWith / cardInHand.Count < 1 && handWith < maxHandWith)
            {
                //extend the length if there are too many cards
                handWith = Mathf.Clamp(handWith + 1, 0, maxHandWith);
            }

            Vector3 pos = Vector3.zero;

            if (isController) pos = controllerHandPos;
            else pos = opponentHandPos;

            float cardDistance = handWith / (cardInHand.Count);
            return new Vector3(pos.x - handWith / 2 + cardDistance * (cardIndex + 0.5f), pos.y, pos.z);
        }
    }

    

    #region Unity

    private void Start()
    {
        Init();
    }

    #endregion
}
