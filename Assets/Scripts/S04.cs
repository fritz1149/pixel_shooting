using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S04 : Buff
{
    public override void Add(PlayerController PlayerC)
    {
        PlayerC.Tornado=true;
    }
}
