using System.Collections;
using UnityEngine;
using LitJson;
using System.IO;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using System.Diagnostics;
using System.Threading;
public class JB2
{
    public string speed,CheckRadius,Damage,name;
}
public class Bullet2 : MonoBehaviour
{
    public float speed,CheckRadius;
    public bool hitEdge,hitPlayer;
    public int Damage,iRoot;
    public byte [] buffer;
    public System.Random rd;
    //传值
    public Vector2 V;
    //引用
    public LayerMask Edge,Edge_,Player;
    public PlayerController PlayerC;
    public Text health;
    void Awake()
    {
        hitEdge=hitPlayer=false;
        Edge=LayerMask.GetMask("Edge");
        Edge_=LayerMask.GetMask("Edge_");
        Player=LayerMask.GetMask("Player");
        PlayerC=GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        health=GameObject.FindGameObjectWithTag("health").GetComponent<Text>();
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
        StreamReader streamreader = new StreamReader(Application.dataPath + "/Data/Bullet2.txt");   //读取数据，转换成数据流
        JsonReader js = new JsonReader(streamreader);                                               //转换成json数据
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
        hitPlayer=Physics2D.OverlapCircle(transform.localPosition,CheckRadius,Player);
        hitEdge=(Physics2D.OverlapCircle(transform.localPosition,CheckRadius,Edge)||Physics2D.OverlapCircle(transform.localPosition,CheckRadius,Edge_));
        if(hitPlayer)
        {
            if(rd.Next(0,3)!=0)
                PlayerC.GetHurt(Damage-PlayerC.Defend);
            Destroy(gameObject);
        }
        if(hitEdge) Destroy(gameObject);
    }
    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.localPosition,CheckRadius);
    }
}
