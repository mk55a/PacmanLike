using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Speed")]
public class SpeedBuff : PowerUp
{
    public float amount;
    public float lifeTime;
    public float duration;
    public Sprite sprite; 
    public override void Apply(GameObject target)
    {
        //target.GetComponent<Player>()._movementSpeed += amount; 
        target.GetComponent<Player>().UsePowerUp(amount, duration);
    }
    public override Sprite GetSprite()
    {
        return sprite;
    }
    public override float DestoryItself()
    {
        
        return lifeTime;    
    }
}
