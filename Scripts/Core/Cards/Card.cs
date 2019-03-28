using UnityEngine;


public class Card : ScriptableObject
{
    public enum Type { mogi, bonus, unknow  }
    public enum PutOn { emptySlot, topOfMogi }

    public CardInfo cardInfo;
    public CardFeatures features; //to add by hand
    public int manaCost;
    public Type type;
    public PutOn putOn; //need empty slot to use
    public Sprite[] display;    
}