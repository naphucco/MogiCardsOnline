using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTurnBtn : MonoBehaviour {

    public int playerTurnAnimationID;

    private Animator animator;

    private void InitAnimation()
    {
        animator = GetComponent<Animator>();

        TurnManager.Instance.AddSwitchTurnEvent((isOpponentTurn) =>
        {
            if (isOpponentTurn)
            {
                animator.SetInteger("Display", 2);
            }
            else
            {
                animator.SetInteger("Display", 0);
            }
        });
    }

    private void SwitchToOpponentTurn()
    {
        if (!TurnManager.Instance.isOpponentTurn)
        {
            TurnManager.Instance.SwitchTurn();
        }
    }
    
    #region Unity

    private void OnMouseUp()
    {
        SwitchToOpponentTurn();
    }

    private void OnMouseDown()
    {
        if (!TurnManager.Instance.isOpponentTurn)
        {
            animator.SetInteger("Display", 1);
        }
    }

    private void Start()
    {
        InitAnimation();
    }
    

    #endregion
}
