using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathTraversal : MonoBehaviour
{
    public static PathTraversal Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType(typeof(PathTraversal)) as PathTraversal;
            return instance;
        }
        set
        {
            instance = value;
        }
    }
    private static PathTraversal instance;

    public List<Vector3Int> pathVectors;
    //private Dictionary<Vector3Int, (Vector3Int, Vector3Int)> leftAndRightFromCurrentHeading;
    private Dictionary<Vector3Int, (Vector3Int, Vector3Int)> leftAndRightFromCurrentHeading = new Dictionary<Vector3Int, (Vector3Int, Vector3Int)>
        {
            // heading key, (left, right)
            { Vector3Int.up, (Vector3Int.left, Vector3Int.right) },
            { Vector3Int.down, (Vector3Int.right, Vector3Int.left) },
            { Vector3Int.left, (Vector3Int.down, Vector3Int.up) },
            { Vector3Int.right, (Vector3Int.up, Vector3Int.down) },
        };
    private List<Coordinates> leftFloodFilledCoordinates; 
    private List<Coordinates> rightFloodFilledCoordinates;

    public Coordinates floodFillCoordinates; 

    private void Start()
    {
        pathVectors = new List<Vector3Int>();
        
        leftFloodFilledCoordinates = new List<Coordinates>();   
        rightFloodFilledCoordinates= new List<Coordinates>();   
    }

    public void FindVectorInts()
    {
        foreach(Coordinates coord in GridManager.Instance.pathCoordinates)
        {
            pathVectors.Add(Vector3Int.RoundToInt(GridManager.Instance.grid.GetWorldPosition(coord.X, coord.Y)));
        }


        for(int i=0; i<GridManager.Instance.pathCoordinates.Count-1; i++)
        {
            Vector3Int heading = (pathVectors[i + 1] - pathVectors[i])/2;
            /*if(heading.x>0)
            {
                heading = Vector3Int.right;
            }
            if (heading.x < 0)
            {
                heading = Vector3Int.left;
            }
            if (heading.y > 0)
            {
                heading = Vector3Int.up;
            }
            if(heading.y < 0)
            {
                heading = Vector3Int.down;
            }
            Debug.Log(heading);*/
            if (leftAndRightFromCurrentHeading.ContainsKey(heading))
            {
                Debug.LogWarning("Contains Heading");
                (Vector3Int left, Vector3Int right) = leftAndRightFromCurrentHeading[heading];

                //Convert left and right to Coordinates.
                Debug.Log("Decided Left and right: "+ left + ", "+ right);
                Vector3 leftPosition = GridManager.Instance.grid.GetWorldPosition(GridManager.Instance.pathCoordinates[i].X, GridManager.Instance.pathCoordinates[i].Y)+ new Vector3(left.x, left.y, left.z)*GridManager.Instance.gridCellSize;
                Vector3 rightPosition = GridManager.Instance.grid.GetWorldPosition(GridManager.Instance.pathCoordinates[i].X, GridManager.Instance.pathCoordinates[i].Y) + new Vector3(right.x, right.y, right.z) * GridManager.Instance.gridCellSize;

                GridManager.Instance.grid.GetXY(leftPosition, out int leftX, out int leftY);
                Debug.LogWarning(leftX + "," + leftY);
                
                GridManager.Instance.grid.GetXY(rightPosition, out int rightX, out int rightY);
                Debug.LogWarning(rightX + "," + rightY);
                if (GridManager.Instance.grid.GetGridObject(leftX, leftY).GetType() == GridType.Grid && GridManager.Instance.grid.GetGridObject(rightX, rightY).GetType() == GridType.Grid)
                {
                    leftFloodFilledCoordinates.Clear();
                    rightFloodFilledCoordinates.Clear();
                    //Proceed to flood fill both sides and count it. 
                    Debug.Log("Checking Left and Right");
                    leftFloodFilledCoordinates = FloodFill.Instance.Flood(leftX, leftY, leftFloodFilledCoordinates);
                    rightFloodFilledCoordinates = FloodFill.Instance.Flood(rightX, rightY, rightFloodFilledCoordinates);

                    if (leftFloodFilledCoordinates.Count < rightFloodFilledCoordinates.Count)
                    {
                        floodFillCoordinates = new Coordinates(leftX, leftY);
                        Debug.LogWarning(leftX +","+ leftY);
                        ///ConvertToGrid(rightFloodFilledCoordinates);
                    }
                    else
                    {
                        floodFillCoordinates = new Coordinates(rightX, rightY);
                        Debug.LogWarning(rightX + "," + rightY);
                        //ConvertToGrid(leftFloodFilledCoordinates);
                    }
                    break;
                }
            }
           
        }
    }
    void ConvertToGrid(List<Coordinates> coords)
    {
        foreach(Coordinates coord in coords) {
            GridManager.Instance.SetAsGrid(GridManager.Instance.grid.gridArray[coord.X, coord.Y]);
        }
    }
    public void Flood()
    {
        if (floodFillCoordinates != null)
        {
            StartCoroutine(FloodFill.Instance.Flood(floodFillCoordinates.X, floodFillCoordinates.Y));
        }
        else
        {
            Debug.LogWarning("NULL");
        }
        
    }

}
