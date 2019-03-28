using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PileUI : MonoBehaviour {

    private static PileUI instance = null;

    public static PileUI Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<PileUI>();
            }
            return instance;
        }
    }

    public GameObject bonusCardPrefab;    
    public GameObject mogisCardPrefab;
    public Transform bonusPilePos;
    public Transform mogisPilePos;

    //for testing
    public async void DrawCard(string cardName, bool byController, Action complete = null)
    {
        if (!MotionManager.running)
        {
            //Pull out a series of cards at the same time
            string cardToDraw = null;

            if (byController)
            {
                cardToDraw = Game.Instance.controller.activity.GetCard(cardName);
            }
            else
            {
                cardToDraw = Game.Instance.opponent.activity.GetCard(cardName);
            }

            if (cardToDraw != null)
            {
                bool effectComplete = false;
                DrawCardEffect(cardToDraw, byController, () => { effectComplete = true; });
                await new WaitUntil(() => effectComplete == true);
                complete?.Invoke();
            }
            else
            {
                complete?.Invoke();
            }
        }
    }

    //to inherit
    public async void DrawCard(bool byController, int numberInQueue, bool getByTurn, Card.Type type, Action complete = null)
    {
        if (!MotionManager.running)
        {
            //Pull out a series of cards at the same time
            List<string> cardToDraws = null;

            if (byController)
            {
                if (type == Card.Type.bonus)
                {
                    cardToDraws = Game.Instance.controller.activity.GetBonusCard(numberInQueue, getByTurn);
                }
                else if (type == Card.Type.mogi)
                {
                    cardToDraws = Game.Instance.controller.activity.GetMogiCard(numberInQueue, getByTurn);
                }
                else //random
                {
                    int drawMogi = 0;

                    if (numberInQueue == 1)
                        drawMogi = UnityEngine.Random.Range(0, 2);
                    else
                        drawMogi = Mathf.CeilToInt(numberInQueue / 2f);//50% is mogi

                    cardToDraws = Game.Instance.controller.activity.GetMogiCard(drawMogi, getByTurn);
                    int drawBonus = numberInQueue - cardToDraws.Count;
                    cardToDraws.AddRange(Game.Instance.controller.activity.GetBonusCard(drawBonus, getByTurn));
                }
            }
            else
            {
                if (type == Card.Type.bonus)
                {
                    cardToDraws = Game.Instance.opponent.activity.GetBonusCard(numberInQueue, getByTurn);
                }
                else if (type == Card.Type.mogi)
                {
                    cardToDraws = Game.Instance.opponent.activity.GetMogiCard(numberInQueue, getByTurn);
                }
                else //random
                {
                    int drawMogi = 0;

                    if (numberInQueue == 1)
                        drawMogi = UnityEngine.Random.Range(0, 2);
                    else
                        drawMogi = Mathf.CeilToInt(numberInQueue / 2f);//50% is mogi

                    cardToDraws = Game.Instance.opponent.activity.GetMogiCard(drawMogi, getByTurn);
                    int drawBonus = numberInQueue - cardToDraws.Count;
                    cardToDraws.AddRange(Game.Instance.opponent.activity.GetBonusCard(drawBonus, getByTurn));
                }
            }

            if (cardToDraws.Count > 0)
            {
                while (cardToDraws.Count > 0)
                {
                    bool effectComplete = false;
                    DrawCardEffect(cardToDraws[0], byController, () => { effectComplete = true; });
                    cardToDraws.RemoveAt(0);
                    await new WaitUntil(() => effectComplete == true);
                }

                complete?.Invoke();
            }
            else
            {
                complete?.Invoke();
            }
        }
    }

    protected virtual void DrawCardEffect(string cardName, bool byController, Action conpleteEffect)
    {
        Vector3 pos = Vector3.zero;
        GameObject prefab = null;
        Card.Type type = CardData.Instance.GetCard(cardName, false).type;

        if (type == Card.Type.bonus)
        {
            pos = bonusPilePos.position;
            prefab = bonusCardPrefab;
        }
        else
        {
            pos = mogisPilePos.position;
            prefab = mogisCardPrefab;
        }
        
        GameObject cardObj = Instantiate(prefab, pos, Quaternion.identity);
        
        CardMotion motion = cardObj.GetComponent<CardMotion>();
        motion.Init(this, byController);
        motion.PileToHand(conpleteEffect);

        CardDisplay display = cardObj.GetComponent<CardDisplay>();
        display.Init(cardName);
        cardObj.name = cardName;
    }
}
