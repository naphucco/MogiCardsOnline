using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIRepareTurn : StateMachineBehaviour {

    private bool preparing;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

        preparing = true;
        AIBehaviour.Instance.inAction = true;

        if (HandUI.Instance.MogisInHand(false).Count > HandUI.Instance.cardInOpponentHand.Count)
        {
            //Avoid draw too many mogi
            PileUI.Instance.DrawCard(false, 1, true, Card.Type.bonus, () => { AIBehaviour.Instance.inAction = false; });
        }
        else
        {
            PileUI.Instance.DrawCard(false, 1, true, Card.Type.unknow, () => { AIBehaviour.Instance.inAction = false; });
        }
        
        //test
        /*
        PileUI.Instance.DrawCard("Beech", false, () => 
        {
            PileUI.Instance.DrawCard("DrawLv1", false, () =>
             {
                 AIBehaviour.Instance.inAction = false;
             });
        });*/
    }

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

        if (!AIBehaviour.Instance.inAction && preparing)
        {
            //switch to use card            
            preparing = false;//because animator not swtich instantly
            animator.SetInteger("action", 1);
        }
	}

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
