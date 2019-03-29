using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMoveCardToBoard : StateMachineBehaviour {

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

        animator.SetInteger("action", -1); //to not auto OnStateEnter
        //stupid ai
        if (Game.Instance.opponent.activity.actionPoints == 0 || //not have action point
            HandUI.Instance.cardInOpponentHand.Count == 0) //not have card in hand            
        {
            ChangeToAttackState(animator);
        }
        else
        {
            UseCardInHand(animator);
        }
    }

    private void ChangeToAttackState(Animator animator)
    {
        //change to attack
        animator.SetInteger("action", 3);
    }

    private async void UseCardInHand(Animator animator)
    {
        await new WaitForSeconds(1);

        //get useable card
        List <CardEntity> allCardInHand = HandUI.Instance.cardInOpponentHand;
        List<MogiEntity> mogiInBoards = BoardUI.Instance.MogiInBoard(false);
        List<CardEntity> useableCard = new List<CardEntity>();

        //put card to slot
        //or top of mogi
        if (BoardUI.Instance.HaveEmptySlot(false))
        {
            for (int i = 0; i < allCardInHand.Count; i++)
            {
                Card cardData = CardData.Instance.GetCard(allCardInHand[i].name, false);

                if (cardData.manaCost <= Game.Instance.opponent.activity.actionPoints)
                {
                    //can't use if card need put on top of mogi
                    //but not have ani mogi card
                    if (mogiInBoards.Count > 0 || cardData.putOn != Card.PutOn.topOfMogi)
                    {
                        useableCard.Add(allCardInHand[i]);
                    }
                }
            }
        }
        //top of mogi only(because not have empty slot)
        else
        {
            for (int i = 0; i < allCardInHand.Count; i++)
            {
                Card cardData = CardData.Instance.GetCard(allCardInHand[i].name, false);

                if (cardData.manaCost <= Game.Instance.opponent.activity.actionPoints)
                {
                    if (cardData.putOn == Card.PutOn.topOfMogi)
                    {
                        useableCard.Add(allCardInHand[i]);
                    }
                }
            }
        }

        if (useableCard.Count > 0)
        {
            CardMotion card = useableCard[Random.Range(0, useableCard.Count)].motion;
            Card cardData = CardData.Instance.GetCard(card.name, false);

            if (cardData.putOn == Card.PutOn.emptySlot)
            {
                card.HandToOpponentSlot(BoardUI.Instance.GetFirstEmptySlot(false), () =>
                {
                    AIBehaviour.Instance.inAction = false;
                    //repeat
                    animator.SetInteger("action", 2);
                });
            }
            else
            {
                card.HandToOpponentTopMogi(mogiInBoards[Random.Range(0, mogiInBoards.Count)], () =>
                {
                    //repeat
                    animator.SetInteger("action", 2);
                });
            }
        }
        else
        {
            ChangeToAttackState(animator);
        }
    }

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	//override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //    if (!AIBehaviour.Instance.inAction)
    //    {

    //    }
	//}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	//override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
