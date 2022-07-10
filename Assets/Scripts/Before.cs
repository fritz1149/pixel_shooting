using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Before : MonoBehaviour
{
    public GameObject St;
    void Awake()
    {
        St.GetComponent<SkillTree>().hide();
        
    }
}
