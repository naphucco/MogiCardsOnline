using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//conatin all data about card
public class CardData : MonoBehaviour {

    //Use 1 unique instance
    private static CardData instance = null;

    public static CardData Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<CardData>();
            }
            return instance;
        }
    }

    public MogiCard[] allMogi;
    public BonusCard[] allBonus;

    //copy instance
    public MogiCard GetMogi(string cardName, bool getInstance)
    {
        for (int i = 0; i < allMogi.Length; i++)
        {
            if (cardName == allMogi[i].name)
            {
                if (getInstance)
                {
                    return allMogi[i].Clone();
                }
                else
                {
                    return allMogi[i];
                }
            }
        }

        return null;
    }

    public Card GetCard(string cardName,bool getInstance)
    {
        Card card = null;
        card = GetMogi(cardName, getInstance);
        if (card == null) card = GetBonus(cardName, getInstance);
        return card;
    }

    public BonusCard GetBonus(string cardName, bool getInstance)
    {
        for (int i = 0; i < allBonus.Length; i++)
        {
            if (cardName == allBonus[i].name)
            {
                if (getInstance)
                {
                    return allBonus[i].Clone();
                }
                else
                {
                    return allBonus[i];
                }
            }
        }

        return null;
    }
}
