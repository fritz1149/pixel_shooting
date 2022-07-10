using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //引用
    private Transform Player;
    void Start()
    {
        Player=GameObject.FindGameObjectWithTag("Player").transform;
    }
    void Update()
    {
        transform.localPosition=new Vector3(Player.localPosition.x,Player.localPosition.y,transform.localPosition.z);
    }
}
