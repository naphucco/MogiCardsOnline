using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardMotion : MonoBehaviour
{
    public new AnimationCurve animation;

    public SpriteRenderer render { get; private set; }
    public Transform cardTran { get; private set; }
    public bool isController { get; private set; }
    public bool moving { get; private set; }
    public enum status { none, inHand, inSlot, attacking }

    private status curStatus;
    private CardDisplay cardUI;
    private bool showingDetail;
    private Vector3 normalPos;
    private int normalsortingOrder;
    private Card cardInfo;

    public void Init(PileUI pileUI, Card cardInfo, bool isController)
    {
        cardTran = transform;
        curStatus = status.none;
        this.isController = isController;
        normalPos = cardTran.position;
        this.cardInfo = cardInfo;
        render = GetComponent<SpriteRenderer>();
        cardUI = GetComponent<CardDisplay>();
    }

    public void PileToHand(Action complete)
    {
        if (isController)
        {
            PileToControllerHand(complete);
        }
        else
        {
            PileToOpponentHand(complete);
        }
    }

    private void PileToOpponentHand(Action complete)
    {        
        MotionManager.AddMotion();
        HandUI.Instance.InsertCard(this, isController);
        curStatus = status.inHand;
        ControllerBehaviour.Instance.AddNewCard(this);

        MoveToPosition(HandUI.Instance.GetCardPosition(this, isController),
            HandUI.Instance.cardInOpponentHand.Count * 2, false, 10, () =>
         {
             MotionManager.RunComplete();
             complete?.Invoke();
         });
    }

    public async void HandToOpponentTopMogi(CardMotion mogi, Action complete)
    {
        MotionManager.AddMotion();
        render.sortingOrder = 1000;
        bool moveComplete = false;

        MoveToPosition(new Vector3(0, 0, -5), 1000, true, 20, () =>
         {
             moveComplete = true;
         });

        await new WaitUntil(() => moveComplete);
        await new WaitForSeconds(0.2f);

        Quaternion midle = Quaternion.Euler(0, -90, 0);

        //rotation
        float timeCouter = 0;
        while (timeCouter < 1)
        {
            timeCouter += Time.deltaTime * 10f;
            cardTran.rotation = Quaternion.Lerp(Quaternion.identity, midle, timeCouter);
            await new WaitForUpdate();
        }

        cardUI.ShowFrontOfCard();
        render.flipX = true;
        Quaternion last = Quaternion.Euler(0, -180, 0);
        timeCouter = 0;

        while (timeCouter < 1)
        {
            timeCouter += Time.deltaTime * 10f;
            cardTran.rotation = Quaternion.Lerp(midle, last, timeCouter);
            await new WaitForUpdate();
        }

        render.flipX = false;
        cardTran.rotation = Quaternion.identity;
        cardUI.RotationComplete();

        await new WaitForSeconds(0.2f);

        MotionManager.RunComplete();

        MoveToTopOfMogi(mogi);
        Dissolving(() => {
            AIBehaviour.Instance.inAction = false;
            complete.Invoke();
        });
    }


    public async void HandToOpponentSlot(SlotUI slot, Action complete)
    {
        MotionManager.AddMotion();
        bool moveComplete = false;
        //move to center
        MoveToPosition(new Vector3(0, 0, -5), 1000, true, 20, () =>
        {
            moveComplete = true;
        });

        await new WaitUntil(() => moveComplete);
        await new WaitForSeconds(0.2f);

        Quaternion midle = Quaternion.Euler(0, -90, 0);

        //rotation
        float timeCouter = 0;
        while (timeCouter < 1)
        {
            timeCouter += Time.deltaTime * 10f;
            cardTran.rotation = Quaternion.Lerp(Quaternion.identity, midle, timeCouter);
            await new WaitForUpdate();
        }

        cardUI.ShowFrontOfCard();
        render.flipX = true;
        Quaternion last = Quaternion.Euler(0, -180, 0);
        timeCouter = 0;

        while (timeCouter < 1)
        {
            timeCouter += Time.deltaTime * 10f;
            cardTran.rotation = Quaternion.Lerp(midle, last, timeCouter);
            await new WaitForUpdate();
        }

        render.flipX = false;
        cardTran.rotation = Quaternion.identity;
        cardUI.RotationComplete();

        await new WaitForSeconds(0.2f);

        MoveToPosition(slot.slotTransform.position, 1, false, 10, () =>
        {
            //move to hand
            MotionManager.RunComplete();
            AIBehaviour.Instance.inAction = false;
            MoveToSlot();
            complete.Invoke();
        });
    }

    private async void PileToControllerHand(Action complete)
    {
        MotionManager.AddMotion();
        bool moveComplete = false;

        MoveToPosition(new Vector3(0, 0, -5), 1000, true, 20, () =>
        {
            moveComplete = true;
        });

        await new WaitUntil(() => moveComplete);
        await new WaitForSeconds(0.2f);

        Quaternion midle = Quaternion.Euler(0, -90, 0);

        //rotation
        float timeCouter = 0;
        while (timeCouter < 1)
        {
            timeCouter += Time.deltaTime * 10f;
            cardTran.rotation = Quaternion.Lerp(Quaternion.identity, midle, timeCouter);
            await new WaitForUpdate();
        }

        cardUI.ShowFrontOfCard();
        render.flipX = true;
        Quaternion last = Quaternion.Euler(0, -180, 0);
        timeCouter = 0;

        while (timeCouter < 1)
        {
            timeCouter += Time.deltaTime * 10f;
            cardTran.rotation = Quaternion.Lerp(midle, last, timeCouter);
            await new WaitForUpdate();
        }

        render.flipX = false;
        cardTran.rotation = Quaternion.identity;
        cardUI.RotationComplete();

        await new WaitForSeconds(0.2f);

        //move to hand
        HandUI.Instance.InsertCard(this, isController);
        curStatus = status.inHand;

        ControllerBehaviour.Instance.AddNewCard(this);
        
        MoveToPosition(HandUI.Instance.GetCardPosition(this, isController),
            HandUI.Instance.cardInControllerHand.Count * 2, false, 20, () =>
            {
                MotionManager.RunComplete();
                complete?.Invoke();
            });
    }

    private bool isSelecting;

    //run when selecting
    public void Selecting()
    {
        if (!isSelecting)
        {
            isSelecting = true;            
        }
        
        if (isController)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = -Camera.main.transform.position.z; // select distance = 10 units from the camera
            Vector3 choosePos = Camera.main.ScreenToWorldPoint(mousePos);
            choosePos.z = 0;

            if (this.curStatus == status.inHand)
            {
                //front of all               
                render.sortingOrder = 1000;
                cardTran.position = choosePos;
            }
            else if (this.curStatus == status.inSlot)
            {
                if(cardInfo.type == Card.Type.mogi) AttackArrow.Instance.Display(cardTran.position, choosePos);
            }
        }
    }

    public void DeSelecting()
    {
        if (isSelecting)
        {
            isSelecting = false;
            render.sortingOrder = normalsortingOrder;
            //về mới sortingOrder lại
            //render.sortingOrder = normalSortingOrder;
            if (isController)
            {
                CheckMoveToControllerBoard(); //move to board
                CheckMogiControllerAttack(); //mogi attack
            }
        }

        if (!moving && isController)
        {
            if (cardTran.position != normalPos) cardTran.position = Vector3.MoveTowards(cardTran.position, normalPos, Time.deltaTime * 20);
        }
    }


    public async void MoveToPosition(Vector3 targetPos, int sortingOrder, bool sortingOrderFirst, float moveSpeed, Action onComplete = null)
    {
        moving = true;
        if (sortingOrderFirst) render.sortingOrder = sortingOrder;

        while (cardTran.position != targetPos)
        {
            await new WaitForUpdate();
            cardTran.position = Vector3.MoveTowards(cardTran.position, targetPos, Time.deltaTime * moveSpeed);
        }

        moving = false;
        normalPos = cardTran.position;
        normalsortingOrder = sortingOrder;
        if (!sortingOrderFirst) render.sortingOrder = sortingOrder;
        onComplete?.Invoke();
    }

    private async void Dissolving(Action complete = null)
    {
        await new WaitForSeconds(1f);
        ControllerBehaviour.Instance.RemoveCard(this);
        complete?.Invoke();
        EffectManager.Instance.Instantiate("Bonus_is_used", cardTran.position);
        Destroy(gameObject);
    }

    private void CheckMogiControllerAttack()
    {
        if (AttackArrow.Instance.showing)
        {
            AttackArrow.Instance.Hide();

            Vector3 mousePos = Input.mousePosition;
            mousePos.z = -Camera.main.transform.position.z; // select distance = 10 units from the camera
            Vector3 choosePos = Camera.main.ScreenToWorldPoint(mousePos);
            choosePos.z = 0;
            List<CardMotion> opponentMogis = BoardUI.Instance.MogiInBoard(false);

            for (int i = 0; i < opponentMogis.Count; i++)
            {
                if (opponentMogis[i].render.bounds.Contains(choosePos))
                {
                    //attack target
                    //Debug.Log("attack : " + opponentMogis[i].name);
                    MogiAttackAnimation(opponentMogis[i]);
                    break;
                }
            }
        }
    }

    private void CheckMoveToControllerBoard()
    {
        if (HandUI.Instance.cardInControllerHand.Contains(this))
        {
            if (BoardUI.Instance.putDownCardArea.bounds.Contains(cardTran.position))
            {
                //putcard down to board
                if (cardInfo.manaCost <= Game.Instance.controller.activity.actionPoints)
                {
                    if (cardInfo.putOn == Card.PutOn.emptySlot)
                    {
                        MoveToSlot();
                    }
                    else
                    {
                        CheckTopOfControllerMogi(BoardUI.Instance.MogiInBoard(true));
                    }
                }
            }
        }
    }

    public void CheckTopOfControllerMogi(List<CardMotion> mogiList)
    {
        if (mogiList.Count > 0)
        {
            for (int i = 0; i < mogiList.Count; i++)
            {
                Vector2 cardPos = cardTran.position;
                CardMotion mogiCard = mogiList[i];

                if (mogiList[i].render.bounds.Contains(cardPos))
                {
                    MoveToTopOfMogi(mogiCard);
                    Dissolving();
                }
            }
        }
    }
    
    private void MoveToSlot()
    {
        if (BoardUI.Instance.InsertToSlot(this, isController))
        {
            curStatus = status.inSlot;
            HandUI.Instance.RemoveCard(this, isController);
        }
    }

    private void MoveToTopOfMogi(CardMotion mogi)
    {
        HandUI.Instance.RemoveCard(this, isController);
        BoardUI.Instance.PutOnTopMogi(this, mogi, isController);
    }

    public async void MogiAttackAnimation(CardMotion target)
    {
        curStatus = status.attacking;

        Vector3 startPosition = transform.position;

        float timeCouter = 0;

        while (timeCouter < 1)
        {
            timeCouter += Time.deltaTime * 2;
            if (timeCouter > 1) timeCouter = 1;
            float lerp = animation.Evaluate(timeCouter);
            cardTran.position = Vector3.Lerp(startPosition, target.cardTran.position, lerp);
            await new WaitForUpdate();
        }

        cardTran.position = startPosition;
        curStatus = status.inSlot;
    }

    /*
    public void ChangePosition(Vector3 position, int sortingOrder, float moveSpeed)
    {
        targetPos = position;
        this.normalSortingOrder = sortingOrder;
        moveToTargetSpeed = moveSpeed;
    }
    */
}
