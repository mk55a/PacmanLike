using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
public class Player : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private Transform _spriteTransform;
    [SerializeField] private Transform originObject;
    [SerializeField]private float _speed;
    [SerializeField] private float _rotationSpeed; 
    [SerializeField]private LayerMask _layerMask;
    [SerializeField] private Rigidbody2D _rigidBody;

    private PlayerControls _controls; 
    private Vector2 _input;
    private SpriteDirection _spriteDirection;

    private int previousGridX, previousGridY;
    private float _movementSpeed;
    private RaycastHit2D hit; 

    public ContactFilter2D movementFilter; 
    private List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    public float collisionOffset = 0.05f;
    private void Update()
    {
    }
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
        HandlePlayerMovement(_input);
        _rigidBody.velocity = _input * _movementSpeed;
        GetGridXY(_player.transform.position);
    }
    public void HandlePlayerMovement(Vector2 direction)
    {
        hit = Physics2D.Raycast(transform.position, transform.right, 1.5f, LayerMask.GetMask("Boundary"));
        Debug.DrawRay(transform.position, transform.right*1.5f, Color.red);
       // Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 4f, Color.red);

        if (hit.collider!=null )
        {
            _movementSpeed = 0;
        }
        else
        {
            _movementSpeed = _speed;
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
            CaptureGrid();
        }
        previousGridX = x;
        previousGridY = y;  
        
        
    }
    private void CaptureGrid()
    {
        GridManager.Instance.PlayerOccupyGrid(previousGridX, previousGridY);    
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
public enum SpriteDirection
{
    Up,
    Down,
    Left,
    Right
}
