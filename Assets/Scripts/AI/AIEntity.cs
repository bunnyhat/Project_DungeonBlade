using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


//base class for both friendly and hostile AI entities
public class AIEntity : MonoBehaviour, IDamageable {
    public int health = 50;
    public int maxHealth = 50;
    public bool canBeLockedOnTo = true;
    public NavMeshAgent nav;
    public enum AIStates { Chase, Idle, Attack }
    public AIStates state = AIStates.Idle;
    public MeleeDetection meleeDetection;
    public Detection detectionArea;
    public Animator anim;

    public float attackTimer = 1f;
    public float attackTimerMax = 1f;
    [SerializeField]
    DamageInformation meleeAttackInformation = new DamageInformation();
    public string getObjectName()
    {
        return this.name;
    }

    public bool takeDamage(DamageInformation damage, GameObject attacker)
    {
        gameObject.SetActive(false);
        return true;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
       

    }


    public virtual  void updateState()
    {
        if (meleeDetection.melee.Count > 0)
        {
            
            state = AIStates.Attack;
            
        }
        else if (detectionArea.area.Count > 0)
        {
       
            state = AIStates.Chase;

        }
        else
        {

            state = AIStates.Idle;
        }
    }
    public GameObject getGameObject()
    {
        throw new System.NotImplementedException();
    }

    void OnBecameVisible()
    {
        if (canBeLockedOnTo) {
            GameObject.FindGameObjectWithTag("Player").
                GetComponent<SoulsLikeCharacterController>().lockableEnemies.Add(gameObject);
        }
        
    }
    void OnBecameInvisible()
    {
        if (canBeLockedOnTo)
        {
            GameObject.FindGameObjectWithTag("Player").
             GetComponent<SoulsLikeCharacterController>().lockableEnemies.Remove(gameObject);
        }
    }
}
[System.Serializable]
public struct DamageInformation{
    //a struct to monitor things like armour piercing attacks or any additional effects. 
    public int damage;
    }
