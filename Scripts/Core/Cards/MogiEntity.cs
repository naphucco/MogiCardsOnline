using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MogiEntity : CardEntity
{
    public override CardDisplay display { get; set; }
    public override CardMotion motion { get; set; }
    public override Card info { get; set; }

    public int HPRemain;
    public int maxHP;
    public int attackPoint;

    private Action onDead, onTakeDamage;

    public void Fight(MogiEntity target)
    {
        target.TakeDamage(attackPoint);
        this.TakeDamage(target.attackPoint);
    }

    public void TakeDamage(int attackPoint)
    {
        HPRemain -= attackPoint;
        onTakeDamage?.Invoke();
        if (HPRemain <= 0) onDead?.Invoke();
    }

    public void AddTakeDamageEvent(Action takeDamageEvent)
    {
        this.onTakeDamage += takeDamageEvent;
    }

    public void AddDeadEvent(Action deadEvent)
    {
        this.onDead += deadEvent;
    }

    public override void Init(Card info, CardDisplay display, CardMotion motion)
    {
        MogiCard cardInfo = (MogiCard)info;
        attackPoint = cardInfo.attackPoint;
        HPRemain = maxHP = cardInfo.hp;
        this.display = display;
        this.motion = motion;
        this.info = info;
    }
}