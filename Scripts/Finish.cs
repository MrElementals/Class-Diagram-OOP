using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Finish : MonoBehaviour
{
    public GameObject fireworkPrefab;
    public GameObject r, g, b;
    public static bool victory;
    public Text Winner;


    private void Start()
    {
        victory = false;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Red")
        {
            fireworkPrefab = r;
            victory = true;
            this.GetComponent<Collider>().enabled = false;
            Winner.text = "RED WINS";
        }
        if (other.gameObject.tag == "Green")
        {
            fireworkPrefab = g;
            victory = true;
            this.GetComponent<Collider>().enabled = false;
            Winner.text = "GREEN WINS";
        }
        if (other.gameObject.tag == "Blue")
        {
            fireworkPrefab = b;
            victory = true;
            this.GetComponent<Collider>().enabled = false;
            Winner.text = "BLUE WINS";
        }
        if (victory)
        {
            for (int i = 0; i < 3; i++)
            {
                Instantiate(fireworkPrefab, transform.position, Quaternion.identity);
            }
        }

        

    }
   
}
