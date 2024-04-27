using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Rigidbody2D _rigidBody;

    [SerializeField] public float _speed;
    [SerializeField] private float _rotationSpeed;

    private Vector2 _input;
    private SpriteDirection _spriteDirection;

    private float _movementSpeed;
    private RaycastHit2D hit;

    private void OnEnable()
    {
        Player.Instance.hasConnectdWithBoundary += StopMovement;
        Player.Instance.hasDisconnectedWithBoundary += AllowMovement;
        Player.OnSpeedBuff += UseSpeedBuff;
    }
    private void OnDisable()
    {
        Player.Instance.hasConnectdWithBoundary += StopMovement;
        Player.Instance.hasDisconnectedWithBoundary += AllowMovement;
        Player.OnSpeedBuff += UseSpeedBuff;
    }

    private void Awake()
    {
        
        _rigidBody = GetComponent<Rigidbody2D>();

        if( _rigidBody == null)
        {
            Debug.LogError("No rigidbody");
        }

        _movementSpeed = _speed;
    }

    
    void Start()
    {
        
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        _input = Player.Instance.GetMovementInput();

        _rigidBody.velocity = _input * _movementSpeed;
    }

    public void StopMovement()
    {
        _movementSpeed = 0;
    }

    public void AllowMovement()
    {
        _movementSpeed = _speed;
    }

    private void UseSpeedBuff(float powerUpSpeed, float time)
    {
        StartCoroutine(UsePowerUpCoroutine(powerUpSpeed, time));
    }

    public IEnumerator UsePowerUpCoroutine(float powerUpSpeed, float time)
    {
        float startTime = Time.time;
        float oldSpeed = _speed;
        while (Time.time - startTime < time)
        {
            _speed = powerUpSpeed;
            yield return null;
        }
        _speed = oldSpeed;

        yield return null;
    }
}
