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
    [SerializeField]private float _speed;
    [SerializeField] private float _rotationSpeed; 
    
    [SerializeField] private Rigidbody2D _rigidBody;

    private PlayerControls _controls; 
    private Vector2 _input;
    private SpriteDirection _spriteDirection;


    private float _movementSpeed;
    private RaycastHit2D hit;
    private RaycastHit2D objectRaycast;

    [SerializeField] private LayerMask restrictionLayerMask;
    [SerializeField]
    private LayerMask interactionLayerMasks;



    private int previousGridX, previousGridY;
    
    private void Awake()
    {
        _controls = new PlayerControls();

        _rigidBody = GetComponent<Rigidbody2D>();   
        if(_rigidBody is null)
        {
            Debug.LogError("RB is null! ");
        }
        _movementSpeed = _speed;
        
          

    }

    private void OnEnable()
    {
        _controls.Player.Enable();
    }
    private void OnDisable()
    {
        _controls.Player.Disable();   
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
        _rigidBody.velocity = _input * _movementSpeed;
        HandlePlayerCollisions(_player.transform.position);
    }
    public void HandlePlayerMovement()
    {
        hit = Physics2D.Raycast(transform.position, transform.right, 1.5f, LayerMask.GetMask("Boundary"));
        Debug.DrawRay(transform.position, transform.right*1.5f, Color.red);

        if (hit.collider!=null )
        {
            _movementSpeed = 0;
        }
        else
        {
            _movementSpeed = _speed;
        }
    }

    private void HandlePlayerCollisions(Vector3 position)
    {
        objectRaycast = Physics2D.Raycast(transform.position, transform.right, 1.5f, interactionLayerMasks);
        //Debug.DrawRay(transform.position, transform.right * 1.5f, Color.green);
        //Debug.Log(objectRaycast.collider);
        if (objectRaycast.collider != null)
        {
            string collidedLayerName = LayerMask.LayerToName(objectRaycast.collider.gameObject.layer); //LayerMask.LayerToName(objectRaycast.collider.gameObject.layer);
            Interactables collidedInteractable = (Interactables)Enum.Parse(typeof(Interactables), collidedLayerName);
            switch (collidedInteractable)
            {
                case Interactables.PathGrid:
                    //Debug.Log("path");
                    int x, y;
                    x = Mathf.FloorToInt((position - GridManager.Instance.originObject.transform.position).x / GridManager.Instance.gridCellSize);
                    y = Mathf.FloorToInt((position - GridManager.Instance.originObject.transform.position).y / GridManager.Instance.gridCellSize);
                    GetGridXY(position);
                    GridManager.Instance.Connect();
                    break;
                case Interactables.BlueGrid:

                    Debug.Log("<color=blue>BLUE</color> ");
                    //GridManager.Instance.CalculateCapturedGrid();
                    /*int x, y;
                    x = Mathf.FloorToInt((position - GridManager.Instance.originObject.transform.position).x / GridManager.Instance.gridCellSize);
                    y = Mathf.FloorToInt((position - GridManager.Instance.originObject.transform.position).y / GridManager.Instance.gridCellSize);
                    GridManager.Instance.Connect(x, y);
                    GridManager.Instance.CheckAround(x, y);*/
                    break;
                case Interactables.Grid:

                    //Debug.Log("GRID");
                    GetGridXY(position);
                    break;

                case Interactables.Enemy:
                    Debug.Log("<color=red>ENEMY</color>");
                    break;
                case Interactables.PowerUp:

                    break;
                default:
                    Debug.Log("D");
                    break;

            }
        }
        //Debug.Log("<color=yellow>"+objectRaycast.collider.name+"</color>");
        else
        {
            Debug.LogError("Object reference is null");
        }

    }
    
    private void UpdatePlayerSprite()
    {
        switch( _spriteDirection )
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

    private void GetGridXY(Vector3 postion)
    {
        int x, y;
        x=Mathf.FloorToInt((postion-GridManager.Instance.originObject.transform.position).x/GridManager.Instance.gridCellSize);
        y=Mathf.FloorToInt((postion-GridManager.Instance.originObject.transform.position).y/GridManager.Instance.gridCellSize);
        if (previousGridX != x || previousGridY != y)
        {
            Debug.LogWarning("Generating a blue/path grid");
            if (GridManager.Instance.grid.blueGridArray[previousGridX, previousGridY] != null || GridManager.Instance.grid.pathGridArray[previousGridX, previousGridY] != null || GridManager.Instance.grid.boundaryGridArray[previousGridX,previousGridY] !=null)
            {
                CaptureGridImmediate();
            }
            else
            {
                CaptureGrid();
            }
            
        }
        previousGridX = x;
        previousGridY = y;  
        
        
    }
    private void CaptureGridImmediate()
    {
        //GridManager.Instance.PlayerOccupyPathGrid(previousGridX, previousGridY);
    }
    private void CaptureGrid()
    {
        
        GridManager.Instance.PlayerOccupyPathGrid(previousGridX, previousGridY);    
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
}
public enum PowerUps
{
     
}
public enum Interactables
{
    PathGrid,
    BlueGrid,
    Grid,
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
