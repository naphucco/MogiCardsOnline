﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDisplay : MonoBehaviour {

    public SpriteRenderer render;

    public virtual void Init(string cardName)
    {
        render = GetComponent<SpriteRenderer>();
    }

    public virtual void ShowFrontOfCard()
    {

    }

    public virtual void RotationComplete()
    {

    }

    protected virtual void SelectCard()
    {
        if (!MotionManager.running)
        {
            Debug.Log("selectin");
        }
    }
}
