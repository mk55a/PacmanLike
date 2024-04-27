using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapturedEnemyState : IAiState
{
    public AiStateId GetId()
    {
        return AiStateId.CHASE;
    }
    public void EnterState(Enemy enemy)
    {

    }

    public void UpdateState(Enemy enemy)
    {

    }

    public void ExitState(Enemy enemy)
    {

    }
}
