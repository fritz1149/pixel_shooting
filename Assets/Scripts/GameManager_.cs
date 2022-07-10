using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Diagnostics;
using System.Threading;
public class GameManager_ : MonoBehaviour
{
    public int maxdis=0,level,iRoot;
    public int [] siz;        
    public int [,] size=new int [5,5],dpos=new int [4,2]{{0,1},{0,-1},{-1,0},{1,0}},type;
    public bool [,] filled;
    public bool [,,] Access=new bool[5,5,4];
    public byte [] buffer;
    System.Random rd;
    public pair [] lw;
    //引用&引用相关
    public GameObject [,] Level=new GameObject[3,10],map=new GameObject[5,5];
    public GameObject [] door,door_;
    public GameObject Player;
    public float [,] doorPos;
    public string [] levelTag;
    public MonsterRoom son;
    void Start()
    {
        InitRandom();
        MonsterInit();
        GenerateMap();
    }
    int max(int x,int y)
    {
        if(x>y) return x;
        return y;
    }
    void InitRandom()
    {
        buffer=Guid.NewGuid().ToByteArray();
        iRoot=BitConverter.ToInt32(buffer,0);
        rd=new System.Random(iRoot);
    }
    void MonsterInit()
    {
        levelTag=new string[3]{"level0","level1","level2"};
        siz=new int[5];
        for(int i=0;i<3;i++)
        {
            GameObject [] temp=GameObject.FindGameObjectsWithTag(levelTag[i]);
            int j=-1;
            foreach(GameObject x in temp)
            {
                Level[i,++j]=x;
                x.GetComponent<Enemy>().ReadJson();
            }
            siz[i]=j;
        }
    }
    void GenerateMap()
    {
        //选择第一个位置
        GameObject [,,] pipe=new GameObject [4,4,4]
        {
            {{GameObject.FindGameObjectWithTag("1212C"),GameObject.FindGameObjectWithTag("912C"),GameObject.FindGameObjectWithTag("1212C"),GameObject.FindGameObjectWithTag("912C")},{GameObject.FindGameObjectWithTag("129C"),GameObject.FindGameObjectWithTag("99C"),GameObject.FindGameObjectWithTag("129C"),GameObject.FindGameObjectWithTag("99C")},{GameObject.FindGameObjectWithTag("1212C"),GameObject.FindGameObjectWithTag("912C"),GameObject.FindGameObjectWithTag("1212C"),GameObject.FindGameObjectWithTag("912C")},{GameObject.FindGameObjectWithTag("129C"),GameObject.FindGameObjectWithTag("99C"),GameObject.FindGameObjectWithTag("129C"),GameObject.FindGameObjectWithTag("99C")}},
            {{GameObject.FindGameObjectWithTag("1212C"),GameObject.FindGameObjectWithTag("129C"),GameObject.FindGameObjectWithTag("1212C"),GameObject.FindGameObjectWithTag("129C")},{GameObject.FindGameObjectWithTag("912C"),GameObject.FindGameObjectWithTag("99C"),GameObject.FindGameObjectWithTag("912C"),GameObject.FindGameObjectWithTag("99C")},{GameObject.FindGameObjectWithTag("1212C"),GameObject.FindGameObjectWithTag("129C"),GameObject.FindGameObjectWithTag("1212C"),GameObject.FindGameObjectWithTag("129C")},{GameObject.FindGameObjectWithTag("912C"),GameObject.FindGameObjectWithTag("99C"),GameObject.FindGameObjectWithTag("912C"),GameObject.FindGameObjectWithTag("99C")}},
            {{GameObject.FindGameObjectWithTag("1212"),GameObject.FindGameObjectWithTag("1212"),GameObject.FindGameObjectWithTag("912"),GameObject.FindGameObjectWithTag("912")},{GameObject.FindGameObjectWithTag("1212"),GameObject.FindGameObjectWithTag("1212"),GameObject.FindGameObjectWithTag("912"),GameObject.FindGameObjectWithTag("912")},{GameObject.FindGameObjectWithTag("129"),GameObject.FindGameObjectWithTag("129"),GameObject.FindGameObjectWithTag("99"),GameObject.FindGameObjectWithTag("99")},{GameObject.FindGameObjectWithTag("129"),GameObject.FindGameObjectWithTag("129"),GameObject.FindGameObjectWithTag("99"),GameObject.FindGameObjectWithTag("99")}},
            {{GameObject.FindGameObjectWithTag("1212"),GameObject.FindGameObjectWithTag("1212"),GameObject.FindGameObjectWithTag("129"),GameObject.FindGameObjectWithTag("129")},{GameObject.FindGameObjectWithTag("1212"),GameObject.FindGameObjectWithTag("1212"),GameObject.FindGameObjectWithTag("129"),GameObject.FindGameObjectWithTag("129")},{GameObject.FindGameObjectWithTag("912"),GameObject.FindGameObjectWithTag("912"),GameObject.FindGameObjectWithTag("99"),GameObject.FindGameObjectWithTag("99")},{GameObject.FindGameObjectWithTag("912"),GameObject.FindGameObjectWithTag("912"),GameObject.FindGameObjectWithTag("99"),GameObject.FindGameObjectWithTag("99")}}
        };
        GameObject [] room=new GameObject[4]{GameObject.FindGameObjectWithTag("1717"),GameObject.FindGameObjectWithTag("1723"),GameObject.FindGameObjectWithTag("2317"),GameObject.FindGameObjectWithTag("2323")};
        type=new int [5,5];
        filled=new bool [5,5];
        int Firstd=rd.Next(0,4),NumOfRoom=rd.Next(4,7),last=0,Firstx=2+dpos[Firstd,0],Firsty=2+dpos[Firstd,1];
        lw=new pair[4]{new pair(15,15),new pair(15,21),new pair(21,15),new pair(21,21)};
        filled[2,2]=filled[Firstx,Firsty]=true;
        size[2,2]=0;
        size[Firstx,Firsty]=rd.Next(1,4);
        map[2,2]=Instantiate(room[0],new Vector2(0,0),transform.localRotation);
        Instantiate(pipe[Firstd,0,size[Firstx,Firsty]],new Vector2(dpos[Firstd,0]*20.5f,dpos[Firstd,1]*20.5f),transform.localRotation);
        //生成基本地图 
        pair [] que=new pair[30];
        que[0]=new pair(Firstx,Firsty);
        int [] oppose=new int[4]{1,0,3,2};
        Access[2,2,Firstd]=Access[Firstx,Firsty,oppose[Firstd]]=true;
        for(int i=0;i<=NumOfRoom;i++)
        {
            if(que[i]==null)
                break;
            map[que[i].x,que[i].y]=Instantiate(room[size[que[i].x,que[i].y]],new Vector2((que[i].x-2)*41,(que[i].y-2)*41),transform.localRotation);
            if(last<NumOfRoom)
                for(int j=0,newx,newy,v;j<4;j++)
                {
                    newx=que[i].x+dpos[j,0];
                    newy=que[i].y+dpos[j,1];
                    v=0;
                    if(last<NumOfRoom&&newx>=0&&newx<=4&&newy>=0&&newy<=4&&!(newx==2&&newy==2)&&!Access[que[i].x,que[i].y,j])
                    {
                        if(!filled[newx,newy])
                        {
                            if(i==last||rd.Next(0,3)==0)
                            {
                                que[++last]=new pair(newx,newy);
                                size[newx,newy]=rd.Next(0,4);
                                filled[newx,newy]=true;
                                if(size[newx,newy]==0)
                                {
                                    size[newx,newy]=0;
                                    type[newx,newy]=4;
                                }
                                v=1;
                            }
                        }
                        else if(rd.Next(0,4)==0)
                            v=1;
                        if(v==1)
                        {
                            Instantiate(pipe[j,size[que[i].x,que[i].y],size[newx,newy]],new Vector2((que[i].x-2)*41+dpos[j,0]*20.5f,(que[i].y-2)*41+dpos[j,1]*20.5f),transform.localRotation);
                            Access[que[i].x,que[i].y,j]=Access[newx,newy,oppose[j]]=true;
                        }
                    }
                }
        }
        //指定房间类型 1为起始房间，2为最终房间，3为怪物房间，4为箱子房间
        type[2,2]=1;
            //指定最终房间
        int [,] dis=new int[5,5];
        dis[2,2]=0;
        que[0]=new pair(2,2);
        last=0;
        for(int i=0;i<=last;i++)
        {
            maxdis=max(maxdis,dis[que[i].x,que[i].y]);
            for(int j=0,newx,newy;j<4;j++)
            {
                newx=que[i].x+dpos[j,0];
                newy=que[i].y+dpos[j,1];
                if(Access[que[i].x,que[i].y,j]&&dis[newx,newy]==0)
                {
                    que[++last]=new pair(newx,newy);
                    dis[newx,newy]=dis[que[i].x,que[i].y]+1;
                }
            }
        }        
        GameObject outer=GameObject.FindGameObjectWithTag("Outer");
        int cnt=-1;
        for(int i=0;i<=last;i++)
            if(dis[que[i].x,que[i].y]==maxdis)
                que[++cnt]=que[i];
        int choose2=rd.Next(0,cnt+1);
        outer.transform.localPosition=new Vector3((que[choose2].x-2)*41,(que[choose2].y-2)*41,0);
        //outer.transform.localPosition=new Vector3(0,0,0);
        type[que[choose2].x,que[choose2].y]=2;
        //补全房间漏口
        door=new GameObject[4] {GameObject.FindGameObjectWithTag("5"),GameObject.FindGameObjectWithTag("5"),GameObject.FindGameObjectWithTag("5C"),GameObject.FindGameObjectWithTag("5C")};
        door_=new GameObject[4]{GameObject.FindGameObjectWithTag("5_"),GameObject.FindGameObjectWithTag("5_"),GameObject.FindGameObjectWithTag("5C_"),GameObject.FindGameObjectWithTag("5C_")};
        doorPos=new float[4,4]
        {
            {8.0f,8.0f,8.0f,8.0f},
            {11.0f,11.0f,8.0f,8.0f},
            {8.0f,8.0f,11.0f,11.0f},
            {11.0f,11.0f,11.0f,11.0f}
        };
        for(int i=0;i<5;i++)
            for(int j=0;j<5;j++)
                for(int k=0;k<4;k++)
                    if(filled[i,j]) 
                    {
                        if(!Access[i,j,k])
                            Instantiate(door[k],new Vector2((i-2)*41+dpos[k,0]*doorPos[size[i,j],k],(j-2)*41+dpos[k,1]*doorPos[size[i,j],k]),transform.localRotation);
                        else Instantiate(door_[k],new Vector2((i-2)*41+dpos[k,0]*doorPos[size[i,j],k],(j-2)*41+dpos[k,1]*doorPos[size[i,j],k]),transform.localRotation);
                    }
            //指定箱子房间
        GameObject box=GameObject.FindGameObjectWithTag("Box");
        for(int i=0;i<5;i++)
            for(int j=0;j<5;j++)
                if(type[i,j]==4)
                    Instantiate(box,new Vector2((i-2)*41,(j-2)*41),transform.localRotation);
        Player=GameObject.FindGameObjectWithTag("Player");
        Player.GetComponent<PlayerController>().box=GameObject.FindGameObjectsWithTag("Box");
            //指定怪物房间
        for(int i=0;i<5;i++)
            for(int j=0;j<5;j++)
                if(filled[i,j])
                {
                    son=map[i,j].GetComponent<MonsterRoom>();
                    if(type[i,j]==0)
                    {
                        type[i,j]=3;
                        Generate((i-2)*41,(j-2)*41,lw[size[i,j]].x,lw[size[i,j]].y);
                        son.Activated=true;
                    }
                    son.x=i;
                    son.y=j;
                    son.type=size[i,j];
                    son.Init();
                }
        UIManager UM=GameObject.FindGameObjectWithTag("UM").GetComponent<UIManager>();
        UM.InitMap();
    }
    void Generate(int x,int y,int len,int wid)
    {
        int left=5;
        for(int i=level,m;i>=1&&left>0;i--)
        {
            m=rd.Next(0,left+1);
            left-=m;
            Create(x,y,i,m,len,wid);
        }
        if(left>0)
            Create(x,y,0,left,len,wid);
    }
    void Create(int X,int Y,int l,int left,int len,int wid)
    {
        
        for(int i=0,m;i<siz[l]&&left>0;i++)
        {
            m=rd.Next(0,left+1);
            left-=m;
            for(int j=1,x,y;j<=m;j++)
            {
                x=rd.Next(1,len+1);
                y=rd.Next(1,wid+1);
                Instantiate(Level[l,i],new Vector2(X+x-(len+1)/2,Y+y-(wid+1)/2),transform.localRotation).GetComponent<Enemy>().Belong=son;
            }
        }
        if(left>0)
            for(int i=1,x,y;i<=left;i++)
            {
                x=rd.Next(1,len+1);
                y=rd.Next(1,wid+1);
                Instantiate(Level[l,siz[l]],new Vector2(X+x-(len+1)/2,Y+y-(wid+1)/2),transform.localRotation).GetComponent<Enemy>().Belong=son;
            }
    }
}
