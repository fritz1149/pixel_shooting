using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class pair
{
    public int x,y;
    public pair(int a,int b)
    {
        x=a;
        y=b;
    }
}
public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    private float RawScalex,GetHitRadius;
    private Vector2 Face;
    public float speed,RecoverTimer=0.0f,StormTimer=0.0f,GhostTimer=0.0f,BefallTimer=0.0f,TornadoTimer=0.0f;
    public pair[] bag;
    public int WeapFocus,FocusType,Health,HealthMax=40,Magic,DamagePlus=0,Defend=0,HealthAdd=0,MagicAdd=0,IntervalCut=1;
    public bool Interacting,Heavy=false,Fly=false,Miss=false,Recover=false,Lightening=false,Storm=false,Befall=false,Tornado=false;
    //引用
    public Text WeapDisplay,HealthT;
    public GameObject[] Weaps,Bullets,box;
    public GameObject Outer,SkillTree,Weap1;
    public S34 BefallS;
    public UIManager UM;
    public Collider2D[] GetHit=new Collider2D[40];
    public LayerMask EnemyL;
    void Awake()
    {
        //DontDestroyOnLoad(transform.gameObject);
        RawScalex=transform.localScale.x;
        speed=0.2f;
        Outer=GameObject.FindGameObjectWithTag("Outer");
        SkillTree=GameObject.FindGameObjectWithTag("SkillTree");
        SkillTree.GetComponent<SkillTree>().Player=this.gameObject;
        SkillTree.GetComponent<SkillTree>().PlayerC=this;
        SkillTree.GetComponent<SkillTree>().Effect();
        Health=PlayerPrefs.GetInt("Health");
        Magic=PlayerPrefs.GetInt("Magic");
        WeapFocus=PlayerPrefs.GetInt("WeapFocus");
        FocusType=PlayerPrefs.GetInt("FocusType");
        bag=new pair[5];
        Weaps=new GameObject[2]{GameObject.FindGameObjectWithTag("Weap0"),GameObject.FindGameObjectWithTag("Weap1")};
        Bullets=new GameObject[2]{GameObject.FindGameObjectWithTag("Bullet0"),GameObject.FindGameObjectWithTag("Bullet1")};
        Weaps[0].GetComponent<Guns>().Bullet1=Bullets[0];
        Weaps[1].GetComponent<Guns>().Bullet1=Bullets[1];
        WeapDisplay=Weap1.GetComponent<Text>();
        box=GameObject.FindGameObjectsWithTag("Box");
        UM=GameObject.FindGameObjectWithTag("UM").GetComponent<UIManager>();
        ChangeWeapon(0,PlayerPrefs.GetInt("id1"),PlayerPrefs.GetInt("type1"));
        ChangeWeapon(1,PlayerPrefs.GetInt("id2"),PlayerPrefs.GetInt("type2"));
        if(FocusType==1) 
        {
            Weaps[WeapFocus].GetComponent<Guns>().Activated=true;
            WeapDisplay.text=Weaps[WeapFocus].GetComponent<Guns>().name;
        }
        else 
        {
            Weaps[WeapFocus].GetComponent<ColdWeapon>().Activated=true;
            WeapDisplay.text=Weaps[WeapFocus].GetComponent<ColdWeapon>().name;
        }
        HealthT=GameObject.FindGameObjectWithTag("PlayerInfo").transform.GetChild(1).gameObject.GetComponent<Text>();
        EnemyL=LayerMask.GetMask("Enemy");
    }
    void FixedUpdate()
    {
        if(!Interacting)
        {
            if(Recover) recover();
            if(Storm) storm();
            if(gameObject.layer==16) ghost();
            if(Befall) befall();
            if(Tornado) tornado();
            HealthCheck();
            Move();
        }
    }
    void Update()
    {
        if(!Interacting)
            Interact();
    }
    public GameObject Lost;
    void recover()
    {
        RecoverTimer+=Time.deltaTime;
        if(RecoverTimer>=7.0f)
        {
            if(Health<HealthMax) Health++;
            RecoverTimer=0.0f;
            HealthT.text=(Health+HealthAdd).ToString();
        }
    }
    void storm()
    {
        StormTimer+=Time.deltaTime;
        if(StormTimer>=2.5f)
        {
            int NumOfHit=Physics2D.OverlapCircleNonAlloc(transform.localPosition,3.0f,GetHit,EnemyL);
            for(int i=0;i<NumOfHit;i++)
                GetHit[i].GetComponent<Enemy>().GetHurt(3);
            StormTimer=0.0f;
        }
    }
    public void Stone()
    {
        int NumOfHit=Physics2D.OverlapCircleNonAlloc(transform.localPosition,30.0f,GetHit,EnemyL);
        for(int i=0;i<NumOfHit;i++)
            GetHit[i].GetComponent<Enemy>().dizzy=true;
    }
    public void ghost()
    {
        GhostTimer+=Time.deltaTime;
        if(GhostTimer>=2.0f)
        {
            gameObject.layer=10;
            GhostTimer=0.0f;
        }
    }
    void befall()
    {
        BefallTimer+=Time.deltaTime;
        if(BefallTimer>=5.0f)
        {
            Befall=false;
            BefallTimer=0.0f;
            speed=BefallS.x1;
            IntervalCut=BefallS.x2;
            HealthAdd=BefallS.x3;
        }
    }
    void tornado()
    {
        TornadoTimer+=Time.deltaTime;
        int NumOfHit=Physics2D.OverlapCircleNonAlloc(transform.localPosition,3.0f,GetHit,EnemyL);
        for(int i=0;i<NumOfHit;i++)
        {
            GetHit[i].GetComponent<Enemy>().dizzy=true;
            GetHit[i].GetComponent<Enemy>().Fly=Opposite(GetHit[i].gameObject)*0.3f;
            GetHit[i].GetComponent<Enemy>().GetHurt(5);
        }
        if(TornadoTimer>=1.5f)
        {
            Tornado=false;
            TornadoTimer=0.0f;
        }
    }
    Vector2 Opposite(GameObject x)
    {
        float Dis=Vector2.Distance(transform.localPosition,x.transform.localPosition);
        return (x.transform.localPosition-transform.localPosition)/Dis;
    }
    void HealthCheck()
    {
        if(Health+HealthAdd<=0)
        {
            Lost.SetActive(true);
            Time.timeScale=0;
        }
    }
    void Move()
    {
        float DirX=Input.GetAxisRaw("Horizontal"),DirY=Input.GetAxisRaw("Vertical");
        if(DirX!=0) transform.localScale=new Vector3(RawScalex*DirX,transform.localScale.y,transform.localScale.z);
        transform.Translate(DirX*speed,DirY*speed,0);
    }
    void Interact()
    {
        if(Input.GetKeyDown("e"))
        {
            if(Vector3.Distance(Outer.transform.localPosition,transform.localPosition)<1.5f)
            {
                Save();
                SkillTree.GetComponent<SkillTree>().show();
            }
            else foreach(GameObject x in box)
                if(Vector3.Distance(transform.localPosition,x.transform.localPosition)<1.5f)
                {
                    Interacting=true;
                    UM.ShowBox(x);
                    break;
                }
        }
        else if(Input.GetKey(KeyCode.Alpha1))
        {
            if(FocusType==0) Weaps[WeapFocus].GetComponent<ColdWeapon>().Activated=false;
            else Weaps[WeapFocus].GetComponent<Guns>().Activated=false;
            WeapFocus=0;
            FocusType=bag[WeapFocus].y;
            if(FocusType==0) 
            {
                Weaps[WeapFocus].GetComponent<ColdWeapon>().Activated=true;
                WeapDisplay.text=Weaps[WeapFocus].GetComponent<ColdWeapon>().name;
            }
            else 
            {
                Weaps[WeapFocus].GetComponent<Guns>().Activated=true;
                WeapDisplay.text=Weaps[WeapFocus].GetComponent<Guns>().name;
            }
        }
        else if(Input.GetKey(KeyCode.Alpha2))
        {
            if(FocusType==0) Weaps[WeapFocus].GetComponent<ColdWeapon>().Activated=false;
            else Weaps[WeapFocus].GetComponent<Guns>().Activated=false;
            WeapFocus=1;
            FocusType=bag[WeapFocus].y;
            if(FocusType==0) 
            {
                Weaps[WeapFocus].GetComponent<ColdWeapon>().Activated=true;
                WeapDisplay.text=Weaps[WeapFocus].GetComponent<ColdWeapon>().name;
            }
            else 
            {
                Weaps[WeapFocus].GetComponent<Guns>().Activated=true;
                WeapDisplay.text=Weaps[WeapFocus].GetComponent<Guns>().name;
            }
        }
    }
    public void ChangeWeapon(int pos,int id,int type)
    {
        bag[pos]=new pair(id,type);
        if(type==0) Weaps[pos].GetComponent<ColdWeapon>().ReadJson(id);
        else
        {
            Weaps[pos].GetComponent<Guns>().ReadJson(id);
            Bullets[pos].GetComponent<Bullet1>().ReadJson(Weaps[pos].GetComponent<Guns>().BulletID);
        }
    }
    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.localPosition,10.0f);
        Gizmos.DrawWireSphere(transform.localPosition,1.5f);
    }
    public void Save()
    {
        PlayerPrefs.SetInt("Health",Health);
        PlayerPrefs.SetInt("Magic",Magic);
        PlayerPrefs.SetInt("WeapFocus",WeapFocus);
        PlayerPrefs.SetInt("FocusType",FocusType);
        PlayerPrefs.SetInt("id1",bag[0].x);
        PlayerPrefs.SetInt("type1",bag[0].y);
        PlayerPrefs.SetInt("id2",bag[1].x);
        PlayerPrefs.SetInt("type2",bag[1].y);
        PlayerPrefs.SetInt("SceneID",SceneManager.GetActiveScene().buildIndex);
        Time.timeScale=1;
    }
    public void Title()
    {
        SceneManager.LoadScene(1);
    }
    public void Quit()
    {
        Time.timeScale=1;
        PlayerPrefs.DeleteAll();
        SkillTree.GetComponent<SkillTree>().Remove();
        SceneManager.LoadScene(0);
    }
    public void GetHurt(int x)
    {
        Health-=x;
        HealthT.text=(Health+HealthAdd).ToString();
        UM.ShowHurt(x);
    }
}
