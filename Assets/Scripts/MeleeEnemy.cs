using System.Collections;
using UnityEngine;
using LitJson;
using System.IO;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.Threading;
using UnityEngine.UI;
public class JME
{
    public string level,speed,MoveTime,MoveInterval,AttackInterval,AttackRadius,DiscoverRadius,CheckAngle,Damage,Health,name;
}
public class MeleeEnemy : Enemy
{
    public float CheckAngle;
    public int Damage;
    //引用
    public Text health;
    protected void Awake()
    {
        PlayerL=LayerMask.GetMask("Player");
        Bullet1L=LayerMask.GetMask("Bullet1");
        Player=GameObject.FindGameObjectWithTag("Player");
        Hurt=GameObject.FindGameObjectWithTag("Hurt");
        PlayerC=Player.GetComponent<PlayerController>();
        UM=GameObject.FindGameObjectWithTag("UM").GetComponent<UIManager>();
        Timer1=Timer2=0.0f;
        health=GameObject.FindGameObjectWithTag("health").GetComponent<Text>();
        ReadJson();
        InitRandom();
    }
    public override void ReadJson()
    {
        StreamReader streamreader = new StreamReader(Application.dataPath + "/Data/MeleeEnemy.txt");   //读取数据，转换成数据流
        JsonReader js = new JsonReader(streamreader);                                               //转换成json数据
        List<JME> L=JsonMapper.ToObject<List<JME>>(js);   
        level=int.Parse(L[id].level);    
        speed=float.Parse(L[id].speed);
        MoveTime=float.Parse(L[id].MoveTime);
        MoveInterval=float.Parse(L[id].MoveInterval);
        AttackInterval=float.Parse(L[id].AttackInterval);                     
        AttackRadius=float.Parse(L[id].AttackRadius);     
        DiscoverRadius=float.Parse(L[id].DiscoverRadius);
        CheckAngle=float.Parse(L[id].CheckAngle);                    
        Damage=int.Parse(L[id].Damage);
        Health=int.Parse(L[id].Health);
        name=L[id].name;
        OrigHealth=Health;
        PayBack=Health*2;
    }
    protected Vector2 PlayerDir(Vector2 LocalPos)
    {
        Vector2 PlayerPos=new Vector2(Player.transform.localPosition.x,Player.transform.localPosition.y);
        float Dis=Vector2.Distance(LocalPos,PlayerPos);
        return (PlayerPos-LocalPos)/Dis;
    }
    protected override void Attack()
    {
        Vector2 LocalPos=new Vector2(transform.localPosition.x,transform.localPosition.y),front=Front();
        Timer2+=Time.deltaTime;
        if(Timer2>=AttackInterval)
        {
            Timer2=0.0f;
            if(Vector2.Angle(front,PlayerDir(LocalPos))<=CheckAngle&&(!PlayerC.Miss||rd.Next(0,3)!=0))
                if(Player.layer!=16)
                    PlayerC.GetHurt(Damage-PlayerC.Defend);
            MoveInit();
        }
    }
    public void OnDrawGizmos()
    {
        //Gizmos.DrawWireSphere(transform.localPosition,AttackRadius);
    }
}
