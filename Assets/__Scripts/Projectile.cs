using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    public static float bottomY = -20f;

    const int LOOKBACK_COUNT = 10;
    static List<Projectile> PROJECTILES = new List<Projectile>();

    [SerializeField]
    private bool _awake = true;
    public bool awake
    {
        get {return _awake;}
        private set {_awake = value;}
    }

    private Vector3         prevPos;
    //private list stores history of projectile's move distance
    private List<float>     deltas = new List<float>();
    private Rigidbody       rigid;


    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        awake = true;
        prevPos = new Vector3(1000,1000,0);
        deltas.Add(1000);
        
        PROJECTILES.Add(this);
    }

    void FixedUpdate()
    {
        if(rigid.isKinematic || !awake) return;

        Vector3 deltaV3 = transform.position - prevPos;
        deltas.Add(deltaV3.magnitude);
        prevPos = transform.position;

        //limit lookback
        while(deltas.Count > LOOKBACK_COUNT)
        {
            deltas.RemoveAt(0);
        }

        //iterate over deltas and find greatest one
        float maxDelta = 0;
        foreach (float f in deltas)
        {
            if (f > maxDelta)maxDelta = f;
        }

        //if projectile hasn't moved more than sleep threashhold
        if (maxDelta <= Physics.sleepThreshold  || transform.position.y < bottomY)
        {
            //set awake to false and put rigidbody to sleep
            awake = false;
            rigid.Sleep();
        }
    }

    private void OnDestroy()
    {
        PROJECTILES.Remove(this);
    }

    static public void DESTROY_PROJECTILES()
    {
        foreach(Projectile p in PROJECTILES)
        {
            Destroy(p.gameObject);
        }
    }
    // void Update(){

    //     //delete projectile if it falls off map to prevent it perpetually falling
    //     if (transform.position.y < bottomY){
    //         Destroy (this.gameObject);
    //     }
    // }

    
}
