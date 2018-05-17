using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAI : AIEntity {
    //uniquely spawns a ranged attack prefab
    public GameObject rangedAttackEffect;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        updateState();
        switch (state)
        {
            case AIStates.Idle:
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
                    transform.LookAt(dir * Time.deltaTime);
                    //     Debug.Log("hostile animal attacked");
                    attackTimer = attackTimerMax;
                    //if the enemy is too close, just do a regular attack. 
                    if (Vector3.Distance(transform.position, meleeDetection.melee[0].getGameObject().transform.position) < 3)
                    {
                        foreach (IDamageable g in meleeDetection.melee)
                        {

                            DamageInformation d = new DamageInformation();
                            g.takeDamage(d, gameObject);
                            if (!g.getGameObject().activeSelf)
                            {
                                meleeDetection.melee.Remove(g);
                                detectionArea.area.Remove(g);

                            }
                            return;
                        }

                    }
                    else
                    {
                        Instantiate(rangedAttackEffect, this.transform.position, this.transform.rotation);
                    }
                }
                //aside from regular attack, spawn prefab in too. 

                break;
            case AIStates.Chase:
                anim.SetBool("walk", true);
                nav.Resume();
                nav.SetDestination(detectionArea.area[0].getGameObject().transform.position);
                break;
        }
    }
}
