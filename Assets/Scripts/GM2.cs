using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM2 : MonoBehaviour
{
    GameObject ST;
    void Start()
    {
        ST=GameObject.FindGameObjectWithTag("SkillTree");
        ST.GetComponent<SkillTree>().Init();
    }
}
