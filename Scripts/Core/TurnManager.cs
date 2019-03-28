using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour {

    private static TurnManager instance = null;

    public static TurnManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<TurnManager>();
            }
            return instance;
        }
    }

    public bool isOpponentTurn { get; private set; }

    private Action<bool> onSwitchTurn;

    public void AddSwitchTurnEvent(Action<bool> switchEvent)
    {
        onSwitchTurn += switchEvent;
    }

    public void SwitchTurn()
    {
        isOpponentTurn = !isOpponentTurn;
        onSwitchTurn?.Invoke(isOpponentTurn);

        if (isOpponentTurn)
        {
            Game.Instance.opponent.activity.StartTurn();
        }
        else
        {
            Game.Instance.controller.activity.StartTurn();
        }
    }   
}