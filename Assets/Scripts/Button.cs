using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public int x,y;
    SkillTree ST;
    void Awake()
    {
        ST=GameObject.FindGameObjectWithTag("SkillTree").GetComponent<SkillTree>();
    }
    public void Click()
    {
        ST.LightUp(x,y);
    }
}
