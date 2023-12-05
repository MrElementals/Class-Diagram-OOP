using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUps : MonoBehaviour
{
    protected float speed;
    protected float duration;
    protected bool active;
    protected bool triggered;
    protected float time;
    protected Vector3 myOriginalSpeed;
    public virtual void Behaviour(Rigidbody target)
    {
    }
 
}

//Speed
//Slow

