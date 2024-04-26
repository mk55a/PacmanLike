using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;



public class Player : MonoBehaviour
{
    public event Action hasConnectdWithBoundary;
    public event Action hasDisconnectedWithBoundary;
    public static event Action<float,float> OnSpeedBuff; 

    private GameObject _player;
    [SerializeField] private Transform _spriteTransform;
    [SerializeField] private Transform originObject;


    [SerializeField] private AudioClip occupyGrid;
    [SerializeField] private AudioClip powerUpEaten;

    private AudioSource playerAudioSource; 

    public PlayerControls _controls; 
    private Vector2 _input;
    private SpriteDirection _spriteDirection;

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
        
        _player = this.gameObject; 
        _controls = new PlayerControls();
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

        GetSpriteDirection();

        HandlePlayerCollision();

        GetPlayerGrid();
    }

    private void GetSpriteDirection()
    {
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

    public void HandlePlayerCollision()
    {
        hit = Physics2D.Raycast(transform.position, transform.right, 0.5f, LayerMask.GetMask("Boundary"));

        if (hit.collider != null && !hasConnected)
        {
            Debug.LogWarning("Collided with boundary");
            hasConnectdWithBoundary?.Invoke();

            GridManager.Instance.Connect();
            hasConnected = true;
        }
        else if (hit.collider == null)
        {
            hasDisconnectedWithBoundary?.Invoke();
            hasConnected = false;

        }
    }

    public void UsePowerUp(PowerUpType type, float powerUpSpeed, float time)
    {
        switch (type)
        {
            case PowerUpType.SpeedBuff:
                OnSpeedBuff?.Invoke(powerUpSpeed, time);
                return;

        }
    }


    public bool HasConnected()
    {
        return hasConnected;
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

    public Vector2 GetMovementInput()
    {
        return _input;
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


public enum PowerUpType
{
    SpeedBuff,
    Etc
}