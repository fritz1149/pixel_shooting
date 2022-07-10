using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S21 : Buff
{
    public override void Add(PlayerController PlayerC)
    {
        PlayerC.HealthAdd=15;
    }
}
