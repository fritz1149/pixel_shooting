using System.Collections;
using UnityEngine;
using LitJson;
using System.IO;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using System.Diagnostics;
using System.Threading;
public class JB1
{
    public string speed,DiscoverRadius,CheckRadius,Damage,name;
}
public class Bullet1 : MonoBehaviour
{
    public float speed,CheckRadius;
    public bool hitEdge,hitEnemy,Heavy;
    public int Damage,iRoot;
    public byte [] buffer;
    public System.Random rd;
    //传值
    public Vector2 V;
    public PlayerController PlayerC;
    //引用
    public LayerMask Edge,Edge_,Enemy;
    void Awake()
    {
        hitEdge=hitEnemy=false;
        Edge=LayerMask.GetMask("Edge");
        Edge_=LayerMask.GetMask("Edge_");
        Enemy=LayerMask.GetMask("Enemy");
        PlayerC=GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        InitRandom();
    }
    void FixedUpdate()
    {
        Move();
        HitCheck();
    }
    void InitRandom()
    {
        buffer=Guid.NewGuid().ToByteArray();
        iRoot=BitConverter.ToInt32(buffer,0);
        rd=new System.Random(iRoot);
    }
    public void ReadJson(int id)
    {
        StreamReader streamreader = new StreamReader(Application.dataPath + "/Data/Bullet1.txt");
        JsonReader js = new JsonReader(streamreader);
        List<JB1> L=JsonMapper.ToObject<List<JB1>>(js);
        speed=float.Parse(L[id].speed);
        CheckRadius=float.Parse(L[id].CheckRadius);
        Damage=int.Parse(L[id].Damage);
        name=L[id].name;
    }
    void Move()
    {
        transform.Translate(V.x*speed,V.y*speed,0);
    }
    void HitCheck()
    {
        hitEnemy=Physics2D.OverlapCircle(transform.localPosition,CheckRadius,Enemy);
        hitEdge=(Physics2D.OverlapCircle(transform.localPosition,CheckRadius,Edge)||Physics2D.OverlapCircle(transform.localPosition,CheckRadius,Edge_));
        if(hitEnemy)
        {
            Collider2D[] GetHit=Physics2D.OverlapCircleAll(transform.localPosition,CheckRadius,Enemy);
            if(Heavy) 
            {
                GetHit[0].GetComponent<Enemy>().dizzy=true;
                GetHit[0].GetComponent<Enemy>().Fly=PlayerC.Fly?V*0.3f:new Vector2(0,0);
            }
            GetHit[0].GetComponent<Enemy>().GetHurt(Damage+PlayerC.DamagePlus);
            if(PlayerC.Lightening&&rd.Next(0,3)==0)
            {
                foreach(Collider2D BeHit in GetHit)
                    BeHit.GetComponent<Enemy>().GetHurt(3);
            }
            Destroy(gameObject);
        }
        if(hitEdge) Destroy(gameObject);
    }
    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.localPosition,CheckRadius);
    }
}
