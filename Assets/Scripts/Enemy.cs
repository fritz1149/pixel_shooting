using System.Collections;
using UnityEngine;
using LitJson;
using System.IO;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.Threading;
using UnityEngine.UI;
public class Enemy : MonoBehaviour
{
    public bool ReadyToAttack,Discover,Rest=true,Moving=false,dizzy=false,Acted;
    public float speed,AttackInterval,Timer1,Timer2,Timer3=0.0f,DiscoverRadius,AttackRadius,MoveTime,MoveInterval,Randomx,Randomy,Randomdx,Randomdy,Dis;
    public int level,id,Health,OrigHealth,PayBack,iRoot;
    public Vector2 Fly;
    public byte [] buffer;
    public System.Random rd;
    //引用
    public LayerMask PlayerL,Bullet1L;
    public GameObject Player,Hurt,Hurt1;
    public PlayerController PlayerC;
    public MonsterRoom Belong;
    public UIManager UM;
    void FixedUpdate()
    {
        if(Belong!=null&&Belong.Built)
        {
            CheckHealth();
            Scan();
            if(Health<=OrigHealth/2&&!Acted) {DangerEvent();Acted=true;}
            if(dizzy) Stay();
            else if(!Discover) RandomMove1();
            else if(Moving) RandomMove2();
            else if(ReadyToAttack) Attack();
            else Approach();
        }
    }
    public virtual void ReadJson()
    {

    }
    protected void InitRandom()
    {
        buffer=Guid.NewGuid().ToByteArray();
        iRoot=BitConverter.ToInt32(buffer,0);
        rd=new System.Random(iRoot);
    }
    protected int min(int x,int y)
    {
        if(x<y) return x;
        return y;
    }
    protected void CheckHealth()
    {
        if(Health<=0)
        {
            Belong.left--;
            PlayerC.Magic=min(100,PlayerC.Magic+PayBack);
            UM.Magic.text=(PlayerC.Magic+PlayerC.MagicAdd).ToString();
            UM.ShowMagic(PayBack);
            DeadEvent();
            Destroy(gameObject);
        }
    }
    protected void Scan()
    {
        Vector2 PlayerPos=new Vector2(Player.transform.localPosition.x,Player.transform.localPosition.y),LocalPos=new Vector2(transform.localPosition.x,transform.localPosition.y);
        Dis=Vector2.Distance(PlayerPos,LocalPos);
        ReadyToAttack=(Vector2.Distance(PlayerPos,LocalPos)<=AttackRadius);
        Discover=(Vector2.Distance(PlayerPos,LocalPos)<=DiscoverRadius||Health<OrigHealth);
    }
    protected void Approach()
    {
        Vector2 front=Front();
        transform.Translate(speed*front.x,speed*front.y,0);
    }
    protected void Escape()
    {
        Vector2 front=Front();
        transform.Translate(speed*front.x*-1,speed*front.y*-1,0);
    }
    protected void RandomMove1()
    {
        Timer1+=Time.deltaTime;
        if(Rest)
        {
            if(Timer1>=MoveInterval)
            {
                Timer1=0.0f;
                Rest=false;
                Randomx=1.0f*rd.Next(0,101)/100;
                Randomy=(float)System.Math.Sqrt(1-Randomx*Randomx);
                Randomdx=rd.Next(0,2)==0?1.0f:-1.0f;
                Randomdy=rd.Next(0,2)==0?1.0f:-1.0f;
            }
        }
        else
        {
            transform.Translate(speed*Randomx*Randomdx,speed*Randomy*Randomdy,0);
            if(Timer1>=MoveTime)
            {
                Timer1=0.0f;
                Rest=true;
            }
        }
    }
    protected void MoveInit()
    {
        Randomx=1.0f*rd.Next(0,101)/100;
        Randomy=(float)System.Math.Sqrt(1-Randomx*Randomx);
        Randomdx=rd.Next(0,2)==0?1.0f:-1.0f;
        Randomdy=rd.Next(0,2)==0?1.0f:-1.0f;
        Moving=true;
    }
    protected void RandomMove2()
    {
        Timer1+=Time.deltaTime;
        if(Timer1<MoveTime)
            transform.Translate(speed*Randomx*Randomdx,speed*Randomy*Randomdy,0);
        else 
        {
            Timer1=0.0f;
            Moving=false;
        }
    }
    protected Vector2 Front()
    {
        Vector2 PlayerPos=new Vector2(Player.transform.localPosition.x,Player.transform.localPosition.y),LocalPos=new Vector2(transform.localPosition.x,transform.localPosition.y);
        float Dis=Vector2.Distance(PlayerPos,LocalPos);
        return (PlayerPos-LocalPos)/Dis;
    }
    protected void Stay()
    {
        Timer3+=Time.deltaTime;
        transform.Translate(Fly.x,Fly.y,0);
        if(Timer3>=2.0f)
        {
            dizzy=false;
            Timer3=0.0f;
        }
    }
    protected virtual void DeadEvent(){}
    protected virtual void DangerEvent(){}
    protected virtual void Attack(){}
    public void GetHurt(int x)
    {
        Health-=x;
        Hurt1=Instantiate(Hurt,transform.localPosition,transform.localRotation);
        Hurt1.transform.GetChild(0).gameObject.GetComponent<Text>().text="-"+x.ToString();
        Hurt1.GetComponent<H>().Follow=this.gameObject;
        Hurt1.GetComponent<H>().Activated=true;
    }
}
