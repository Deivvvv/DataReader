using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerZone : MonoBehaviour
{
    public GameObject root;
    public pController player;
    public string tayp;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (root != other.gameObject)
        {
            switch (tayp)
            {
                case ("Echo"):
                    if (other.gameObject.GetComponent<Rigidbody>())
                    {
                        other.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, 300, 0));
                    }
                    break;
                case ("FlyObject"):
                    player.FlyObject(other.gameObject);
                    //if (other.gameObject.GetComponent<Rigidbody>())
                    //{
                    //    other.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, 300, 0));
                    //}
                    break;
            }
           
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (root != other.gameObject)
        {
            switch (tayp)
            {
                case ("Grab"):
                    if(other.gameObject.tag == "Object")
                        player.GrabItem(other.gameObject);
                 
                    break;
               
            }

        }

    }
}
