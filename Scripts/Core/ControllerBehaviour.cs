﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerBehaviour : MonoBehaviour {

    private static ControllerBehaviour instance = null;

    public static ControllerBehaviour Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<ControllerBehaviour>();
            }
            return instance;
        }
    }

    private bool lookingForTarget;//draw mouse

    public void OnSelectCard(CardEntity card)
    {
        if (card.curStatus == CardEntity.status.inSlot)
        {
            if (card.info.type == Card.Type.mogi)
            {
                List<MogiEntity> opponentMogis = BoardUI.Instance.MogiInBoard(false);
                
                if (!((MogiEntity)card).hadAttack)
                {
                    for (int i = 0; i < opponentMogis.Count; i++)
                    {
                        ((CardMogiDisplay)opponentMogis[i].display).Targeted();
                    }

                    lookingForTarget = true;

                    AttackArrow.Instance.Display(card.motion.cardTran);
                }
            }
        }
    }

    public void OnSelectingCard(CardEntity card)
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -Camera.main.transform.position.z; // select distance = 10 units from the camera
        Vector3 choosePos = Camera.main.ScreenToWorldPoint(mousePos);
        choosePos.z = 0;

        if (card.curStatus == CardEntity.status.inHand)
        {
            //front of all               
            card.motion.render.sortingOrder = 1000;
            card.motion.cardTran.position = choosePos;
        }       
    }

    public void OnDeselectCard(CardEntity card)
    {
        List<MogiEntity> opponentMogis = BoardUI.Instance.MogiInBoard(false);

        for (int i = 0; i < opponentMogis.Count; i++)
        {
            ((CardMogiDisplay)opponentMogis[i].display).StopTargeted();
        }

        CheckMoveToBoard(card); //move to board
        CheckMogiStartAttack(card); //mogi attack

        lookingForTarget = false;
    }

    private void CheckMoveToBoard(CardEntity card)
    {
        if (HandUI.Instance.cardInControllerHand.Contains(card))
        {
            if (BoardUI.Instance.putDownCardArea.bounds.Contains(card.motion.cardTran.position))
            {
                //putcard down to board
                if (card.info.manaCost <= Game.Instance.controller.activity.actionPoints)
                {
                    if (card.info.putOn == Card.PutOn.emptySlot)
                    {
                        card.MoveToSlot();
                    }
                    else
                    {
                        CheckTopOfControllerMogi(card,BoardUI.Instance.MogiInBoard(true));
                    }
                }
            }
        }
    }

    public void CheckTopOfControllerMogi(CardEntity card,List<MogiEntity> mogiList)
    {
        if (mogiList.Count > 0)
        {
            for (int i = 0; i < mogiList.Count; i++)
            {
                Vector2 cardPos = card.motion.cardTran.position;
                CardEntity mogiCard = mogiList[i];

                if (mogiList[i].motion.render.bounds.Contains(cardPos))
                {
                    CardBonusMotion cardBonus = (CardBonusMotion)card.motion;
                    cardBonus.MoveToTopOfMogi(mogiCard);
                }
            }
        }
    }

    private void CheckMogiStartAttack(CardEntity card)
    {
        if (lookingForTarget)
        {
            AttackArrow.Instance.HideArrow();
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = -Camera.main.transform.position.z; // select distance = 10 units from the camera
            Vector3 choosePos = Camera.main.ScreenToWorldPoint(mousePos);
            choosePos.z = 0;
            List<MogiEntity> opponentMogis = BoardUI.Instance.MogiInBoard(false);

            for (int i = 0; i < opponentMogis.Count; i++)
            {
                if (opponentMogis[i].motion.render.bounds.Contains(choosePos))
                {
                    //attack target
                    //Debug.Log("attack : " + opponentMogis[i].name);
                    ((CardMogiMotion)card.motion)
                        .MogiAttackAnimation((CardMogiMotion)opponentMogis[i].motion,() => 
                    {
                        ((MogiEntity)card).Attack(opponentMogis[i]);
                    });

                    break;
                }
            }
        }
    }    
}