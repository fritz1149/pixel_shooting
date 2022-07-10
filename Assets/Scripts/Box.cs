using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Diagnostics;
using System.Threading;
public class Box : MonoBehaviour
{
    public int Total=4,iRoot,id,type;
    public byte [] buffer;
    System.Random rd;
    void Start()
    {
        buffer=Guid.NewGuid().ToByteArray();
        iRoot=BitConverter.ToInt32(buffer,0);
        rd=new System.Random(iRoot);
        id=rd.Next(1,Total);
        type=rd.Next(0,2);
    }
    void Update()
    {
        
    }
}
