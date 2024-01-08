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


    private void Start()
    {
        
    }

    private void Update()
    {
        HandleMovement();
        if(Input.GetMouseButtonDown(0))
        {
            SetTargetPosition(Utils.GetMouseWorldPosition());   
        }
    }

    private void HandleMovement()
    {
        if(pathVectorList!= null)
        {
            Debug.Log("pathVectorList not null");
            Vector3 targetPosition = pathVectorList[currentPathIndex];
            if(Vector3.Distance(transform.position, targetPosition) >1f) {
                Debug.Log("moving");
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

        }
    }

    public void SetTargetPosition(Vector3 targetPosition)
    {
        currentPathIndex = 0;
        pathVectorList = Pathfinding.Instance.FindPath(GetPosition(), targetPosition);
        Debug.LogWarning(pathVectorList);
        if(pathVectorList != null && pathVectorList.Count > 1)
        {
            pathVectorList.RemoveAt(0);
        }
    }

    private void StopMoving()
    {
        pathVectorList = null;
    }

    public Vector3 GetPosition()
    {
        Debug.Log(transform.position);
        return transform.position;
    }

    
}
