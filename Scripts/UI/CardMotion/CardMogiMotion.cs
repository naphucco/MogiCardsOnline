using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardMogiMotion : CardMotion
{
    public AnimationCurve ani;

    private bool attacking;

    public override void Init(CardEntity entity)
    {
        base.Init(entity);
        entity.AddDiscardEvent(MogiDeadEffect);
        ((MogiEntity)entity).AddTakeDamageEvent(UnderAttackEffect);
    }

    private async void MogiDeadEffect()
    {
        if (entity.curStatus == CardEntity.status.inSlot)
        {
            await new WaitForSeconds(0.16f);
        }
        else if(entity.curStatus == CardEntity.status.attacking)
        {
            await new WaitUntil(() => entity.curStatus == CardEntity.status.inSlot);
        }

        EffectManager.Instance.Instantiate("MogiCardDead", cardTran.position);
        Destroy(gameObject);
    }

    public async void MogiAttackAnimation(CardMogiMotion target, Action onHitTarget)
    {
        entity.curStatus = CardEntity.status.attacking;
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
            float lerp = ani.Evaluate(timeCouter);
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
        entity.curStatus = CardEntity.status.inSlot;
    }

    //whne under attack
    //atacker not be effect
    public async void UnderAttackEffect()
    {
        if (!entity.isController && !TurnManager.Instance.isOpponentTurn
            || entity.isController && TurnManager.Instance.isOpponentTurn)
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
