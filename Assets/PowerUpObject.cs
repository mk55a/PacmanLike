using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpObject : MonoBehaviour
{
    [SerializeField]
    public PowerUp powerUp;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    private float lifeTime; 

    private void OnEnable()
    {
        spriteRenderer.sprite = powerUp.GetSprite();
        lifeTime = powerUp.DestoryItself();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
        powerUp.Apply(collision.gameObject);
    }
    private void Update()
    {
        if(lifeTime > 0)
        {
            lifeTime -= Time.deltaTime;
        }
        else
        {
            Destroy(gameObject );
        }
    }

}
