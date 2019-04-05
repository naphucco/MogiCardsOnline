using System;
using UnityEngine;

public class CardMotion : MonoBehaviour
{
    public SpriteRenderer render { get; private set; }
    public Transform cardTran { get; private set; }    
    public bool moving { get; private set; }    
        
    private bool showingDetail;
    private Vector3 normalPos;    
    private Action<CardEntity> onDeslect, onSelecting, onSelect;

    protected CardDisplay cardUI;
    protected int normalsortingOrder;
    protected CardEntity entity;

    public virtual void Init(CardEntity entity)
    {
        cardTran = transform;        
        normalPos = cardTran.position;
        this.entity = entity;
        render = GetComponent<SpriteRenderer>();
        cardUI = GetComponent<CardDisplay>();        
    }

    public void PileToHand(Action complete)
    {
        if (entity.isController)
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
        HandUI.Instance.InsertCard(entity);
        entity.curStatus = CardEntity.status.inHand;

        render.sortingOrder = 1000;

        MoveToPosition(HandUI.Instance.GetCardPosition(entity),
            HandUI.Instance.cardInOpponentHand.Count * 2, false, 10, () =>
         {
             MotionManager.RemoveMotion();
             complete?.Invoke();
         });
    }

    public async void MoveToSlot(SlotUI slot,bool moveToTop,Action complete)
    {
        MotionManager.AddMotion();

        if (!cardUI.showFront)
        {
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
            cardUI.ShowFrontComplete();

            await new WaitForSeconds(0.2f);
        }

        int sorting = 1;
        if (moveToTop) sorting = 1000;

        MoveToPosition(slot.slotTransform.position, sorting, false, 10, () => {
            //move to hand
            MotionManager.RemoveMotion();
            if(!moveToTop) entity.MoveToSlot();
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
        cardUI.ShowFrontComplete();

        await new WaitForSeconds(0.2f);

        //move to hand
        HandUI.Instance.InsertCard(entity);
        entity.curStatus = CardEntity.status.inHand;
        
        MoveToPosition(HandUI.Instance.GetCardPosition(entity),
            HandUI.Instance.cardInControllerHand.Count * 2, false, 20, () =>
            {
                MotionManager.RemoveMotion();
                complete?.Invoke();
            });
    }

    private bool isSelecting;

    public void AddSelectingCardEvent(Action<CardEntity> cardEvent)
    {
        onSelecting += cardEvent;
    }
    
    public void AddSelectCardEvent(Action<CardEntity> cardEvent)
    {
        onSelect += cardEvent;
    }

    //run when selecting
    public void Selecting()
    {
        if (!isSelecting)
        {
            onSelect?.Invoke(entity);
            isSelecting = true;
        }

        onSelecting?.Invoke(entity);
    }

    public void AddDeselectCardEvent(Action<CardEntity> cardEvent)
    {
        onDeslect += cardEvent;
    }

    public void DeSelecting()
    {
        if (isSelecting)
        {
            isSelecting = false;
            render.sortingOrder = normalsortingOrder;
            onDeslect?.Invoke(entity);
        }

        if (!moving && entity.isController)
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
    
    private void MogiDeadEffect()
    {
        
    }
}
