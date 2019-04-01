using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBehaviour : MonoBehaviour
{
    //Use 1 unique instance
    private static CardBehaviour instance = null;

    public static CardBehaviour Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<CardBehaviour>();
            }
            return instance;
        }
    }

    public List<CardEntity> cardMotions = new List<CardEntity>();

    private CardEntity selectingCard;

    public void AddNewCard(CardEntity card)
    {
        cardMotions.Add(card);

        if (card.motion.isController)
        {
            card.motion.AddDeselectCardEvent(ControllerBehaviour.Instance.OnDeselectCard);
            card.motion.AddSelectingCardEvent(ControllerBehaviour.Instance.OnSelectingCard);
            card.motion.AddSelectCardEvent(ControllerBehaviour.Instance.OnSelectCard);
        }
    }
    
    public void RemoveCard(CardEntity card)
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
                                    if (HandUI.Instance.cardInControllerHand[i].motion.render.bounds.Contains(choosePos))
                                    {
                                        selectingCard = HandUI.Instance.cardInControllerHand[i];
                                        selectingCard.motion.Selecting();
                                        break;
                                    }
                                }
                            }
                            
                            for (int i = 0; i < BoardUI.Instance.controllerSlotUIs.Length; i++)
                            {
                                if (selectingCard == null)
                                {                                    
                                    if (BoardUI.Instance.controllerSlotUIs[i].cardEntity != null)
                                    {
                                        CardEntity card = BoardUI.Instance.controllerSlotUIs[i].cardEntity;

                                        if (card.motion.render.bounds.Contains(choosePos))
                                        {
                                            selectingCard = BoardUI.Instance.controllerSlotUIs[i].cardEntity;
                                            selectingCard.motion.Selecting();
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        selectingCard.motion.Selecting();
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
                    cardMotions[i].motion.DeSelecting();
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
