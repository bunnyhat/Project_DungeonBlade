using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLockOnTarget : MonoBehaviour {
    //For Locking the character onto a a target like in dark souls. 
    //goal is to keep the camera behind the player and facing the enemy. 
    //add check to stop two types of scripts being on at the same time later. 
    public Transform player;

    public Transform target;
    public Transform positionBehindPlayer;
    private ThirdPersonCameraWithOrbit normalCameraMode;
	// Use this for initialization
	void Start () {
        normalCameraMode = GetComponent<ThirdPersonCameraWithOrbit>();
	}

    private void OnEnable()
    {
      
    }
    private void OnDisable()
    {
        //send rotation to normal orbit script to stop it jumping
        this.transform.parent = null;
        normalCameraMode.y = this.transform.localRotation.eulerAngles.x;
        normalCameraMode.x = this.transform.localRotation.eulerAngles.y;
        normalCameraMode.distance =Vector3.Distance(this.transform.position, player.position);     
        normalCameraMode.enabled = true;

  
    }

    // Update is called once per frame
    void Update () {
        transform.LookAt(player);
        this.transform.position = positionBehindPlayer.transform.position; //not efficent at all. 
	}

    public void positionCamera() {
        if (normalCameraMode == null)
        {
            normalCameraMode = GetComponent<ThirdPersonCameraWithOrbit>();
        }
        player.LookAt(target);
        normalCameraMode.enabled = false;
        positionBehindPlayer.transform.LookAt(target);
        this.transform.position = positionBehindPlayer.position;
        this.transform.parent = positionBehindPlayer.transform.parent;
    }
}
