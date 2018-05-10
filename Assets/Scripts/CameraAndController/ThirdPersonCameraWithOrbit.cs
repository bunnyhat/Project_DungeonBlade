using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCameraWithOrbit : MonoBehaviour {
    [SerializeField]
    private Transform idealPosition; //position behind player. seek toward it when moving.
    [SerializeField]
    private Transform player;
    private Vector3 oldPlayerPos;
    private Transform cameraTransform;
    private bool reOrientCamera = false;
    [Header("Movement variables")]
    [Space(22)]
    [SerializeField]
    public float distance = 25f;
   public float x = 0.0f;
    public float y = 0.0f;
    public float minYClamp = 30; //70  has least flight errors
    public float maxYClamp = 180;
    public Vector3 startingPos = Vector3.zero;
    // Use this for initialization
    void Start () {
        cameraTransform = GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void LateUpdate () {

        
        //distance and speed
        x += Input.GetAxis("Mouse X") ;
        y -= Input.GetAxis("Mouse Y") ;
     
        y = ClampAngle(y, minYClamp, maxYClamp);
        cameraTransform.LookAt(player);
        Quaternion rotation = Quaternion.Euler(  y,  x, 0);
          cameraTransform.rotation = rotation;
         distance+= -Input.GetAxis("Mouse ScrollWheel") * 2;
    
     
        Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
        Vector3 position = rotation * negDistance + player.position ;
        // cameraTransform.rotation = rotation;
        cameraTransform.rotation = rotation;
        cameraTransform.position = position;
        
        //cameraTransform.LookAt(player);
        //cameraTransform.position = idealPosition.position;
    }




    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }

    public void reset() {
        x = 0;
        y = 0;

    }
}


