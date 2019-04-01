using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardMogiMotion : CardMotion
{
    public new AnimationCurve animation;

    private bool attacking;

    public override void Init(CardEntity entity, bool isController)
    {
        base.Init(entity, isController);
        ((MogiEntity)entity).AddDeadEvent(MogiDeadEffect);
        ((MogiEntity)entity).AddTakeDamageEvent(UnderAttackEffect);
    }

    private async void MogiDeadEffect()
    {
        if (curStatus == status.inSlot)
        {
            await new WaitForSeconds(0.16f);
        }
        else if(curStatus == status.attacking)
        {
            await new WaitUntil(() => curStatus == status.inSlot);
        }

        EffectManager.Instance.Instantiate("MogiCardDead", cardTran.position);
        Destroy(gameObject);
    }

    public async void MogiAttackAnimation(CardMogiMotion target, Action onHitTarget)
    {
        curStatus = status.attacking;
        render.sortingOrder = 1000;
        Vector3 startPosition = transform.position;

        float timeCouter = 0;
        float distanceToTarget = 0;
        bool hitTarget = false;
        Vector3 targetPos = target.cardTran.position;

        while (timeCouter < 1)
        {
            timeCouter += Time.deltaTime * 4;
            if (timeCouter > 1) timeCouter = 1;
            float lerp = animation.Evaluate(timeCouter);
            cardTran.position = Vector3.Lerp(startPosition, targetPos, lerp);

            if (!hitTarget && distanceToTarget > Vector3.Distance(cardTran.position, targetPos))
            {
                hitTarget = true;
                onHitTarget?.Invoke();
            }

            distanceToTarget = Vector3.Distance(cardTran.position, targetPos);
            await new WaitForUpdate();
        }

        render.sortingOrder = normalsortingOrder;
        cardTran.position = startPosition;
        curStatus = status.inSlot;
    }

    //whne under attack
    //atacker not be effect
    public async void UnderAttackEffect()
    {
        if (!isController && !TurnManager.Instance.isOpponentTurn
            || isController && TurnManager.Instance.isOpponentTurn)
        {
            float timeCouter = 0.16f;
            Vector2 startPosition = cardTran.position;
            float frequency = 0.01f;

            while (timeCouter > 0)
            {
                timeCouter -= frequency;
                await new WaitForSeconds(frequency);
                if (cardTran != null) cardTran.position = startPosition + UnityEngine.Random.insideUnitCircle * 0.1f;
                else return;
                await new WaitForUpdate();
            }

            cardTran.position = startPosition;
        }
    }
}
