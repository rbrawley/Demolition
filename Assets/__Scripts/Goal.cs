using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]

public class Goal : MonoBehaviour
{
    //static field accessible by code anywhere
    static public bool goalMet = false;

    void OnTriggerEnter( Collider other)
    {
        //when trigger is hit by something check to see if it's a projectile
        Projectile proj = other.GetComponent<Projectile>();
        if (proj != null)
        {
            //if so, set goalMetto true
            Goal.goalMet = true;
            //set alpha of color to higher opacity
            Material mat = GetComponent<Renderer>().material;
            Color c = mat.color;
            c.a = 0.75f;
            mat.color = c;

        }
    }
}
