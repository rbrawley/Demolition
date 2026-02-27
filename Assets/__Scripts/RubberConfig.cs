using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="Configuration", menuName = "Config")]
public class RubberConfig : ScriptableObject
{
   [field: SerializeField] public int Radius {get; private set;} = 5;
}
