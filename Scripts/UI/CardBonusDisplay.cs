using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBonusDisplay : CardDisplay {

    protected SpriteAnimination ani;
    private Sprite[] sprite;
        
    public override void Init(string cardName)
    {
        base.Init(cardName);
        ani = GetComponent<SpriteAnimination>();
        sprite = CardData.Instance.GetBonus(cardName, false).display;
    }

    public override void ShowFrontOfCard()
    {
        base.ShowFrontOfCard();
        ani.sprites = sprite;
        ani.enabled = true;
    }
}