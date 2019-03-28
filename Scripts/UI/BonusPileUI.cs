using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusPileUI : MonoBehaviour
{
    private static BonusPileUI instance = null;

    public static BonusPileUI Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<BonusPileUI>();
            }
            return instance;
        }
    }
    
    private void OnMouseDown()
    {
        //draw one card per mopuse down
        if (!TurnManager.Instance.isOpponentTurn) PileUI.Instance.DrawCard(true, 1, true, Card.Type.bonus);
    }
}