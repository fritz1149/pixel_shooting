using System.Collections;
using UnityEngine;
using LitJson;
using System.IO;
using System.Collections.Generic;
public class DevilBubbleWithGun : GunEnemy
{
    protected override void Attack()
    {
        Timer2+=Time.deltaTime;
        if(Timer2>=AttackInterval)
        {
            Timer2=0.0f;
            Instantiate(Bullet2,transform.localPosition,transform.localRotation).GetComponent<Bullet2>().V=Front();
            MoveInit();
        }
    }
}
