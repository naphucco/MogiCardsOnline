using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MogiEntity : CardEntity
{
    public override CardDisplay display { get; set; }
    public override CardMotion motion { get; set; }
    public override Card info { get; set; }
    public override bool isController { get; set; }
    public bool hadAttack { get; private set; }

    public int HPRemain;
    public int maxHP;
    public int attackPoint;
    
    private Action onTakeDamage;

    public void Attack(MogiEntity target)
    {
        hadAttack = true;
        target.TakeDamage(attackPoint);
        this.TakeDamage(target.attackPoint);
    }

    public void TakeDamage(int attackPoint)
    {
        HPRemain -= attackPoint;
        onTakeDamage?.Invoke();
        if (HPRemain <= 0) DisCard();
    }

    public void AddTakeDamageEvent(Action takeDamageEvent)
    {
        this.onTakeDamage += takeDamageEvent;
    }    

    public override void Init(Card info, CardDisplay display, CardMotion motion, bool isController)
    {        
        MogiCard cardInfo = (MogiCard)info;
        attackPoint = cardInfo.attackPoint;
        HPRemain = maxHP = cardInfo.hp;
        this.display = display;
        this.motion = motion;
        this.info = info;
        this.isController = isController;
        base.Init(info, display, motion, isController);
        AddInitEvent();
    }

    private void AddInitEvent()
    {
        TurnManager.Instance.AddSwitchTurnEvent((isController) => 
        {
            //reset attack
            if (isController == this.isController)
            {
                hadAttack = false;
            }
        });
    }
}