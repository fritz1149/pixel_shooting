using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S01 : Buff
{
    public override void Add(PlayerController PlayerC)
    {
        PlayerC.DamagePlus=2;
    }
}
