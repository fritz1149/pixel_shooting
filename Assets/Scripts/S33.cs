using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S33 : Buff
{
    public override void Add(PlayerController PlayerC)
    {
        PlayerC.Storm=true;
    }
}
