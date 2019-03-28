using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MogisPileUI : MonoBehaviour
{
    private static MogisPileUI instance = null;

    public static MogisPileUI Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<MogisPileUI>();
            }
            return instance;
        }
    }

    private void OnMouseDown()
    {
        //draw one card per mopuse down
        if (!TurnManager.Instance.isOpponentTurn) PileUI.Instance.DrawCard(true, 1, true, Card.Type.mogi);
    }
}