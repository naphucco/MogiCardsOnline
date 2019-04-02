using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeTurnUI : MonoBehaviour {

    private Animator animator;
    private Image img;

    public void EndAnimation()
    {
        animator.Play("ChangeTurnUI", -1, 0f);
        animator.enabled = false;
        img.enabled = false;
    }

    private void Init()
    {
        animator = GetComponent<Animator>();
        img = GetComponent<Image>();

        TurnManager.Instance.AddSwitchTurnEvent((isOpponentTurn) =>
        {
            animator.enabled = true;
            img.enabled = true;
        });
    }

    private void Awake()
    {
        Init();
    }
}
