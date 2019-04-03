using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMogiAttack : StateMachineBehaviour {

    

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

        //have no mogi to attack        
        if (BoardUI.Instance.MogiInBoardNotAttacked(false).Count == 0)
        {
            animator.SetInteger("action", 5);
        }
        //attack direct controller
        else if (BoardUI.Instance.MogiInBoard(true).Count == 0)
        {
            //not done yet
            animator.SetInteger("action", 5);
        }
        else
        {
            AttackControllerMogi(animator);
        }
    }

    private async void AttackControllerMogi(Animator animator)
    {
        //stupid AI
        MogiEntity attackCard = WeakestCard(false);
        MogiEntity targetCard = WeakestCard(true);

        AIBehaviour.Instance.inAction = true;

        ((CardMogiMotion)attackCard.motion)
            .MogiAttackAnimation((CardMogiMotion)targetCard.motion, () =>
        {
            AIBehaviour.Instance.inAction = false;
            attackCard.Attack(targetCard);
        });

        await new WaitUntil(() => AIBehaviour.Instance.inAction);
        await new WaitForSeconds(1);

        animator.SetInteger("action", 4);
    }
    
    private MogiEntity WeakestCard(bool isController)
    {
        MogiEntity card = null;

        List<MogiEntity> cards = BoardUI.Instance.MogiInBoardNotAttacked(isController);

        for (int i = 0; i < cards.Count; i++)
        {
            if (card == null || cards[i].attackPoint < card.attackPoint)
            {
                card = cards[i];
            }
        }

        return card;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
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
 