using EmptyCharacter.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField] private EnemyPathfindingMovementHandler movementHandler;
    [SerializeField] private LayerMask restrictionLayer;
    [SerializeField] private LayerMask pathGridLayer; 
    private Pathfinding pathfinding;
    private int targetX, targetY;

    private SpriteDirection spriteDirection;

    bool enemyEnabled = false;

    private void Awake()
    {
        pathfinding = new Pathfinding(GridManager.Instance.width, GridManager.Instance.height);
        //pathfinding.GetGrid().showDebug = true;
        //MoveToTarget();
        movementHandler.SetTargetPosition(GetTargetPosition(), pathfinding);

    }
    public void EnableEnemy(bool enable)
    {
        if (enable)
        {
            enemyEnabled = true;
            movementHandler.SetTargetPosition(GetTargetPosition(), pathfinding);
        }
        else
        {
            movementHandler.StopMoving();
        }
    }

    private void Update()
    {
        /*if (Input.GetMouseButtonDown(0))
        {
            movementHandler.SetTargetPosition(GetTargetPosition(),pathfinding);
        }*/

        MoveToTarget();
    }

    private void MoveToTarget()
    {
        movementHandler.HandleMovement();
    }

    private Vector3 GetTargetPosition()
    {
        //Debug.LogWarning(GameManager.Instance.player.transform.position);
        return player.gameObject.transform.position;    
        //return GameManager.Instance.player.transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.LogWarning("COLLISION");
        //first get the pathnodes. 
        if(restrictionLayer == (restrictionLayer | (1 << collision.gameObject.layer)))
        {
            Debug.Log("BLUE GRID");
            movementHandler.StopMoving();
            movementHandler.SetTargetPosition(GetTargetPosition(), pathfinding);
        }
        if(pathGridLayer==(pathGridLayer | (1 << collision.gameObject.layer)))
        {
            Debug.LogWarning("Enemy Collided with Path grid");
            EventManager.SetGameState(EventManager.GameState.GAMEOVER);
            //GAME OVER 
        }
        
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
}

