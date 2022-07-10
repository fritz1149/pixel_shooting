using System.Collections;
using UnityEngine;
using LitJson;
using System.IO;
using System.Collections.Generic;
public class Rabbit1 : GunEnemy
{
    bool shoot1,shoot2;
    Vector2 front;
    protected override void DangerEvent(){speed=0.3f;}
    protected override void Attack()
    {
        Timer2+=Time.deltaTime;
        if(Timer2>=AttackInterval)
        {
            if(Timer2>=AttackInterval+0.4) 
            {
                Instantiate(Bullet2,transform.localPosition,transform.localRotation).GetComponent<Bullet2>().V=front;
                Timer2=0.0f;
                shoot1=shoot2=false;
                MoveInit();
            }
            else if(Timer2>=AttackInterval+0.2&&!shoot2) 
            {
                Instantiate(Bullet2,transform.localPosition,transform.localRotation).GetComponent<Bullet2>().V=front;
                shoot2=true;
            }
            else if(!shoot1) 
            {
                front=Front();
                Instantiate(Bullet2,transform.localPosition,transform.localRotation).GetComponent<Bullet2>().V=front;
                shoot1=true;
            }
        }
    }
}
