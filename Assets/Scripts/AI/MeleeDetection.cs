using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeDetection : MonoBehaviour
{
    //will add bool to change behavouir for when the player uses it if needed. 
    public List<IDamageable> melee = new List<IDamageable>();
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "NPC" || other.gameObject.tag == "Player")
        {
            if (other.gameObject == transform.parent.gameObject)
            {
                return;
            }

            IDamageable newNPC = other.gameObject.GetComponent<IDamageable>();

            foreach (IDamageable g in melee)
            {
                if (g == newNPC)
                {
                    return;
                }
            }
            //if no duplicate
            // Debug.Log(gameObject.transform.parent.name + " added enemy " + other.gameObject.name);
            melee.Add(newNPC);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "NPC" || other.gameObject.tag == "Player")
        {
            melee.Remove(other.GetComponent<IDamageable>());
        }
    }

}
