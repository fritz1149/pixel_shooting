using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S02 : Buff
{
    public override void Add(PlayerController PlayerC)
    {
        PlayerC.Heavy=true;
    }
}
