

public interface IAiState 
{
    AiStateId GetId();
    void EnterState(Enemy enemyAgent);

    void UpdateState(Enemy enemyAgent);
    void ExitState(Enemy enemyAgent);

}

public enum AiStateId
{
    IDLE,
    CHASE,
    TARGET,
    DEATH
}




