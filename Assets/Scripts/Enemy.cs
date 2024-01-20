using EmptyCharacter.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    
    [SerializeField] private EnemyPathfindingMovementHandler movementHandler;
    [SerializeField] private LayerMask restrictionLayer;
    [SerializeField] private LayerMask pathGridLayer; 
    public Pathfinding pathfinding;
    private int targetX, targetY;

    private GameObject player;

    public SpriteDirection spriteDirection;

    public EnemyState currentEnemyState; 

    private void Awake()
    {
        pathfinding = new Pathfinding(GridManager.Instance.width, GridManager.Instance.height);
        player = GameManager.Instance.player;
        currentEnemyState = EnemyState.ATTARGET;
    }

    private void Update()
    {
        switch (currentEnemyState)
        {
            case EnemyState.TOTARGET:
                MoveToTarget();
                break;
            case EnemyState.ATTARGET:
                //Find new Target
                movementHandler.StopMoving();
                movementHandler.SetTargetPosition(GetTargetPosition(),pathfinding);
                ChangeEnemyState(EnemyState.TOTARGET);
                break;
            case EnemyState.DEAD:
                //Dead
                Destroy(this.gameObject);
                break;

        }

        UpdateEnemySprite();
    }

    private void MoveToTarget()
    {
        movementHandler.HandleMovement();
    }

    private Vector3 GetTargetPosition()
    {

        return player.gameObject.transform.position;    
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //first get the pathnodes. 
        if(restrictionLayer == (restrictionLayer | (1 << collision.gameObject.layer)))
        {
            movementHandler.SetTargetPosition(GetTargetPosition(), pathfinding);
            //ChangeEnemyState(EnemyState.ATTARGET);

        }
        if(pathGridLayer==(pathGridLayer | (1 << collision.gameObject.layer)))
        {
            EventManager.SetGameState(EventManager.GameState.GAMEOVER);
            //GAME OVER 
        }

    }

   
    public void ChangeEnemyState(EnemyState newEnemyState)
    {
        currentEnemyState = newEnemyState;
    }
    private void UpdateEnemySprite()
    {
        switch (spriteDirection)
        {
            case SpriteDirection.Left:
                transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                break;
            case SpriteDirection.Right:
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                break;
            default:
                return;
        }
    }
    private bool IsTrapped()
    {
        float raycastDistance = 2.0f; // Adjust as needed based on your grid spacing

        // Check in four directions
        bool up = !RaycastHitInDirection(Vector2.up, raycastDistance);
        bool down = !RaycastHitInDirection(Vector2.down, raycastDistance);
        bool left = !RaycastHitInDirection(Vector2.left, raycastDistance);
        bool right = !RaycastHitInDirection(Vector2.right, raycastDistance);

        // If all directions have hits, the enemy is trapped
        return up && down && left && right;
    }
    private bool RaycastHitInDirection(Vector2 direction, float distance)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance);

        // If a collider is hit, return true (there is an obstacle in that direction)
        return hit.collider != null;
    }
}

public enum EnemyState
{
    TOTARGET,
    ATTARGET,
    DEAD
}