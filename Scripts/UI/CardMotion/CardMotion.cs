using System;
using UnityEngine;

public class CardMotion : MonoBehaviour
{
    public new AnimationCurve animation;

    public SpriteRenderer render { get; private set; }
    public Transform cardTran { get; private set; }
    public bool isController { get; private set; }
    public bool moving { get; private set; }
    public status curStatus { get; set; }

    public enum status { none, inHand, inSlot, attacking }
    
    private CardDisplay cardUI;
    private bool showingDetail;
    private Vector3 normalPos;
    private int normalsortingOrder;
    private Action<CardEntity> onDeslect, onSelecting, onSelect;
    private CardEntity entity;

    public virtual void Init(CardEntity entity, bool isController)
    {
        cardTran = transform;
        curStatus = status.none;
        normalPos = cardTran.position;
        this.entity = entity;
        this.isController = isController;
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
        HandUI.Instance.InsertCard(entity, isController);
        curStatus = status.inHand;
        CardBehaviour.Instance.AddNewCard(entity);

        render.sortingOrder = 1000;

        MoveToPosition(HandUI.Instance.GetCardPosition(entity, isController),
            HandUI.Instance.cardInOpponentHand.Count * 2, false, 10, () =>
         {
             MotionManager.RunComplete();
             complete?.Invoke();
         });
    }

    public async void HandToOpponentTopMogi(MogiEntity mogi, Action complete)
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
            MoveToSlot();
            complete.Invoke();
        });
    }

    public void MoveToSlot()
    {
        if (BoardUI.Instance.InsertToSlot(entity, isController))
        {
            curStatus = status.inSlot;
            HandUI.Instance.RemoveCard(entity, isController);
        }
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
        HandUI.Instance.InsertCard(entity, isController);
        curStatus = status.inHand;

        CardBehaviour.Instance.AddNewCard(entity);

        MoveToPosition(HandUI.Instance.GetCardPosition(entity, isController),
            HandUI.Instance.cardInControllerHand.Count * 2, false, 20, () =>
            {
                MotionManager.RunComplete();
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

    public void MoveToTopOfMogi(CardEntity mogi)
    {
        HandUI.Instance.RemoveCard(entity, isController);
        BoardUI.Instance.PutOnTopMogi(entity, (MogiEntity)mogi, isController);
    }

    public async void Dissolving(Action complete = null)
    {
        await new WaitForSeconds(1f);
        CardBehaviour.Instance.RemoveCard(entity);
        complete?.Invoke();
        EffectManager.Instance.Instantiate("Bonus_is_used", cardTran.position);
        Destroy(gameObject);
    }
    
    public async void MogiAttackAnimation(CardMotion target,Action onHitTarget)
    {
        curStatus = status.attacking;
        render.sortingOrder = 1000;
        Vector3 startPosition = transform.position;

        float timeCouter = 0;
        float distanceToTarget = 0;
        bool hadVibrate = false;

        while (timeCouter < 1)
        {
            timeCouter += Time.deltaTime * 2;
            if (timeCouter > 1) timeCouter = 1;
            float lerp = animation.Evaluate(timeCouter);            
            cardTran.position = Vector3.Lerp(startPosition, target.cardTran.position, lerp);

            if (!hadVibrate && distanceToTarget > Vector3.Distance(cardTran.position, target.cardTran.position))
            {
                hadVibrate = true;
                onHitTarget?.Invoke();
                target.Vibrate();
            }

            distanceToTarget = Vector3.Distance(cardTran.position, target.cardTran.position);
            await new WaitForUpdate();
        }

        render.sortingOrder = normalsortingOrder;
        cardTran.position = startPosition;
        curStatus = status.inSlot;
    }

    //whne under attack
    public async void Vibrate()
    {
        float timeCouter = 0.16f;
        Vector2 startPosition = cardTran.position;
        float frequency = 0.01f;

        while (timeCouter > 0)
        {
            timeCouter -= frequency;
            await new WaitForSeconds(frequency);
            cardTran.position = startPosition + UnityEngine.Random.insideUnitCircle*0.1f;
            await new WaitForUpdate();
        }

        cardTran.position = startPosition;
    }

    private void MogiDeadEffect()
    {
        
    }
}
