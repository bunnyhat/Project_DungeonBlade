using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulsLikeCharacterController : MonoBehaviour {
     /* add slow down when rotating later, just steal it off the third person script */
     [SerializeField]
    float moveSpeed;
   
    [SerializeField]
    Animator anim;
    [SerializeField]
    MeleeDetection meleeDetection; //used to determine what to attack in melee, shared with AI
    [SerializeField]
    CharacterController player;
    Vector3 moveDirection;
    public bool lockedOnTarget = false; //if locked on target, we no longer rotate when the camera turns. 
    public Transform lockOnTarget; //what we should rotate around if locked on. 

    [SerializeField]
    private CameraLockOnTarget camLock; //the lock on controller. 
    public Transform cameraPosition; //er...nothing?
    /// </summary>
    public List<GameObject> lockableEnemies = new List<GameObject>();
    private int targetLockIndex = 0; 
    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    //random keys currently to activate things.
    void Update () {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            /*Overall, this should be an event tied to the attack animation */
            foreach (IDamageable g in meleeDetection.melee) {
                g.takeDamage(15);
            }

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
        moveDirection = Vector3.zero;
        Vector3 forward = Camera.main.transform.TransformDirection(Vector3.forward);
        forward.y = 0;
        forward = forward.normalized;
        // Right vector relative to the camera
        // Always orthogonal to the forward vector
        Vector3 right = new Vector3(forward.z, 0, -forward.x);
        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");
        Vector3 targetDirection = h * right + v * forward;

       // float targetSpeed = Mathf.Min(targetDirection.magnitude, 1.0f);
        //moveSpeed = Mathf.Lerp(moveSpeed, targetSpeed, 0.4f);
        // Debug.Log(targetSpeed);
        if (targetDirection != Vector3.zero)
        {
            moveDirection = Vector3.RotateTowards(moveDirection, targetDirection, .3f * Mathf.Deg2Rad * Time.deltaTime, 1000);

            moveDirection = moveDirection.normalized;

        }

        Vector3 movement = moveDirection * moveSpeed;
        if (movement != Vector3.zero)
        {
           
            if (!lockedOnTarget)
            {
                  transform.rotation = Quaternion.LookRotation(moveDirection);
        
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
        player.Move(movement * moveSpeed);
        #endregion
    }

    

}
