using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MindlessChargeAI : AIEntity {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        updateState();
        switch (state)
        {
            case AIStates.Idle:
                //whether to dile or walk somewhere
                /*        if (hasDest)
                        {
                            anim.SetBool("walk", true);
                        }
                        else
                        {
                            anim.SetBool("walk", false);
                        }*/
                break;
            case AIStates.Attack:
                nav.Stop();
                attackTimer -= Time.deltaTime;
                anim.SetBool("walk", false);
                if (attackTimer < 0)
                {

                    anim.SetTrigger("attack");
                    Vector3 dir = meleeDetection.melee[0].getGameObject().transform.position;
                    dir.y = transform.position.y;
                    transform.LookAt(dir);
               
                    attackTimer = attackTimerMax;
                    foreach (IDamageable g in meleeDetection.melee)
                    {

                        g.takeDamage(5);
                        if (!g.getGameObject().activeSelf)
                        {
                            meleeDetection.melee.Remove(g);
                            detectionArea.area.Remove(g);

                        }
                        return;
                    }
                }
                break;
            case AIStates.Chase:
                anim.SetBool("walk", true);
                nav.Resume();
                nav.SetDestination(detectionArea.area[0].getGameObject().transform.position);
                break;
        }
    }

    
}
