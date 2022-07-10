using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S03 : Buff
{
    public override void Add(PlayerController PlayerC)
    {
        PlayerC.Fly=true;
    }
}
