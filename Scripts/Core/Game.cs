using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {

    private static Game instance = null;

    public static Game Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<Game>();
            }
            return instance;
        }
    }

    public enum GameType { botMatch }

    //we have 2 player in game
    public Player controller;
    public Player opponent;
    public GameType gameType = GameType.botMatch;
    
    //reapare for battle
    private void RepareBoard()
    {        
        controller.Reset();
        opponent.Reset();
        controller.activity.StartTurn();
    }
    
    #region Unity function

    private void Start()
    {
        RepareBoard();
    }
    
    //test
    /*
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            controller.activity.GetMogiCard(2);
        }
    }*/

    #endregion
}
