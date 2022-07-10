using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class H : MonoBehaviour
{
    float Timer=0.0f;
    public GameObject Follow;
    public bool Activated=false;
    void FixedUpdate()
    {
        if(Activated)
        {
            Timer+=Time.deltaTime;
            if(Follow!=null) transform.localPosition=Follow.transform.localPosition+new Vector3(1,1,0);
            if(Timer>=0.4f) Destroy(gameObject);
        }
    }
}
