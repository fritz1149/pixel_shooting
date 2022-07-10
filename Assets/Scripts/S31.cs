using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S31 : Buff
{
    public override void Add(PlayerController PlayerC)
    {
        PlayerC.MagicAdd=100;
    }
}
