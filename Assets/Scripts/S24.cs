using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S24 : Buff
{
    public override void Add(PlayerController PlayerC)
    {
        PlayerC.Stone();
    }
}
