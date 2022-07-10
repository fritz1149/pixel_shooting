using System.Collections;
using UnityEngine;
using LitJson;
using System.IO;
using System.Collections.Generic;
public class Rabbit2 : GunEnemy
{
    Vector2 [] V=new Vector2[12]
    {
        new Vector2(1,0),new Vector2(0.866f,0.5f),new Vector2(0.5f,0.866f),new Vector2(0,1),
        new Vector2(-0.5f,0.866f),new Vector2(-0.866f,0.5f),new Vector2(-1,0),new Vector2(-0.866f,-0.5f),
        new Vector2(-0.5f,-0.866f),new Vector2(0,-1),new Vector2(0.5f,-0.866f),new Vector2(0.866f,-0.5f)
    };
    protected override void DangerEvent(){Boom();Boom();}
    protected override void Attack()
    {
        Timer2+=Time.deltaTime;
        if(Timer2>=AttackInterval)
        {
            Timer2=0.0f;
            Boom();
            MoveInit();
        }
    }
    void Boom()
    {
        for(int i=0;i<12;i++)
            Instantiate(Bullet2,transform.localPosition,transform.localRotation).GetComponent<Bullet2>().V=V[i];
    }
    protected override void DeadEvent()
    {
        Boom();Boom();Boom();Boom();
    }
}
