using System.Collections.Generic;

[System.Serializable]
public class Pile
{
    public DiscardPile discardPile;
    //There is no unlock card feature, and load card    
    public List<string> cardInPile;

    //get a copy of data
    public void CreateRandomPile(Card[] cardData)
    {
        discardPile.Clear();

        cardInPile = new List<string>();

        for (int i = 0; i < cardData.Length; i++)
        {
            cardInPile.Add(cardData[i].name);
        }

        cardInPile = Helper.Shuffle(cardInPile);
    }

    public string ForceDrawCard(string cardName)
    {
        if (cardInPile.Contains(cardName))
        {
            cardInPile.Remove(cardName);
            return cardName;
        }

        return null;
    }

    //get card from pile
    public List<string> DrawCard(int amount)
    {
        int count = 0;
        List<string> drawCards = new List<string>();

        if (cardInPile.Count < amount)
        {
            if (cardInPile.Count > 0)
            {
                count = cardInPile.Count;
                drawCards.AddRange(cardInPile);                
            }
            
            cardInPile.Clear();

            //get all card from discardPile to pile
            cardInPile = discardPile.TakeAllCard();

            while (count < amount)
            {
                if (cardInPile.Count > 0)
                {
                    drawCards.Add(cardInPile[0]);
                    cardInPile.RemoveAt(0);
                }

                count++;
            }

            return drawCards;
        }
        else
        {
            while (count < amount)
            {
                drawCards.Add(cardInPile[0]);
                cardInPile.RemoveAt(0);
                count++;
            }

            return drawCards;
        }
    }


}
