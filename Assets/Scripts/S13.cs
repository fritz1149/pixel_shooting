using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S13 : Buff
{
    public override void Add(PlayerController PlayerC)
    {
        PlayerC.IntervalCut=2;
    }
}
