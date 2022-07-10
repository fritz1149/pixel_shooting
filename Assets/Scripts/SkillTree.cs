using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SkillTree : MonoBehaviour
{
    int Points,Left;
    bool [,] vist=new bool[4,5];
    bool [] cd=new bool[4];
    float [] Timer=new float[4]{0.0f,0.0f,0.0f,0.0f},Cd=new float[4]{20.0f,5.0f,15.0f,20.0f};
    KeyCode [] KeyPos=new KeyCode[4]{KeyCode.Alpha3,KeyCode.Alpha4,KeyCode.Alpha5,KeyCode.Alpha6};
    //引用
    public GameObject Rest,Canvas1,Canvas2,Grid,Player,PlayerInfo;
    public GameObject [,] Skill=new GameObject[4,5],Button=new GameObject[4,5],ActiveSkill=new GameObject[4,4];
    public PlayerController PlayerC;
    void Start()
    {
        DontDestroyOnLoad(this);
        DontDestroyOnLoad(Canvas1);
        DontDestroyOnLoad(Canvas2);
        DontDestroyOnLoad(Grid);
        hide();
        SceneManager.LoadScene(1);
    }
    void FixedUpdate()
    {
        for(int i=0;i<4;i++)
            if(vist[i,4]&&cd[i])
            {
                Timer[i]-=Time.deltaTime;
                ActiveSkill[i,3].GetComponent<Text>().text=Timer[i].ToString("f2");
                if(Timer[i]<=0.0f)
                {
                    Timer[i]=0.0f;
                    cd[i]=false;
                    ActiveSkill[i,1].SetActive(true);
                    ActiveSkill[i,3].SetActive(false);
                }
            }
        for(int i=0;i<4;i++)
            if(Input.GetKey(KeyPos[i])&&vist[i,4]&&!cd[i])
            {
                Button[i,4].GetComponent<Buff>().Add(PlayerC);
                ActiveSkill[i,1].SetActive(false);
                ActiveSkill[i,3].SetActive(true);
                ActiveSkill[i,3].GetComponent<Text>().text=Cd[i].ToString("f2");
                cd[i]=true;
                Timer[i]=Cd[i];
            }
    }
    public void Init()
    {
        ASInit();
        for(int i=0;i<4;i++)
        {
            vist[i,0]=true;
            for(int j=1;j<5;j++)
            {
                Button[i,j]=Canvas1.transform.GetChild(i).GetChild(j-1).gameObject;
                Button[i,j].GetComponent<Button>().x=i;
                Button[i,j].GetComponent<Button>().y=j;
                Skill[i,j]=Canvas2.transform.GetChild(i).GetChild(j-1).gameObject;
                Skill[i,j].SetActive(false);
                vist[i,j]=false;
                cd[i]=false;
            }
        }
        hide();
    }
    public void hide()
    {
        Rest.SetActive(false);
        Canvas1.SetActive(false);
        Canvas2.SetActive(false);
        Grid.SetActive(false);
        Time.timeScale=1;
    }
    public void show()
    {
        if(SceneManager.GetActiveScene().buildIndex==4)
        {
            SceneManager.LoadScene(5);
            Remove();
        }
        Points=PlayerPrefs.GetInt("SceneID")*4;
        Left+=4;
        Rest.GetComponent<Text>().text=Left.ToString();
        Rest.SetActive(true);
        Canvas1.SetActive(true);
        Canvas2.SetActive(true);
        Grid.SetActive(true);
        Grid.transform.localPosition=Player.transform.localPosition;
        Time.timeScale=0;
    }
    public void Remake()
    {
        Rest.GetComponent<Text>().text=Points.ToString();
        Left=Points;
        for(int i=0;i<4;i++)
            for(int j=1;j<5;j++)
            {
                if(!vist[i,j]) break;
                Skill[i,j].SetActive(false);
                vist[i,j]=false;
                Button[i,j].GetComponent<Buff>().vist=false;
            }
    }
    public void ToNext()
    {
        SceneManager.LoadScene(PlayerPrefs.GetInt("SceneID")+1);
    }
    public void Effect()
    {
        ASInit();
        for(int i=0;i<4;i++)
        {
            for(int j=1;j<4;j++)
            {
                if(!vist[i,j]) break;
                Button[i,j].GetComponent<Buff>().Add(PlayerC);
            }
            if(vist[i,4])
            {
                for(int j=0;j<3;j++) ActiveSkill[i,j].SetActive(true);
                cd[i]=false;
                Timer[i]=0.0f;
            }
        }
        hide();
    }
    void ASInit()
    {
        PlayerInfo=GameObject.FindGameObjectWithTag("PlayerInfo");
        for(int i=0;i<4;i++) 
            for(int j=0;j<4;j++)
            {
                ActiveSkill[i,j]=PlayerInfo.transform.GetChild(i+7).GetChild(j).gameObject;
                ActiveSkill[i,j].SetActive(false);
            }
    }
    public void LightUp(int x,int y)
    {
        if(Left>0&&vist[x,y-1])
        {
            Skill[x,y].SetActive(true);
            vist[x,y]=true;
            Rest.GetComponent<Text>().text=(--Left).ToString();
        }
    }
    public void Remove()
    {
        Destroy(Canvas1);
        Destroy(Canvas2);
        Destroy(Grid);
        Destroy(gameObject);
    }
}