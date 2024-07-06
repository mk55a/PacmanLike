using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleEnemyState : IAiState
{

    public AiStateId GetId()
    {
        return AiStateId.IDLE;
    }
    public void EnterState(Enemy enemy)
    {
        Debug.Log(GetId().ToString());
        enemy.circleCollider2D.radius = enemy.enemyConfig.colliderRadius;
        enemy.movementHandler.HandleIdleMovement(enemy.pathfinding);

    }

    public void UpdateState(Enemy enemy)
    {
        
        //Debug.Log("Updating Idle State");
    }


    public void ExitState(Enemy enemy)
    {
        
    }
}

