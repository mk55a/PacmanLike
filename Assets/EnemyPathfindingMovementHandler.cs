using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EmptyCharacter.Utils;
using Unity.VisualScripting;

public class EnemyPathfindingMovementHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject enemy; 
    [SerializeField] private float speed;
    private int currentPathIndex;
    private List<Vector3> pathVectorList;
    private List<PathNode> pathNodes;

    private void Start()
    {
        
    }

    private void Update()
    {
        //HandleMovement();
        /*if(Input.GetMouseButtonDown(0))
        {
            SetTargetPosition(Utils.GetMouseWorldPosition());   
        }*/
    }

    public void HandleMovement()
    {
        if(pathVectorList!= null)
        {
            //Debug.Log("pathVectorList not null");
            Vector3 targetPosition = pathVectorList[currentPathIndex];
            //Debug.Log("target : "+ targetPosition);
            if(Vector3.Distance(transform.position, targetPosition) >1f) {
                //Debug.Log("moving");
                Vector3 moveDir = (targetPosition -  transform.position).normalized;

                float distanceBefore = Vector3.Distance(transform.position, targetPosition);
                transform.position = transform.position + moveDir*speed*Time.deltaTime;
            }
            else
            {
                currentPathIndex++;
                if(currentPathIndex >= pathVectorList.Count) {
                    StopMoving();

                }
            }
        }
        else
        {
            Debug.LogError("Path vector list is null");
        }
    }

    public void SetTargetPosition(Vector3 targetPosition, Pathfinding pathfinding)
    {
        Debug.LogWarning("Setting target Post : " + targetPosition);
        currentPathIndex = 0;
        pathVectorList = pathfinding.FindPath(GetPosition(), targetPosition);
       // Debug.LogWarning(pathVectorList.Count);
        foreach(Vector3 pathVector in pathVectorList)
        {
            //Debug.LogWarning(pathVector);
        }
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
        Debug.LogWarning("Enemypos:"+ transform.position);
        return transform.position;
    }

    
}
