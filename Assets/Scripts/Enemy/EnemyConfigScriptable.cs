using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class EnemyConfigScriptable : ScriptableObject
{
    public float colliderRadius; 
    public float chaseDuration;
    public bool isChaseEnabled; 
}
