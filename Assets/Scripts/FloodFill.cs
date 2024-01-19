using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloodFill : MonoBehaviour
{
    public static FloodFill Instance { get; private set; }  
    [SerializeField] private float fillDelay = 0.2f;
    private GridManager gridManager;
    public bool isFloodFilling;
    public List<Coordinates> lastFloodedCoordinates;
    private void Awake()
    {
        if(Instance == null)
            Instance = this;

        gridManager = GetComponent<GridManager>();  
        lastFloodedCoordinates = new List<Coordinates>();   
    }

   
    public IEnumerator Flood(int startX, int startY)
    {
        WaitForSeconds wait = new WaitForSeconds(fillDelay);
        Stack<Vector2Int> stack = new Stack<Vector2Int>();
        stack.Push(new Vector2Int(startX, startY));
        Debug.LogWarning(startX+",,"+ startY);
        while (stack.Count > 0)
        {
            isFloodFilling = true;
            Vector2Int current = stack.Pop();
            int x = current.x;
            int y = current.y;

            if (x >= 0 && x < gridManager.width && y >= 0 && y < gridManager.height && (gridManager.grid.gridArray[x, y].GetType() == GridType.Grid))
            {
                if((gridManager.grid.gridArray[x, y].GetType() == GridType.Grid) || (gridManager.grid.gridArray[x, y].GetType() == GridType.BlueGrid))
                {
                    gridManager.SetGridAsBlue(gridManager.grid.gridArray[x, y], x, y);
                    if(GameManager.Instance.numberOfEnemiesAlive!= 0)
                    {
                        Debug.Log("Number of enemies Alive : " + GameManager.Instance.numberOfEnemiesAlive + "," + GameManager.Instance.numberOfEnemies);
                        for (int i = 0; i < GameManager.Instance.numberOfEnemiesAlive; i++)
                        {
                            GameManager.Instance.enemies[i].GetComponent<Enemy>().pathfinding._grid.GetGridObject(x, y).isWalkable = false;
                        }
                    }
                    
                    stack.Push(new Vector2Int(x + 1, y));
                    stack.Push(new Vector2Int(x - 1, y));
                    stack.Push(new Vector2Int(x, y + 1));
                    stack.Push(new Vector2Int(x, y - 1));
                }
            }
            yield return wait;
        }
        isFloodFilling = false; 
    }

    public List<Coordinates> Flood(int startX, int startY, List<Coordinates> filledVectors)
    {
        //WaitForSeconds wait = new WaitForSeconds(fillDelay);
        Stack<Vector2Int> stack = new Stack<Vector2Int>();
        Stack<Vector2Int> stackFull = new Stack<Vector2Int>();  
        stack.Push(new Vector2Int(startX, startY));
        //stackFull.Push(new Vector2Int(startX, startY));
        Debug.LogWarning("Flooding "+startX + ",," + startY);
        while (stack.Count > 0)
        {
            Vector2Int current = stack.Pop();
            int x = current.x;
            int y = current.y;

            if (x >= 0 && x < gridManager.width && y >= 0 && y < gridManager.height )
            {
                if ((gridManager.grid.gridArray[x, y].GetType() == GridType.Grid) && !stackFull.Contains(new Vector2Int(x,y)))
                {
                    stackFull.Push(new Vector2Int(x, y));
                    //gridManager.SetGridAsExample(gridManager.grid.gridArray[x, y],x,y);
                    //Debug.Log("Is a Grid :" + x + "," + y);
                    filledVectors.Add(new Coordinates(x, y));
                    stack.Push(new Vector2Int(x + 1, y));
                    stack.Push(new Vector2Int(x - 1, y));
                    stack.Push(new Vector2Int(x, y + 1));
                    stack.Push(new Vector2Int(x, y - 1));
                }
                else
                {
                    //Debug.LogError("Not a grid");
                }
            }
            else
            {
                Debug.LogError("Not in the grid only");
            }
            
        }
        lastFloodedCoordinates = filledVectors;
        return filledVectors;
    }
    public bool CheckForEnemy(Coordinates enemyCoordinates)
    {
        Debug.LogWarning("enemy " + enemyCoordinates.X + " ," + enemyCoordinates.Y);
        foreach(Coordinates coord in lastFloodedCoordinates)
        {
            Debug.LogWarning("flood " + coord.X + " ," + coord.Y);
            if(coord == enemyCoordinates)
            {
                return true;
            }
        }
        return false; 
        /*if (lastFloodedCoordinates.Contains(enemyCoordinates))
        {
            Debug.LogWarning("Enemy is at location");
            return true;
        }
        else { return false; }*/

    }
    public void InitiateFlood(int x, int y)
    {
        StartCoroutine(Flood(x, y));
    }
    
}
