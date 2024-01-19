using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EmptyCharacter.Utils;
using Unity.VisualScripting;

public class EnemyPathfindingMovementHandler : MonoBehaviour
{
    [SerializeField]
    private Enemy enemy; 
    [SerializeField] private float speed;
    private int currentPathIndex;
    private List<Vector3> pathVectorList;
    private List<PathNode> pathNodes;

    public bool hasCheckedForEnemy = false;

    public void HandleMovement()
    {
        if(pathVectorList!= null)
        {
            //Debug.Log("pathVectorList not null");
            Vector3 targetPosition = pathVectorList[currentPathIndex];
            //Debug.Log("target : "+ targetPosition);
            if(Vector3.Distance(transform.position, targetPosition) >1f) {
                //Debug.Log("moving");
                Vector3 moveDir = (targetPosition -  transform.position).normalized ;

                float distanceBefore = Vector3.Distance(transform.position, targetPosition);
                //Debug.LogWarning("Local Scale : " + transform.localScale + "Move Dir : "+ moveDir);
                transform.position = transform.position  + moveDir*speed*Time.deltaTime;
            }
            else
            {
                currentPathIndex++;
                if(currentPathIndex >= pathVectorList.Count) {
                    //StopMoving();
                    GetComponent<Enemy>().ChangeEnemyState(EnemyState.ATTARGET);
                }
            }
        }
        else
        {
            Debug.LogError("Path vector list is null");
            GridManager.Instance.grid.GetXY(this.gameObject.transform.position, out int x, out int y);
            if (GridManager.Instance.grid.GetGridObject(x,y).GetType()==GridType.BlueGrid)
            {
                Debug.LogError("ENEMY IS IN FLOOD");
                GameManager.Instance.enemies.Remove(this.gameObject);
                GameManager.Instance.numberOfEnemiesAlive--;
                GetComponent<Enemy>().ChangeEnemyState(EnemyState.DEAD);
            }
            else
            {
                enemy.ChangeEnemyState(EnemyState.ATTARGET);
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

    
}
