using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S22 : Buff
{
    public override void Add(PlayerController PlayerC)
    {
        PlayerC.Recover=true;
    }
}
