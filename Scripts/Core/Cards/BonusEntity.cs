using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusEntity : CardEntity
{
    public override CardDisplay display { get; set; }
    public override CardMotion motion { get; set; }
    public override Card info { get; set; }

    public override void Init(Card info, CardDisplay display, CardMotion motion)
    {
        this.display = display;
        this.motion = motion;
        this.info = info;
    }
}
