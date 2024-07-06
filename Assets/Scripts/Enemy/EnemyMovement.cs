using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EmptyCharacter.Utils;
using Unity.VisualScripting;

public class EnemyMovement : MonoBehaviour
{
    
    private Enemy enemy; 

    [SerializeField] private float speed; // Later get it from the level manager.

    private int currentPathIndex;

    private List<Vector3> pathVectorList;

    private List<PathNode> pathNodes;

    public bool hasCheckedForEnemy = false;

    private Vector3 moveDir;
    private LineRenderer lineRenderer;
    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        lineRenderer = GetComponent<LineRenderer>();    
    }

    private void UpdateLineRenderer(Vector3 startPoint, Vector3 endPoint)
    {
        // Set LineRenderer positions
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, startPoint);
        lineRenderer.SetPosition(1, endPoint);
    }
    private void UpdateLineRenderer()
    {
        lineRenderer.positionCount = pathVectorList.Count;
        for (int i = 0; i < pathVectorList.Count; i++)
        {
            lineRenderer.SetPosition(i, pathVectorList[i]);
        }
    }

    public void HandleMovement()
    {
        if(pathVectorList!= null)
        {
            Debug.Log("pathVectorList not null");
            Vector3 targetPosition = pathVectorList[currentPathIndex];
            Debug.Log("target : "+ targetPosition + " current:"+ transform.position);
            if(Vector3.Distance(transform.position, targetPosition) >1f) {
                //Debug.Log("moving");
                moveDir = (targetPosition -  transform.position).normalized ;
                /*UpdateLineRenderer(transform.position, transform.position + moveDir * 5f);*/
                UpdateLineRenderer();
                if (moveDir.x > 0)
                {
                    ///Right
                    enemy.spriteDirection = SpriteDirection.Right;
                }
                else
                {
                    enemy.spriteDirection=SpriteDirection.Left;
                }
                float distanceBefore = Vector3.Distance(transform.position, targetPosition);
                //Debug.LogWarning("Local Scale : " + transform.localScale + "Move Dir : "+ moveDir);
                transform.position = transform.position  + moveDir*speed*Time.deltaTime;

                
            }
            else
            {
                currentPathIndex++;
                if(currentPathIndex >= pathVectorList.Count) {
                    pathVectorList = null;
                    lineRenderer.positionCount = 0;
                    Debug.Log("Enemy Stopped moving");
                    //StopMoving();
                    /*GetComponent<Enemy>().ChangeEnemyState(EnemyState.ATTARGET);*/
                }
            }
        }
        else
        {
            Debug.LogError("Path vector list is null");
            GridManager.Instance.grid.GetXY(this.gameObject.transform.position, out int x, out int y);
            if (GridManager.Instance.grid.GetGridObject(x,y).GetType()==GridType.BlueGrid)
            {
                GameManager.Instance.enemies.Remove(this.gameObject);
                enemy.stateMachine.ChangeState(AiStateId.DEATH);
                Debug.LogError("ENEMY IS IN FLOOD");
                
                GameManager.Instance.numberOfEnemiesAlive--;
                
                /*GetComponent<Enemy>().ChangeEnemyState(EnemyState.DEAD);*/
            }
            else
            {
                enemy.stateMachine.ChangeState(AiStateId.IDLE);
                /*enemy.ChangeEnemyState(EnemyState.ATTARGET);*/
            }


        }
    }
    private void Update()
    {
        HandleMovement();
    }

    public void HandleIdleMovement(Pathfinding pathfinding)
    {
        //Make the enemy traverse through random empty grids. 
        Vector3 randomPos = GetRandomPositionOnGrid();

        SetTargetPosition(randomPos, pathfinding);
        //HandleMovement();

    }

    private Vector3 GetRandomPositionOnGrid()
    {
        int gridWidth = GridManager.Instance.grid.GetWidth();
        int gridHeight = GridManager.Instance.grid.GetHeight();

        while (true)
        {
            int randomX = Random.Range(0, gridWidth);
            int randomY = Random.Range(0, gridHeight);

            PathNode randomNode = Pathfinding.Instance.GetNode(randomX, randomY);

            if(randomNode != null  && randomNode.isWalkable)
            {
                return Pathfinding.Instance.GetGrid().GetWorldPosition(randomX, randomY);
            }
        }
    }

    public void SetTargetPosition(Vector3 targetPosition, Pathfinding pathfinding)
    {
        //Debug.LogWarning("Setting target Post : " + targetPosition);
        currentPathIndex = 0;
        pathVectorList = pathfinding.FindPath(GetPosition(), targetPosition);
       // Debug.LogWarning(pathVectorList.Count);
        
        if(pathVectorList != null && pathVectorList.Count > 1)
        {
            pathVectorList.RemoveAt(0);
        }
        UpdateLineRenderer();
    }
   
    public void CheckPathNodes(Vector3 targetPosition, Pathfinding pathfinding)
    {
        pathfinding.GetGrid().GetXY(targetPosition, out int endX, out int endY);
        pathfinding.GetGrid().GetXY(GetPosition(), out int startX, out int startY);

        pathNodes = pathfinding.FindPath(startX, startY, endX, endY);
        for(int i=0; i<pathNodes.Count-1; i++)
        {
            if(GridManager.Instance.grid.gridArray[pathNodes[i].x, pathNodes[i].y].GetType() != GridType.PathGrid)
            {
                //Enemy is either running into a boundary, path grid or a blue grid.
                Debug.LogWarning("Will connected with players path Grid");
            }
        }
    }
    public void StopMoving()
    {
        pathVectorList = null;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void ChaseTarget(Transform player, Pathfinding pathfinding)
    {
        Debug.Log("Setting up a chase target");
        currentPathIndex = 0;
        pathVectorList = pathfinding.FindPath(GetPosition(), player.position);

        if (pathVectorList != null && pathVectorList.Count > 1)
        {
            pathVectorList.RemoveAt(0);
        }
    }
}


