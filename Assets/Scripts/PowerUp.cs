using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUp : ScriptableObject
{
    public abstract void Apply(GameObject target);
    public abstract Sprite GetSprite();
}
