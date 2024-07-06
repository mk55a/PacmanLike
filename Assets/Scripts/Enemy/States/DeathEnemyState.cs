using UnityEngine;
public class DeathEnemyState : IAiState
{
    public AiStateId GetId()
    {
        return AiStateId.DEATH;
    }
    public void EnterState(Enemy enemy)
    {
        Debug.Log(GetId().ToString());
        enemy.OnDeath();
    }

    public void UpdateState(Enemy enemy)
    {

    }

    public void ExitState(Enemy enemy)
    {

    }
}
