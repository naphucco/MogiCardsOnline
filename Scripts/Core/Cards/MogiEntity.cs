using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MogiEntity : MonoBehaviour
{
    public int HPRemain;
    public int maxHP;
    public int attackPoint;

    public void Init(MogiCard info)
    {
        attackPoint = info.attackPoint;
        HPRemain = maxHP = info.hp;
    }
}