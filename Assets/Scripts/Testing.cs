using EmptyCharacter.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField]
    public Sprite sprite;
    private Pathfinding pathfinding;
    [SerializeField]
    public GameObject originObject;
    private int targetX , targetY;
    [SerializeField]
    private GameObject enemyOne;

    [SerializeField] private EnemyMovement characterPathfinding;
    private void Start()
    {
        pathfinding = new Pathfinding(25, 25);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPosition = Utils.GetMouseWorldPosition();
            pathfinding.GetGrid().GetXY(mouseWorldPosition, out int x, out int y);
            targetX = x;
            targetY = y;
            pathfinding.GetGrid().GetXY(enemyOne.transform.position, out int enemyX, out int enemyY);

            List<PathNode> path = pathfinding.FindPath(0, 0, targetX, 0);
            // pathfinding.GetGrid().InstantiateBlueSprite(0, 0,sprite);
            if (path != null)
            {
                for (int i = 0; i < path.Count - 1; i++)
                {
                    Debug.DrawLine(new Vector2(path[i].x, path[i].y) * 10f + Vector2.one * 5f, new Vector2(path[i + 1].x, path[i + 1].y) * 5f + Vector2.one * 5f, Color.green, 3f);
                    pathfinding.GetGrid().InstantiateBlueSprite(path[i].x, path[i].y, sprite);
                }
            }
            //characterPathfinding.SetTargetPosition(mouseWorldPosition);
        }
    }
}
