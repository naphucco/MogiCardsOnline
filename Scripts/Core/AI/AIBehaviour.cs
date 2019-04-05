using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//manager auto behaviour of ai
public class AIBehaviour : MonoBehaviour {

    private static AIBehaviour instance = null;

    public static AIBehaviour Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<AIBehaviour>();
            }
            return instance;
        }
    }

    public bool inAction { get; set; }

    private Animator behaviourController;
    private bool isAITurn;
    
    private void UpdateBehaviour()
    {
        if (!isAITurn)
        {
            if (TurnManager.Instance.isOpponentTurn)
            {
                isAITurn = true;
                GetComponent<Animator>().enabled = true;
                GetComponent<Animator>().SetInteger("action", 0);
            }
        }
        else
        {
            if (!TurnManager.Instance.isOpponentTurn)
            {
                isAITurn = false;
                GetComponent<Animator>().enabled = false;
                GetComponent<Animator>().SetInteger("action", 7);
            }
        }
    }

    #region Unity    

    // Update is called once per frame
    private void Update ()
    {
        UpdateBehaviour();
	}
    
    #endregion
}
