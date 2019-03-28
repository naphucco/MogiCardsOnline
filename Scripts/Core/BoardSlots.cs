using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class BoardSlots
{
    public int number = 5;
    public BoardSlot[] slots;

    public void Clear()
    {
        slots = new BoardSlot[number];
    }
}