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

    public void OnSelectCard(CardEntity card)
    {
        if (card.motion.curStatus == CardMotion.status.inSlot)
        {
            if (card.info.type == Card.Type.mogi)
            {
                List<MogiEntity> opponentMogis = BoardUI.Instance.MogiInBoard(false);

                for (int i = 0; i < opponentMogis.Count; i++)
                {
                    ((CardMogiDisplay)opponentMogis[i].display).Targeted();
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

        if (card.motion.curStatus == CardMotion.status.inHand)
        {
            //front of all               
            card.motion.render.sortingOrder = 1000;
            card.motion.cardTran.position = choosePos;
        }
        else if (card.motion.curStatus == CardMotion.status.inSlot)
        {
            if (card.info.type == Card.Type.mogi)
            {
                List<MogiEntity> opponentMogis = BoardUI.Instance.MogiInBoard(false);

                for (int i = 0; i < opponentMogis.Count; i++)
                {
                    ((CardMogiDisplay)opponentMogis[i].display).Targeted();
                }
                
                AttackArrow.Instance.Display(card.motion.cardTran.position, choosePos);
            }
        }
    }

    public void OnDeselectCard(CardEntity card)
    {
        List<MogiEntity> opponentMogis = BoardUI.Instance.MogiInBoard(false);

        for (int i = 0; i < opponentMogis.Count; i++)
        {
            ((CardMogiDisplay)opponentMogis[i].display).StopTargeted();
        }

        CheckMoveToControllerBoard(card); //move to board
        CheckMogiControllerAttack(card); //mogi attack
    }

    private void CheckMoveToControllerBoard(CardEntity card)
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
                        card.motion.MoveToSlot();
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
                    card.motion.MoveToTopOfMogi(mogiCard);
                    card.motion.Dissolving();
                }
            }
        }
    }

    private void CheckMogiControllerAttack(CardEntity card)
    {
        if (AttackArrow.Instance.showing)
        {
            AttackArrow.Instance.Hide();

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
                    card.motion.MogiAttackAnimation(opponentMogis[i].motion,() => 
                    {
                        card.GetComponent<MogiEntity>().Fight(opponentMogis[i].GetComponent<MogiEntity>());
                    });

                    break;
                }
            }
        }
    }

    
}
