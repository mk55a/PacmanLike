using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseEnemyState : IAiState
{
    private float chaseDuration;
    private float chaseTimer;

    public AiStateId GetId()
    {
        return AiStateId.CHASE;
    }
    public void EnterState(Enemy enemy)
    {
        enemy.circleCollider2D.radius = 0;
        chaseDuration = enemy.enemyConfig.chaseDuration;
        chaseTimer = 0f; 
    }

    public void UpdateState(Enemy enemy)
    {
        
        enemy.movementHandler.ChaseTarget(enemy.player.gameObject.transform, enemy.pathfinding);
        //enemy.movementHandler.HandleMovement();

        chaseTimer += Time.deltaTime;
        if(chaseTimer >= chaseDuration)
        {
            enemy.stateMachine.ChangeState(AiStateId.IDLE);
        }
    }

    public void ExitState(Enemy enemy)
    {
        Debug.LogError("EXITING CHASE");
    }
}
