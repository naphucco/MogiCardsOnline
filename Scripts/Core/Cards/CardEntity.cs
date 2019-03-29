using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardEntity : MonoBehaviour
{
    public abstract CardDisplay display { get; set; }
    public abstract CardMotion motion { get; set; }
    public abstract Card info { get; set; }

    public abstract void Init(Card info, CardDisplay display, CardMotion motion);
}
