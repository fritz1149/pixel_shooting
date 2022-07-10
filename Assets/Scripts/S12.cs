using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S12 : Buff
{
    public override void Add(PlayerController PlayerC)
    {
        PlayerC.Miss=true;
    }
}
