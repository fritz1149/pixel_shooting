using System.Collections;
using UnityEngine;
using LitJson;
using System.IO;
using System.Collections.Generic;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    
    public int note=0,x=2,y=2;
    public bool MagicP=false,hurt=false;
    public float Timer=0.0f,Timer2=0.0f;
    //引用
    public Text WeapDisplay,Health,Magic;
    public string [] InBagName=new string[2]{"InBag1","Inbag2"};
    public GameObject Player,CheckBox,Interface,PlayerInfo,InBox,Box,Paper,MagicPlus,Hurt;
    public GameObject [] InBag,Pipe;
    public PlayerController PlayerC;
    public Box BoxC;
    public GameManager_ GM;
    void Start()
    {
        Player=GameObject.FindGameObjectWithTag("Player");//
        PlayerC=Player.GetComponent<PlayerController>();
        CheckBox=GameObject.FindGameObjectWithTag("CheckBox");//
        Paper=CheckBox.transform.GetChild(3).gameObject;
        InBox=CheckBox.transform.GetChild(4).gameObject;
        InBag=new GameObject[2];
        for(int i=0;i<2;i++) InBag[i]=CheckBox.transform.GetChild(i+5).gameObject;
        CheckBox.SetActive(false);
        PlayerInfo=GameObject.FindGameObjectWithTag("PlayerInfo");//
        Health=PlayerInfo.transform.GetChild(1).gameObject.GetComponent<Text>();
        Health.text=(PlayerPrefs.GetInt("Health")+PlayerC.HealthAdd).ToString();
        Magic=PlayerInfo.transform.GetChild(3).gameObject.GetComponent<Text>();
        Magic.text=(PlayerPrefs.GetInt("Magic")+PlayerC.MagicAdd).ToString();
        MagicPlus=PlayerInfo.transform.GetChild(6).gameObject;
        MagicPlus.SetActive(false);
        Hurt=PlayerInfo.transform.GetChild(11).gameObject;
        Hurt.SetActive(false);
        Interface=GameObject.FindGameObjectWithTag("Interface");//
        GM=GameObject.FindGameObjectWithTag("GM").GetComponent<GameManager_>();//
        Pipe=new GameObject[4]{GameObject.FindGameObjectWithTag("PipeC"),GameObject.FindGameObjectWithTag("PipeC"),GameObject.FindGameObjectWithTag("PipeR"),GameObject.FindGameObjectWithTag("PipeR")};
        WeapDisplay=GameObject.FindGameObjectWithTag("Weap").GetComponent<Text>();
    }
    void Update()
    {
        if(PlayerC.Interacting)
        {
            if(Input.GetKeyDown(KeyCode.Tab))
            {
                note=(note+1)%2;
                Paper.transform.localPosition=new Vector3((note^1)*280,Paper.transform.localPosition.y,0);
            }
            if(Input.GetKeyDown(KeyCode.E))
            {
                int id=BoxC.id,type=BoxC.type;
                BoxC.id=PlayerC.bag[note].x;
                BoxC.type=PlayerC.bag[note].y;
                PlayerC.ChangeWeapon(note,id,type);
                ShowBox(Box);
            }
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                CheckBox.SetActive(false);
                PlayerInfo.SetActive(true);
                PlayerC.Interacting=false;
                PlayerC.FocusType=PlayerC.bag[PlayerC.WeapFocus].y;
                if(PlayerC.FocusType==0) WeapDisplay.text=PlayerC.Weaps[PlayerC.WeapFocus].GetComponent<ColdWeapon>().name;
                else WeapDisplay.text=PlayerC.Weaps[PlayerC.WeapFocus].GetComponent<Guns>().name;
                note=0;
            }
        }
        else 
        {
            if(MagicP)
            {
                Timer+=Time.deltaTime;
                if(Timer>=1.0f)
                {
                    MagicP=false;
                    MagicPlus.SetActive(false);
                }
            }
            if(hurt)
            {
                Timer2+=Time.deltaTime;
                if(Timer2>=0.3f)
                {
                    hurt=false;
                    Hurt.SetActive(false);
                }
            }
        }
    }
    public GameObject [,] Room=new GameObject[5,3];
    public GameObject [,,] room=new GameObject[5,5,3];
    public string [,] RoomTag=new string[4,3]{{"H0","H1","H2"},{"E0","E1","E2"},{"M0","M1","M2"},{"B0","B1","B2"}};
    public int [,] dpos=new int [4,2]{{0,1},{0,-1},{-1,0},{1,0}};
    public bool [,] Entered=new bool[5,5];
    public void InitMap()
    {
        for(int i=0;i<4;i++)
            for(int j=0;j<3;j++)
                Room[i,j]=GameObject.FindGameObjectWithTag(RoomTag[i,j]);
        for(int i=0;i<5;i++)
            for(int j=0;j<5;j++)
                if(GM.filled[i,j])
                    for(int k=0;k<3;k++)
                    {
                        room[i,j,k]=Instantiate(Room[GM.type[i,j]-1,k],new Vector2(200+(i-2)*45.0f,50+(j-2)*45.0f),transform.localRotation);
                        room[i,j,k].transform.SetParent(Interface.transform,false);
                        room[i,j,k].SetActive(false);
                    }
        for(int i=0;i<3;i++) room[2,2,i].SetActive(true);
        for(int i=0;i<4;i++) 
            if(GM.Access[2,2,i])
            {
                room[2+dpos[i,0],2+dpos[i,1],0].SetActive(true);
                Instantiate(Pipe[i],new Vector2(200+dpos[i,0]*22.5f,50+dpos[i,1]*22.5f),transform.localRotation).transform.SetParent(Interface.transform,false);
                break;
            }
        Entered[2,2]=true;
    }
    public void ShowBox(GameObject x)
    {
        Box=x;
        BoxC=x.GetComponent<Box>();
        CheckBox.SetActive(true);
        InBox.GetComponent<Text>().text=ReadBoxJson(BoxC);
        PlayerInfo.SetActive(false);
        for(int i=0;i<2;i++) InBag[i].GetComponent<Text>().text=ReadBagJson(PlayerC.bag[i]);  
    }
    string ReadBoxJson(Box x)
    {
        string display;
        if(x.type==0)
        {
            StreamReader streamreader = new StreamReader(Application.dataPath + "/Data/ColdWeapons.txt");
            JsonReader js = new JsonReader(streamreader);
            List<JCW> L=JsonMapper.ToObject<List<JCW>>(js);
            display=L[x.id].name+"\n攻击间隔:"+L[x.id].AttackInterval+"\n攻击半径"+L[x.id].CheckRadius+"\n角度范围"+L[x.id].CheckAngle+"\n伤害"+L[x.id].Damage;
        }
        else
        {   
            StreamReader streamreader = new StreamReader(Application.dataPath + "/Data/Guns.txt");
            JsonReader js = new JsonReader(streamreader);
            List<JG> L=JsonMapper.ToObject<List<JG>>(js);
            string dis1=L[x.id].name+"\n攻击间隔:"+L[x.id].AttackInterval+"\n法力消耗:"+L[x.id].Cost;
            int id=int.Parse(L[x.id].BulletID);
            streamreader = new StreamReader(Application.dataPath + "/Data/Bullet1.txt");
            js = new JsonReader(streamreader);
            List<JB1> L_=JsonMapper.ToObject<List<JB1>>(js);
            string dis2="\n\n弹药:\n"+L_[id].name+"\n飞行速度:"+L_[id].speed+"\n碰撞半径"+L_[id].CheckRadius+"\n伤害:"+L_[id].Damage;
            display=dis1+dis2;
        }
        return display;
    }
    string ReadBagJson(pair slot)
    {
        string display;
        if(slot.y==0)
        {
            StreamReader streamreader = new StreamReader(Application.dataPath + "/Data/ColdWeapons.txt");
            JsonReader js = new JsonReader(streamreader);
            List<JCW> L=JsonMapper.ToObject<List<JCW>>(js);
            display=L[slot.x].name+"\n攻击间隔:"+L[slot.x].AttackInterval+"\n攻击半径:"+L[slot.x].CheckRadius+"\n角度范围:"+L[slot.x].CheckAngle+"\n伤害"+L[slot.x].Damage;
        }
        else
        {   
            StreamReader streamreader = new StreamReader(Application.dataPath + "/Data/Guns.txt");
            JsonReader js = new JsonReader(streamreader);
            List<JG> L=JsonMapper.ToObject<List<JG>>(js);
            string dis1=L[slot.x].name+"\n攻击间隔:"+L[slot.x].AttackInterval+"\n法力消耗:"+L[slot.x].Cost;
            int id=int.Parse(L[slot.x].BulletID);
            streamreader = new StreamReader(Application.dataPath + "/Data/Bullet1.txt");
            js = new JsonReader(streamreader);
            List<JB1> L_=JsonMapper.ToObject<List<JB1>>(js);
            string dis2="\n\n弹药:\n"+L_[id].name+"\n飞行速度:"+L_[id].speed+"\n碰撞半径:"+L_[id].CheckRadius+"\n伤害:"+L_[id].Damage;
            display=dis1+dis2;
        }
        return display;
    }
    public void ShowMagic(int x)
    {
        MagicP=true;
        MagicPlus.SetActive(true);
        MagicPlus.GetComponent<Text>().text="+"+x.ToString();
        Timer=0.0f;
    }
    public void ShowHurt(int x)
    {
        hurt=true;
        Hurt.SetActive(true);
        Hurt.GetComponent<Text>().text="-"+x.ToString();
        Timer2=0.0f;
    }
    public GameObject PauseMenu;
    public void Pause()
    {
        Time.timeScale=0;
        PauseMenu.SetActive(true);
    }
    public void Resume()
    {
        PauseMenu.SetActive(false);
        Time.timeScale=1;
    }
}
