using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Diagnostics;
using System.Threading;
public class MonsterRoom : MonoBehaviour
{
    public int left=5,x,y,type;
    public float l,w;
    public bool Activated,Entered,Built,Passed,Entering;
    //引用
    public GameObject Player,GameManager,Interface;
    public GameObject [] wall=new GameObject[4];
    public GameManager_ GM;
    public UIManager UM;
    void Awake()
    {
        Player=GameObject.FindGameObjectWithTag("Player");
        GameManager=GameObject.FindGameObjectWithTag("GM");
        Interface=GameObject.FindGameObjectWithTag("Interface");
        GM=GameManager.GetComponent<GameManager_>();
        UM=GameObject.FindGameObjectWithTag("UM").GetComponent<UIManager>();
    }
    void FixedUpdate()
    {
        if((!Entering)&&(Entering=(abs(Player.transform.localPosition.x-(x-2)*41)<=(l*0.5f)&&abs(Player.transform.localPosition.y-(y-2)*41)<=(w*0.5f))))
        {
            Entered=true;
            UM.room[UM.x,UM.y,2].SetActive(false);
            UM.room[x,y,2].SetActive(true);
            if(!(x==UM.x&&y==UM.y)) GM.map[UM.x,UM.y].GetComponent<MonsterRoom>().Entering=false;
            UM.x=x;
            UM.y=y;
            if(!UM.Entered[x,y])
            {
                UM.Entered[x,y]=true;
                UM.room[x,y,1].SetActive(true);
            }
            for(int i=0;i<4;i++)
                if(GM.Access[x,y,i]&&!UM.Entered[x+GM.dpos[i,0],y+GM.dpos[i,1]])
                {
                    UM.room[x+GM.dpos[i,0],y+GM.dpos[i,1],0].SetActive(true);
                    Instantiate(UM.Pipe[i],new Vector2(200+(x-2)*45.0f+GM.dpos[i,0]*22.5f,50+(y-2)*45.0f+GM.dpos[i,1]*22.5f),transform.localRotation).transform.SetParent(Interface.transform,false);
                }
        }
        if(Activated)
            if(!Passed)
            {
                if(!Built)
                {
                    if(Entered)
                    {
                        for(int i=0;i<4;i++)
                            if(GM.Access[x,y,i])
                                wall[i]=Instantiate(GM.door[i],new Vector2((x-2)*41+GM.dpos[i,0]*GM.doorPos[GM.size[x,y],i],(y-2)*41+GM.dpos[i,1]*GM.doorPos[GM.size[x,y],i]),transform.localRotation);
                        Built=true;
                        Interface.SetActive(false);
                    }
                }
                if(Built&&left==0)
                {
                    for(int i=0;i<4;i++)
                        if(GM.Access[x,y,i])
                            Destroy(wall[i]);
                    Passed=true;
                    Interface.SetActive(true);
                }
            }
    }
    public void Init()
    {
        l=GM.lw[type].x;
        w=GM.lw[type].y;
    }
    float abs(float a)
    {
        if(a<0) return a*(-1);
        return a;
    }
}
