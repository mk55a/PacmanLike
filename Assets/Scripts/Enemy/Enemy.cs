using EmptyCharacter.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public EnemyMovement movementHandler;
    public Pathfinding pathfinding;
    public GameObject player;

    [SerializeField] public EnemyConfigScriptable enemyConfig;

    public EnemyAiStateMachine stateMachine;
    public AiStateId initialState;
    public AiStateId currentState;

    [SerializeField] private LayerMask restrictionLayer;
    [SerializeField] private LayerMask pathGridLayer;
    [SerializeField] public CircleCollider2D circleCollider2D;

    private int targetX, targetY;

    public SpriteDirection spriteDirection;

    private void Awake()
    {
        movementHandler = GetComponent<EnemyMovement>();
        pathfinding = new Pathfinding(GridManager.Instance.width, GridManager.Instance.height);
        player = GameManager.Instance.player;

    }
    private void Start()
    {
        stateMachine = new EnemyAiStateMachine(this);
        stateMachine.RegisterState(new IdleEnemyState());
        stateMachine.RegisterState(new ChaseEnemyState());
        stateMachine.ChangeState(initialState);
    }

    private void Update()
    {
        currentState = stateMachine.currentState;
        
        stateMachine.Update();
    }








    //public EnemyState currentEnemyState;
    //public AiStateId currentEnemyState;

    /*private IAiState currentState;

    public IdleEnemyState idleState;
    public TargetState targetState;
    public ChaseEnemyState chaseState;
    public DeathEnemyState deathState; 
    public CapturedEnemyState capturedState;

    private void Awake()
    {
        movementHandler = GetComponent<EnemyMovement>();

        pathfinding = new Pathfinding(GridManager.Instance.width, GridManager.Instance.height);

        player = GameManager.Instance.player;

        //currentEnemyState = EnemyState.ATTARGET;
        currentEnemyState = AiStateId.IDLE;

        idleState = new IdleEnemyState();
        targetState = new TargetState();
        chaseState = new ChaseEnemyState();
        deathState = new DeathEnemyState();
        capturedState = new CapturedEnemyState();

        currentState = idleState;
        currentState.EnterState();
    }

    private void Update()
    {
        UpdateEnemyState();

        UpdateEnemySprite();
    }
    */

    

    void UpdateEnemyState()
    {
        /*switch (currentEnemyState)
        {

            case AiStateId.IDLE: //The state where enemies are just bouncing around
                //Do RandomMovment
                break;

            case AiStateId.TARGET: //The state where the enemies will go to the target, doesn't depend on on the distance as long as it is in bounds. 
                
                movementHandler.StopMoving();
                movementHandler.SetTargetPosition(GetTargetPosition(), pathfinding); //Since this is in update loop, it is basically chasing the player as update loop. 

                MoveToTarget();

                break;

            case AiStateId.CHASE: //The state they switch to from IDLE when they they detect the enemy in range through a circular collider.
                
                break;

            case AiStateId.DEATH:
                //Dead
                Destroy(this.gameObject);

                break;

        }*/


       
    }

    private void MoveToTarget()
    {
        movementHandler.HandleMovement();
    }

    public Vector3 GetTargetPosition()
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player in Range");
            stateMachine.ChangeState(AiStateId.CHASE);
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