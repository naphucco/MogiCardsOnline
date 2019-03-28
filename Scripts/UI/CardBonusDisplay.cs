using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBonusDisplay : CardDisplay {

    protected new SpriteAnimination animation;
    private Sprite[] sprite;
        
    public override void Init(string cardName)
    {
        base.Init(cardName);
        animation = GetComponent<SpriteAnimination>();
        sprite = CardData.Instance.GetBonus(cardName, false).display;
    }

    public override void ShowFrontOfCard()
    {
        base.ShowFrontOfCard();
        animation.sprites = sprite;
        animation.enabled = true;
    }
}