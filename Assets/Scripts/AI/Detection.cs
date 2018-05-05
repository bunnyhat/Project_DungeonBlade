using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detection : MonoBehaviour
{
    /* This can get awkward with detecting things it shouldn't or being too rigid if you add too much
     * but until that actually comes up it's acceptable as long as you make sure it can't detect random child objects */
    public List<IDamageable> area = new List<IDamageable>();
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

            foreach (IDamageable g in area)
            {
                if (g == newNPC)
                {
                    return;
                }
            }
            //if no duplicate
            // Debug.Log(gameObject.transform.parent.name + " added enemy " + other.gameObject.name);
            area.Add(newNPC);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "NPC" || other.gameObject.tag == "Player")
        {
            area.Remove(other.GetComponent<IDamageable>());
        }
    }

}
