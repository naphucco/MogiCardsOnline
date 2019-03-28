using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerBehaviour : MonoBehaviour
{
    //Use 1 unique instance
    private static ControllerBehaviour instance = null;

    public static ControllerBehaviour Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<ControllerBehaviour>();
            }
            return instance;
        }
    }

    public List<CardMotion> cardMotions = new List<CardMotion>();

    private CardMotion selectingCard;

    public void AddNewCard(CardMotion card)
    {
        cardMotions.Add(card);
    }

    public List<CardMotion> CardMotions(bool isController)
    {
        List<CardMotion> cardMotions = new List<CardMotion>();

        for (int i = 0; i < this.cardMotions.Count; i++)
        {
            if (this.cardMotions[i].isController == isController)
            {
                cardMotions.Add(this.cardMotions[i]);
            }
        }

        return cardMotions;
    }

    public void RemoveCard(CardMotion card)
    {
        cardMotions.Remove(card);
    }

    private void SelectCard()
    {
        if (cardMotions.Count > 0)
        {
            if (TurnManager.Instance.isOpponentTurn)
            {
                selectingCard = null;
            }
            else
            {
                if (Input.GetMouseButton(0))
                {
                    if (selectingCard == null)
                    {
                        if (!MotionManager.running)
                        {
                            var mousePos = Input.mousePosition;
                            mousePos.z = -Camera.main.transform.position.z; // select distance = 10 units from the camera
                            Vector3 choosePos = Camera.main.ScreenToWorldPoint(mousePos);
                            choosePos.z = 0;

                            //Priority for the card is added later
                            //Select only one card
                            for (int i = HandUI.Instance.cardInControllerHand.Count - 1; i >= 0; i--)
                            {
                                if (selectingCard == null)
                                {
                                    if (HandUI.Instance.cardInControllerHand[i].render.bounds.Contains(choosePos))
                                    {
                                        selectingCard = HandUI.Instance.cardInControllerHand[i];
                                        selectingCard.Selecting();
                                        break;
                                    }
                                }
                            }
                            
                            for (int i = 0; i < BoardUI.Instance.controllerSlotUIs.Length; i++)
                            {
                                if (selectingCard == null)
                                {                                    
                                    if (BoardUI.Instance.controllerSlotUIs[i].cardMotion != null)
                                    {
                                        CardMotion card = BoardUI.Instance.controllerSlotUIs[i].cardMotion;

                                        if (card.render.bounds.Contains(choosePos))
                                        {
                                            selectingCard = BoardUI.Instance.controllerSlotUIs[i].cardMotion;
                                            selectingCard.Selecting();
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        selectingCard.Selecting();
                    }
                }
                else
                {
                    selectingCard = null;
                }
            }

            for (int i = 0; i < cardMotions.Count ; i++)
            {
                if (cardMotions[i] != selectingCard)
                {
                    cardMotions[i].DeSelecting();
                }
            }
        }
    }

    #region Unity

    private void Update()
    {
        SelectCard();
    }

    #endregion
}
