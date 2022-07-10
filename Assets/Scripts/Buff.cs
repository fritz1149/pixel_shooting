using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : MonoBehaviour
{
    public bool vist;
    public virtual void Add(PlayerController PlayerC){}
    public virtual void RollBack(){}
}
