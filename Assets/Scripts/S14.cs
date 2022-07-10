using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S14 : Buff
{
    public override void Add(PlayerController PlayerC)
    {
        PlayerC.gameObject.layer=16;
        PlayerC.ghost();
    }
}
