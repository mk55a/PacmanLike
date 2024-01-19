using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpObject : MonoBehaviour
{
    [SerializeField]
    public PowerUp powerUp;

    [SerializeField]
    private SpriteRenderer spriteRenderer;
    private void OnEnable()
    {
        spriteRenderer.sprite = powerUp.GetSprite(); 
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
        powerUp.Apply(collision.gameObject);
    }
}
