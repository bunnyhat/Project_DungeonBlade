using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapDamagePlayer : MonoBehaviour {
    //should probably be divided into one that damages constantly and one that just has an effect when entered. 
    
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player") {
            other.gameObject.GetComponent<IDamageable>().takeDamage(5);
            //play burn effect and take damage. 
        }
    }

}
