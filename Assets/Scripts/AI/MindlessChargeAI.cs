using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MindlessChargeAI : AIEntity {
    [SerializeField]
    private List<GameObject> wayPoints = new List<GameObject>();
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        updateState();
        switch (state)
        {
            case AIStates.Idle:
                nav.SetDestination(wayPoints
                    [0].transform.position);
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
                break;
            case AIStates.Chase:
                anim.SetBool("walk", true);
                nav.Resume();
                nav.SetDestination(detectionArea.area[0].getGameObject().transform.position);
                break;
        }
    }

    
}
