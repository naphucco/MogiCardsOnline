using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardMogiDisplay : CardDisplay
{
    public SpriteAnimination ani;
    public SpriteRenderer animationRender;
    public SpriteRenderer bgRender;
    public Sprite frontBG;
    public GameObject targetedEffect;

    private Sprite[] sprite;
    private bool hadShowFront = false;

    public override void Init(string cardName)
    {
        base.Init(cardName);
        sprite = CardData.Instance.GetMogi(cardName, false).display;
    }

    public void Targeted()
    {
        targetedEffect.SetActive(true);
    }

    public void StopTargeted()
    {
        targetedEffect.SetActive(false);
    }

    public override void ShowFrontOfCard()
    {
        base.ShowFrontOfCard();
        bgRender = GetComponent<SpriteRenderer>();
        bgRender.sprite = frontBG;

        animationRender.flipX = true;
        animationRender.enabled = true;
        ani.sprites = sprite;
        ani.enabled = true;
        hadShowFront = true;
    }

    public override void ShowFrontComplete()
    {
        base.ShowFrontComplete();
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
