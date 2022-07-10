using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eye : MeleeEnemy
{
    protected override void DangerEvent()
    {
        Damage=6;
        AttackInterval=0.3f;
        speed=0.2f;
    }
}
