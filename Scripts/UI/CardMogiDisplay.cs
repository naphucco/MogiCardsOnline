using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardMogiDisplay : CardDisplay
{
    public new SpriteAnimination animation;
    public SpriteRenderer animationRender;
    public SpriteRenderer bgRender;
    public Sprite frontBG;
    
    private Sprite[] sprite;
    private bool hadShowFront = false;

    public override void Init(string cardName)
    {
        base.Init(cardName);
        sprite = CardData.Instance.GetMogi(cardName, false).display;
    }

    public override void ShowFrontOfCard()
    {
        base.ShowFrontOfCard();
        bgRender = GetComponent<SpriteRenderer>();
        bgRender.sprite = frontBG;

        animationRender.flipX = true;
        animationRender.enabled = true;
        animation.sprites = sprite;
        animation.enabled = true;
        hadShowFront = true;
    }

    public override void RotationComplete()
    {
        base.RotationComplete();
        animationRender.flipX = false;
    }

    private void UpdateDisplay()
    {
        if (hadShowFront)
        {
            animationRender.sortingOrder = bgRender.sortingOrder + 1;
        }
    }

    #region Unity
    private void Update()
    {
        UpdateDisplay();
    }
    #endregion
}
