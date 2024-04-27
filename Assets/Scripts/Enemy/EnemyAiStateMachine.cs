using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAiStateMachine
{
    public IAiState[] states;
    public Enemy enemyAgent;
    public AiStateId currentState;

    public EnemyAiStateMachine(Enemy enemyAgent)
    {
        this.enemyAgent = enemyAgent;
        int numStates = System.Enum.GetNames(typeof(AiStateId)).Length;
        states = new IAiState[numStates];
    }

    public void RegisterState(IAiState state)
    {
        int index = (int)state.GetId();
        states[index] = state;
    }

    public IAiState GetState(AiStateId stateId)
    {
        
        int index = (int)stateId;
        return states[index];
    }

    public void Update()
    {
        GetState(currentState)?.UpdateState(enemyAgent);
    }

    public void ChangeState(AiStateId newState)
    {
        GetState(currentState)?.ExitState(enemyAgent);
        currentState = newState;
        GetState(currentState)?.EnterState(enemyAgent);
    }
}
