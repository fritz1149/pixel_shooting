using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S23 : Buff
{
    public override void Add(PlayerController PlayerC)
    {
        PlayerC.Defend=3;   
    }
}
