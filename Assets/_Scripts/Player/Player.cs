using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{ 
    [SerializeField] private GameObject _player;
    [SerializeField] private Transform _spriteTransform;
    [SerializeField] private Transform originObject;
    [SerializeField]public float _speed;
    [SerializeField] private float _rotationSpeed; 
    
    [SerializeField] private Rigidbody2D _rigidBody;


    [SerializeField] private AudioClip occupyGrid;
    [SerializeField] private AudioClip powerUpEaten;

    private AudioSource playerAudioSource; 

    private PlayerControls _controls; 
    private Vector2 _input;
    private SpriteDirection _spriteDirection;


    public float _movementSpeed;
    private RaycastHit2D hit;


    [SerializeField] private LayerMask restrictionLayerMask;
    [SerializeField]
    private LayerMask interactionLayerMasks;


    private bool hasConnected = false;

    public static Player Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType(typeof(Player)) as Player;
            }
            return instance;
        }
        set
        {
            instance = value;   
        }
    }
    private static Player instance;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        _controls = new PlayerControls();

        _rigidBody = GetComponent<Rigidbody2D>();   
        if(_rigidBody is null)
        {
            Debug.LogError("RB is null! ");
        }
        _movementSpeed = _speed;
        
        playerAudioSource = GetComponent<AudioSource>();    

    }
    private void OnEnable()
    {
        _controls.Player.Enable();
    }
    private void OnDisable()
    {
        _controls.Player.Disable();
    }
    public void EnableControls(bool enable)
    {
        if (enable)
        {
            _controls.Player.Enable();
        }
        else if (!enable)
        {
            _controls.Player.Disable();
        }
    }
    private void FixedUpdate()
    {
        _input = _controls.Player.Movement.ReadValue<Vector2>();
        //Debug.LogWarning(_input);
        if (_input.x > 0)
        {
            _spriteDirection = SpriteDirection.Right;
        }
        else if (_input.x < 0)
        {
            _spriteDirection = SpriteDirection.Left;
        }
        else if (_input.y < 0)
        {
            _spriteDirection = SpriteDirection.Down;
        }
        else if (_input.y > 0)
        {
            _spriteDirection = SpriteDirection.Up;
        }
        UpdatePlayerSprite();
        HandlePlayerMovement();
        //Debug.LogWarning(_movementSpeed);
        _rigidBody.velocity = _input * _movementSpeed;
        GetPlayerGrid();
    }
    private void UpdatePlayerSprite()
    {
        switch (_spriteDirection)
        {
            case SpriteDirection.Left:
                _player.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
                break;
            case SpriteDirection.Right:
                _player.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                break;
            case SpriteDirection.Up:
                _player.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
                break;
            case SpriteDirection.Down:
                _player.transform.rotation = Quaternion.Euler(0f, 0f, -90f);
                break;
            default:
                return;
        }
    }
    public void HandlePlayerMovement()
    {
        hit = Physics2D.Raycast(transform.position, transform.right, 0.5f, LayerMask.GetMask("Boundary"));


        if (hit.collider!=null && !hasConnected)
        {
            _movementSpeed = 0;
            Debug.LogWarning("Collided with boundary");

            GridManager.Instance.Connect();
            hasConnected = true;
        }
        else if(hit.collider == null)
        {
            hasConnected = false;
            _movementSpeed = _speed;
        }
    }


    private void GetPlayerGrid()
    {
        GridMapObject gridObject = GridManager.Instance.grid.GetGridObject(transform.position);
        if(gridObject != null)
        {
            if (gridObject.GetType() == GridType.Grid)
            {
                GridManager.Instance.SetGridAsPath(gridObject, transform.position);
                playerAudioSource.clip = occupyGrid;
                playerAudioSource.Play();
            }
        }
    }

    public Vector3 GetPlayerPosition()
    {
        return gameObject.transform.position;
    }
    public Vector2 GetPlayerWorldPosition()
    {
        Vector2  pos = GetPlayerWorldRotation(_player, Camera.main);
        
        return pos;
    }
    private Vector2 GetPlayerWorldRotation(GameObject player ,Camera worldCamera) {
        Vector2 worldPosition = worldCamera.ScreenToWorldPoint(player.transform.position);
        return worldPosition;
    }

    public void UsePowerUp(float powerUpSpeed, float time)
    {
        StartCoroutine(UsePowerUpCoroutine(powerUpSpeed, time));
    }
    public IEnumerator UsePowerUpCoroutine(float powerUpSpeed, float time)
    {
        float startTime = Time.time;
        float oldSpeed = _speed;
        while (Time.time - startTime <time)
        {
            _speed = powerUpSpeed;
            yield return null;
        }
        _speed = oldSpeed;


    }
}
public enum Interactables
{
    PathGrid,
    BlueGrid,
    Grid,
    Boundary,
    Enemy,
    PowerUp
}
public enum SpriteDirection
{
    Up,
    Down,
    Left,
    Right
}
