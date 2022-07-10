using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S34 : Buff
{
    public float x1;
    public int x2,x3;
    public override void Add(PlayerController PlayerC)
    {
        x1=PlayerC.speed;
        x2=PlayerC.IntervalCut;
        x3=PlayerC.HealthAdd;
        PlayerC.speed=0.35f;
        PlayerC.IntervalCut=4;
        PlayerC.HealthAdd=30;
        PlayerC.BefallS=this.GetComponent<S34>();
        PlayerC.Befall=true;
    }
}
