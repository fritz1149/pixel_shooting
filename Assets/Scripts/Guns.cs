using System.Collections;
using UnityEngine;
using LitJson;
using System.IO;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.Threading;
public class JG
{
    public string AttackInterval,Cost,BulletID,name;
}
public class Guns : MonoBehaviour
{
    public float AttackInterval,Timer;
    public int Cost,BulletID,id,iRoot;
    public bool Activated;
    byte [] buffer;
    System.Random rd;
    //引用
    public GameObject Player,Bullet1;
    public Bullet1 Clone;
    public PlayerController PlayerC;
    public UIManager UM;
    void Awake()
    {
        Player=GameObject.FindGameObjectWithTag("Player");
        PlayerC=Player.GetComponent<PlayerController>();
        Timer=0.0f;
        UM=GameObject.FindGameObjectWithTag("UM").GetComponent<UIManager>();
        InitRandom();
    }
    void FixedUpdate()
    {
        if(Activated)
            Shoot();
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
        StreamReader streamreader = new StreamReader(Application.dataPath + "/Data/Guns.txt");
        JsonReader js = new JsonReader(streamreader);
        List<JG> L=JsonMapper.ToObject<List<JG>>(js);
        AttackInterval=float.Parse(L[id].AttackInterval);
        Cost=int.Parse(L[id].Cost);
        BulletID=int.Parse(L[id].BulletID);
        name=L[id].name;
        AttackInterval/=PlayerC.IntervalCut;
    }
    public void Shoot()
    {
        if(Input.GetMouseButton(0))
        {
            if(PlayerC.Magic+PlayerC.MagicAdd>=Cost)
            {
                Vector2 LocalPos=Player.transform.localPosition;
                Timer+=Time.deltaTime;
                if(Timer>=AttackInterval)
                {
                    Timer=0.0f;
                    Clone=Instantiate(Bullet1,LocalPos,Player.transform.localRotation).GetComponent<Bullet1>();
                    Clone.V=Front();
                    if(PlayerC.Heavy) Clone.Heavy=(rd.Next(0,3)==0);
                    PlayerC.Magic-=Cost;
                    UM.Magic.text=(PlayerC.Magic+PlayerC.MagicAdd).ToString();
                }
            }
        }
    }
    Vector2 Front()
    {
        Vector2 Mouse=Input.mousePosition,Centre=new Vector2(Screen.width/2,Screen.height/2);
        float Dis=Vector2.Distance(Mouse,Centre);
        return (Mouse-Centre)/Dis;
    }
}
