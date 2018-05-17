using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoulsLikeCharacterController : MonoBehaviour,IDamageable {
    /* add slow down when rotating later, just steal it off the third person script */
    [SerializeField]
    PlayerUIDisplay UIDisplay;
    [SerializeField]
    CharacterController player;
    [SerializeField]
    float moveSpeed;
    bool IsGrounded = true;
    [SerializeField]
    Animator anim;
    Vector3 moveDirection;
    Vector3 impact= Vector3.zero;

    [Space(5)]
    [Header("Combat and lockon")]
    private int health = 50;
    private int maxHealth = 50;
    public bool lockedOnTarget = false; //if locked on target, we no longer rotate when the camera turns. 
    public Transform lockOnTarget; //what we should rotate around if locked on. 
    [SerializeField]
    MeleeDetection meleeDetection; //used to determine what to attack in melee, shared with AI
    [SerializeField]
    private CameraLockOnTarget camLock; //the lock on controller. 

    bool isBlocking = false; //gonna have to update damage interface to allow block piercing. 
    private delegate void ClickEvent();
    private ClickEvent clickEvent;
    public Transform cameraPosition; //er...nothing?
    /// </summary>
    public List<GameObject> lockableEnemies = new List<GameObject>();
    private int targetLockIndex = 0;

    [Space(5)]
    [Header("Interaction stuff")]
    //intearction toolip variables. 
    private IInteractable interactionObject;
    private float tooltipTimer = 1f;
    [SerializeField]
    private GameObject crosshair;
    [SerializeField]
    private Text interactionDisplayText;


    bool one_click = false;
    bool timer_running = false;
    float timer_for_double_click = 0.2f;

    //this is how long in seconds to allow for a double click
    float delay = 0.3f;

    // Use this for initialization
    void Start () {
		
	}

    void dodge() {
        if (lockedOnTarget)
        {
            AddImpact(-transform.right);
        }
        else {
            AddImpact(transform.forward);
        }
    }

    // Update is called once per frame
    //random keys currently to activate things.
    void Update () {
        // impact vanishes to zero over time
        impact = Vector3.Lerp(impact, Vector3.zero, .1f);
     


        //reset movement each frame and check if grounded. 
        //y is used for jumping so not changed
        moveDirection.x = 0;
        moveDirection.z = 0;
        CheckGroundStatus();
        //block
        if (Input.GetKeyDown(KeyCode.Mouse2))
        {
            isBlocking = true;
        }
        else {
            isBlocking = false;
        }
        //jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (IsGrounded) {
                Debug.Log("jump");
                moveDirection.y = 2;

            }

        }
        //attack
        if (Input.GetKey(KeyCode.Mouse1))
        {
            anim.SetBool("Attack", true);
            /*Overall, this should be an event tied to the attack animation */
            attack();

        }
        else {
            anim.SetBool("Attack", false);
        }

        #region interaction
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (interactionObject != null)
            {
                interactionObject.interact(gameObject);
            }
        }



        tooltipTimer -= Time.deltaTime;
        if (tooltipTimer < 0)
        {
            interactionObject = null;
            tooltipTimer = .3f;
            RaycastHit hit;
            Vector3 fwd = transform.TransformDirection(Vector3.forward);
            Ray ray = Camera.main.ScreenPointToRay(crosshair.transform.position);


            Debug.DrawRay(transform.position, ray.direction, Color.blue, 3f);
            if (Physics.Raycast(ray, out hit, 45))
            {
                interactionObject = hit.collider.GetComponent<IInteractable>();

                if (interactionObject != null)
                {
                  //  Debug.Log("Raycast Tooltip hit something" + hit.collider.gameObject);
                    interactionDisplayText.text = "[F] " + interactionObject.getObjectName();

                }
                else
                {
                    interactionDisplayText.text = "";
                }
            }
            else
            {
                interactionDisplayText.text = "";
            }
            #endregion
        }



        #region targetLocking
        if (Input.GetKeyDown(KeyCode.LeftControl)) {


            if (lockableEnemies.Count != 0)
            {
               
                lockedOnTarget = !lockedOnTarget;
                targetLockIndex = 0;

                //add lerp to movement later. 
                camLock.target = lockableEnemies[targetLockIndex].transform;
                lockOnTarget = lockableEnemies[targetLockIndex].transform; //we need to know what to rotate around to turn
                camLock.positionCamera();
                camLock.enabled = lockedOnTarget;
            }
          
        }

        if (Input.GetKeyDown(KeyCode.Q)) {
            if (lockedOnTarget) {
                if (lockableEnemies.Count != 0) {
                    targetLockIndex++;
                    if (targetLockIndex > lockableEnemies.Count - 1) {
                        targetLockIndex = 0;
                    }
                    camLock.target = lockableEnemies[targetLockIndex].transform;
                    lockOnTarget = lockableEnemies[targetLockIndex].transform;
                    camLock.positionCamera();

                }
            }
        }

        if (lockedOnTarget) {
            transform.LookAt(lockOnTarget);
        }
        #endregion
        #region movement
     
        Vector3 forward = Camera.main.transform.TransformDirection(Vector3.forward);
        forward.y = 0;
        forward = forward.normalized;
        // Right vector relative to the camera
        // Always orthogonal to the forward vector
        Vector3 right = new Vector3(forward.z, 0, -forward.x);
        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");
        Vector3 targetDirection = h * right + v * forward;
        clickEvent = dodge;
        doubleClick(KeyCode.A, clickEvent);

        // float targetSpeed = Mathf.Min(targetDirection.magnitude, 1.0f);
        //moveSpeed = Mathf.Lerp(moveSpeed, targetSpeed, 0.4f);
        // Debug.Log(targetSpeed);
        if (targetDirection != Vector3.zero)
        {
            float t = moveDirection.y;
            moveDirection.y = 0;
            moveDirection = Vector3.RotateTowards(moveDirection, targetDirection, .3f * Mathf.Deg2Rad * Time.deltaTime, 1000);
            moveDirection.y = t;
            moveDirection = moveDirection.normalized;
            moveDirection.y = t; //I think I added extra stuff here in my wild error fixing. 

        }
        Vector3 movement = moveDirection * moveSpeed;      
        if (movement != Vector3.zero)
        {
           
            if (!lockedOnTarget)
            {
                float t = moveDirection.y;
                moveDirection.y = 0;
                if (moveDirection != Vector3.zero)
                {
                    transform.rotation = Quaternion.LookRotation(moveDirection);
                }
                moveDirection.y = t;

            } 
            else {
                //if locked, just face the target.
                //    transform.LookAt(lockOnTarget);
             //   Camera.main.transform.LookAt(this.transform.position);
            }
            // Do the rotation here
        }


        movement *= Time.deltaTime;

        if (movement != Vector3.zero)
        {
            anim.SetBool("walk", true);
        }
        else
        {
            anim.SetBool("walk", false);
        }

        moveDirection.y -= 2 * Time.deltaTime;
        moveDirection.y = Mathf.Clamp(moveDirection.y, -2, moveDirection.y);
         movement =  movement + impact;
        player.Move(movement * moveSpeed );
        #endregion
    }

    private void attack()
    {
        DamageInformation d = new DamageInformation();
        foreach (IDamageable g in meleeDetection.melee)
        {
            g.takeDamage(d, this.gameObject);
        }
    }

    void CheckGroundStatus()
    {
        RaycastHit hitInfo;
#if UNITY_EDITOR
        // helper to visualise the ground check ray in the scene view
        Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + (Vector3.down * 2));
#endif
        // 0.1f is a small offset to start the ray from inside the character
        // it is also good to note that the transform position in the sample assets is at the base of the character
        if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, 2))
        {
         
            IsGrounded = true;
        
        }
        else
        {
            IsGrounded = false;
        
        }
    }
   
    void AddImpact(Vector3 force)
    {
        Vector3 dir = force.normalized;
        impact += dir.normalized * 0.1f;
    }

   private float lastClickTime;

    public float catchTime = 0.25f;
    private KeyCode lastKey;
    void doubleClick(KeyCode key, ClickEvent action )
    {

        if (Input.GetKeyDown(key))
        {
          
            if (Time.time - lastClickTime < catchTime && lastKey == key )
            {
                //double click
                print("Double click");
                action();
            }
            else
            {
                //normal click
                lastKey = key;
            }
            lastClickTime = Time.time;
        }
    }




    public string getObjectName()
    {
        throw new System.NotImplementedException();
    }

    public bool takeDamage(DamageInformation damage, GameObject attacker)
    {
        Vector3 direction = (transform.position - attacker.transform.position);
     //   AddImpact(direction  * 0.1f);
        if (isBlocking) {
            return false;
        }
        health -= damage.damage;
        UIDisplay.updateHealth(health, maxHealth);
        UIDisplay.flickerScreen();
        return true;
    }

    public GameObject getGameObject()
    {
        return gameObject; 
    }
}

    


