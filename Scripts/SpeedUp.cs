using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUp : PowerUps
{
    private void Start()
    {
        speed = Random.Range(2,4);
        duration = Random.Range(0.5f, 4);

    }
    public override void Behaviour(Rigidbody t)
    {
        if (triggered)
        {
            time = 0;
        }
        if (!active && triggered)
        {
            myOriginalSpeed = t.velocity;
            active = true;
            t.velocity *= speed;
            triggered = false;
        }
        if (active)
        {
            time += Time.deltaTime;
            if (time > duration)
            {
                t.velocity = myOriginalSpeed;
                active = false;
                time = 0;
            }
        }
    }
    public void Update()
    {
        Behaviour(this.GetComponent<Rigidbody>());
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Fast")
        {
            triggered = true;
        }
    }
}
