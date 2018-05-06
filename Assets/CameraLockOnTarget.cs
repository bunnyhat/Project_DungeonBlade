using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLockOnTarget : MonoBehaviour {
    //For Locking the character onto a a target like in dark souls. 
    //goal is to keep the camera behind the player and facing the enemy. 
    //add check to stop two types of scripts being on at the same time later. 
    public Transform target;
    public Transform positionBehindPlayer;
	// Use this for initialization
	void Start () {
		
	}

    private void OnEnable()
    {
        this.transform.position = positionBehindPlayer.position;
        this.transform.parent = positionBehindPlayer.transform.parent;
    }


    // Update is called once per frame
    void Update () {
        transform.LookAt(target);
	}
}
