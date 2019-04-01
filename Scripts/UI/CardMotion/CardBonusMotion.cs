using System;
using UnityEngine;

public class CardBonusMotion : CardMotion {

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
        EffectManager.Instance.Instantiate("BonusIsUsed", cardTran.position);
        Destroy(gameObject);
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
}
