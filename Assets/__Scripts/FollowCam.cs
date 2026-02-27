using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    static public GameObject POI; //static point of interest

    [Header("Inscribed")]
    public float                    easing = 0.05f;
    public Vector2                  minXY = Vector2.zero; 

    [Header("Dynamic")]
    public float camZ; //desired z pos of camera

    void Awake()
    {
        camZ = this.transform.position.z;
    }

    void FixedUpdate()
    {
        // if(POI == null) return; //if no POI, then return

        // //get position of POI
        // Vector3 destination = POI.transform.position;

        Vector3 destination = Vector3.zero;

        if(POI != null)
        {
            //if POI  has a rigidbody, check if it's sleeping
            Rigidbody poiRigid = POI.GetComponent<Rigidbody>();
            if((poiRigid !=null) && poiRigid.IsSleeping())
            {
                POI = null;
            }
        }

        if (POI != null)
        {
            destination = POI.transform.position;
        }

        
        //limit min values of destination.x and destination.y
        destination.x = Mathf.Max(minXY.x, destination.x);
        destination.y = Mathf.Max(minXY.y, destination.y);
        //interpolate from current camera position
        destination = Vector3.Lerp(transform.position, destination, easing);
        //force destination.z to be camz to keep camera far enough away
        destination.z = camZ;
        //set camera to destination
        transform.position = destination;

        //set orthographic size of camera to keep ground in view
        Camera.main.orthographicSize = destination.y +10;

    }

}
