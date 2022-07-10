using System.Collections;
using UnityEngine;
using LitJson;
using System.IO;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.Threading;
public class JGE
{
    public string level,speed,MoveTime,MoveInterval,AttackRadius,AttackInterval,DiscoverRadius,BulletID,Health,name;
}
public class GunEnemy : Enemy
{
    public int BulletID;
    //引用
    public GameObject Bullet2;
    protected void Awake()
    {
        PlayerL=LayerMask.GetMask("Player");
        Bullet1L=LayerMask.GetMask("Bullet1");
        Player=GameObject.FindGameObjectWithTag("Player");
        Hurt=GameObject.FindGameObjectWithTag("Hurt");
        PlayerC=Player.GetComponent<PlayerController>();
        UM=GameObject.FindGameObjectWithTag("UM").GetComponent<UIManager>();
        Bullet2=Instantiate(GameObject.FindGameObjectWithTag("Bullet2"),new Vector2(227,-57),transform.localRotation);
        Bullet2.GetComponent<Bullet2>().ReadJson(BulletID);
        Timer1=Timer2=0.0f;
        ReadJson();
        InitRandom();
    }
    public override void ReadJson()
    {
        StreamReader streamreader = new StreamReader(Application.dataPath + "/Data/GunEnemy.txt");   //读取数据，转换成数据流
        JsonReader js = new JsonReader(streamreader);                                               //转换成json数据
        List<JGE> L=JsonMapper.ToObject<List<JGE>>(js);
        level=int.Parse(L[id].level);  
        speed=float.Parse(L[id].speed);            
        MoveTime=float.Parse(L[id].MoveTime);
        MoveInterval=float.Parse(L[id].MoveInterval);                                
        AttackRadius=float.Parse(L[id].AttackRadius);
        AttackInterval=float.Parse(L[id].AttackInterval);                                            
        DiscoverRadius=float.Parse(L[id].DiscoverRadius);
        Health=int.Parse(L[id].Health);
        BulletID=int.Parse(L[id].BulletID);
        name=L[id].name;
        OrigHealth=Health;
        PayBack=Health*2;
    }
    public void OnDrawGizmos()
    {
        //Gizmos.DrawWireSphere(transform.localPosition,AttackRadius);
    }
}
