using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S11 : Buff
{
    public override void Add(PlayerController PlayerC)
    {
        PlayerC.speed=0.3f;
    }
}
