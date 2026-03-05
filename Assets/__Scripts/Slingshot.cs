using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour{

    [SerializeField] private LineRenderer rubber;

    [SerializeField] private Transform leftArm;

    [SerializeField] private Transform rightArm;
    [SerializeField] private AudioClip rubberClip;

    [Header("Inscribed")]
    public GameObject                   projectilePrefab;
    public float                        velocityMult = 10f;
    public GameObject                   projLinePrefab;


    [Header("Dynamic")]
    public GameObject                   launchPoint;
    public Vector3                      launchPos;
    public GameObject                   projectile;
    public bool                         aimingMode;

    void Start()
    {
        //temporary values to have rubber's left and right positions set at top of slingshot instead of middle of arms
        Vector3 tempLeft = leftArm.position;
        Vector3 tempRight = rightArm.position;
        tempLeft.y +=1;
        tempRight.y +=1;

        //sets the position of the rubber to the left and right arm spots
        rubber.SetPosition(0, tempLeft);
        rubber.SetPosition(2, tempRight);

    }

    void Awake(){
       Transform launchPointTrans = transform.Find("LaunchPoint"); 
       launchPoint = launchPointTrans.gameObject;
       launchPoint.SetActive(false);
       launchPos = launchPointTrans.position;
    }
    void OnMouseEnter(){
        //print("Slingshot:OnMouseEnter()");
        launchPoint.SetActive(true);
    }

    void OnMouseExit(){
        //print("Slingshot:OnMouseExit()");
        launchPoint.SetActive(false);

    }

    void OnMouseDown(){
        //pressed mouse button while over slingshot
        aimingMode = true;
        //instantiate a projectile
        projectile = Instantiate(projectilePrefab) as GameObject;
        //Start at LaunchPoint
        projectile.transform.position = launchPos;
        //set it to iskinematic
        projectile.GetComponent<Rigidbody>().isKinematic = true;
    }

    void Update(){
        //if not in aim mode, don't run
        if(!aimingMode) return;

        //get position of mouse for the rubber?
        // if (Input.GetMouseButton(0)){
        //     rubber.SetPosition(1, GetMousePos());
        // }

        //get current mouse position in 2d
        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);

        //find the delta from the launchpos to the mousepos3d
        Vector3 mouseDelta = mousePos3D - launchPos;
        //limit mousedelta to radius of the slingshot sphere collider
        float maxMagnitude = this.GetComponent<SphereCollider>().radius;
        if (mouseDelta.magnitude > maxMagnitude){
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitude;
        }

        //move the projectile to this new position
        Vector3 projPos = launchPos+mouseDelta;
        projectile.transform.position = projPos;

        //test 
        rubber.SetPosition(1, projPos);

        if (Input.GetMouseButtonUp(0))
        {
            aimingMode = false;
            Rigidbody projRB = projectile.GetComponent<Rigidbody>();
            projRB.isKinematic = false;
            projRB.collisionDetectionMode = CollisionDetectionMode.Continuous;
            projRB.velocity = -mouseDelta * velocityMult;

            //switch to slingshot view immediately before setting POI
            FollowCam.SWITCH_VIEW(FollowCam.eView.slingshot);
            FollowCam.POI = projectile;
            //add a projectileline to the projectile
            Instantiate<GameObject>(projLinePrefab, projectile.transform);
            projectile = null;
            SFXManager.S.PlaySFX(rubberClip, transform, 1f);
            MissionDemolition.SHOT_FIRED();
        }
    }


    //get mouse position to determine rubber's position
    Vector3 GetMousePos()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z += Camera.main.transform.position.z;
        Vector3 mousePositionInWorld = Camera.main.ScreenToWorldPoint(mousePosition);

        // //set band to Radius
        // if(mousePositionInWorld.magnitude > RubberConfig.Radius)
        // {
        //     mousePositionInWorld.Normalize();
        //     mousePositionInWorld *= RubberConfig.Radius;
        // }

        return mousePositionInWorld - transform.position;
    }
}
