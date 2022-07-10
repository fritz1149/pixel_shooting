using System.Collections;
using UnityEngine;
using LitJson;
using System.IO;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.Threading;
public class JCW
{
    public string AttackInterval,CheckRadius,CheckAngle,Damage,name;
}
public class ColdWeapon : MonoBehaviour
{
    public float AttackInterval,Timer,CheckRadius,CheckAngle;
    public int id,NumOfHit,Damage,iRoot;
    public bool hit,Activated;
    byte [] buffer;
    System.Random rd;
    //引用
    public LayerMask EnemyL;
    public Collider2D [] GetHit;
    public GameObject Player;
    public PlayerController PlayerC;
    public UIManager UM;
    void Awake()
    {
        EnemyL=LayerMask.GetMask("Enemy");
        Player=GameObject.FindGameObjectWithTag("Player");
        PlayerC=Player.GetComponent<PlayerController>();
        UM=GameObject.FindGameObjectWithTag("UM").GetComponent<UIManager>();
        Timer=0.0f;
        GetHit=new Collider2D[40];
        InitRandom();
    }
    void FixedUpdate()
    {
        if(Activated)
            Hit();
    }
    void InitRandom()
    {
        buffer=Guid.NewGuid().ToByteArray();
        iRoot=BitConverter.ToInt32(buffer,0);
        rd=new System.Random(iRoot);
    }
    public void ReadJson(int ID)
    {
        id=ID;
        StreamReader streamreader = new StreamReader(Application.dataPath + "/Data/ColdWeapons.txt");
        JsonReader js = new JsonReader(streamreader);
        List<JCW> L=JsonMapper.ToObject<List<JCW>>(js);
        AttackInterval=float.Parse(L[id].AttackInterval);
        CheckRadius=float.Parse(L[id].CheckRadius);
        CheckAngle=float.Parse(L[id].CheckAngle);
        Damage=int.Parse(L[id].Damage);
        name=L[id].name;
        AttackInterval/=PlayerC.IntervalCut;
    }
    public void Hit()
    {
        if(Input.GetMouseButton(0))
        {
            Vector2 LocalPos=Player.transform.localPosition,front=Front();
            Timer+=Time.deltaTime;
            if(Timer>=AttackInterval)
            {
                Timer=0.0f;
                hit=Physics2D.OverlapCircle(LocalPos,CheckRadius,EnemyL);
                if(hit)
                {
                    NumOfHit=Physics2D.OverlapCircleNonAlloc(LocalPos,CheckRadius,GetHit,EnemyL);
                    for(int i=0;i<NumOfHit;i++)
                        if(Vector2.Angle(front,EnemyDir(LocalPos,i))<=CheckAngle)
                        {
                            if(PlayerC.Heavy&&rd.Next(0,3)==0)
                            {
                                GetHit[i].GetComponent<Enemy>().dizzy=true;
                                GetHit[i].GetComponent<Enemy>().Fly=PlayerC.Fly?Front()*0.3f:new Vector2(0,0);
                            }
                            GetHit[i].GetComponent<Enemy>().GetHurt(Damage+PlayerC.DamagePlus);
                        }
                    if(PlayerC.Lightening&&rd.Next(0,3)==0)
                    {
                        for(int i=0;i<NumOfHit;i++)
                            GetHit[i].GetComponent<Enemy>().GetHurt(3);
                    }
                }
            }
        }
    }
    Vector2 EnemyDir(Vector2 LocalPos,int x)
    {
        Vector2 EnemyP=new Vector2(GetHit[x].transform.localPosition.x,GetHit[x].transform.localPosition.y);
        float Dis=Vector2.Distance(LocalPos,EnemyP);
        return (EnemyP-LocalPos)/Dis;
    }
    Vector2 Front()
    {
        Vector2 Mouse=Input.mousePosition,Centre=new Vector2(Screen.width/2,Screen.height/2);
        float Dis=Vector2.Distance(Mouse,Centre);
        return (Mouse-Centre)/Dis;
    }
    float abs(float x)
    {
        if(x<0) return -x;
        return x;
    }
    float Ang(Vector2 Temp)
    {
        float pi=3.1415926f,ang1=(float)System.Math.Acos(Temp.x)*180/pi,ang2=(float)System.Math.Asin(Temp.y)*180/pi;
        if(!(ang1<0&&ang2<0)) return -abs(ang1);
        else return(-180-ang2);
    }
}
