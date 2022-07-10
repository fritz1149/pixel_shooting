using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    bool FailToLoad=false;
    float Timer=0.0f;
    //引用
    public GameObject T;
    void FixedUpdate()
    {
        if(FailToLoad)
        {
            Timer+=Time.deltaTime;
            if(Timer>=2.0f)
            {
                T.SetActive(false);
                Timer=0.0f;
                FailToLoad=false;
            }
        }
    }
    public void NewStart()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("Health",40);
        PlayerPrefs.SetInt("Magic",100);
        PlayerPrefs.SetInt("WeapFocus",1);
        PlayerPrefs.SetInt("FocusType",1);
        PlayerPrefs.SetInt("id1",0);
        PlayerPrefs.SetInt("type1",0);
        PlayerPrefs.SetInt("id2",0);
        PlayerPrefs.SetInt("type2",1);
        Time.timeScale=1;
        SceneManager.LoadScene(2);
    }
    public void Load()
    {
        if(PlayerPrefs.HasKey("SceneID"))
            SceneManager.LoadScene(PlayerPrefs.GetInt("SceneID"));
        else
        {
            T.SetActive(true);
            FailToLoad=true;
        }
    }
    public void Quit()
    {
        Application.Quit();
    }
}
