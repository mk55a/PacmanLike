using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using EmptyCharacter.Utils;
using System.Drawing;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;

public class GridManager : MonoBehaviour
{
    //public static GridManager Instance { get; private set; }
    [SerializeField] public int width, height;
    [SerializeField] public float gridCellSize;
    [SerializeField] public Sprite sprite;
    [SerializeField] public GameObject originObject;
    public Grid<GridMapObject> grid;


    public List<Coordinates> boundaryCoordinates;
    public List<Coordinates> blueCoordinates;
    public List<Coordinates> pathCoordinates;
    public List<Coordinates> allCoordinates;
    public List<Coordinates> capturedCoordinates;
    public Coordinates boundaryStartCoordinates, boundaryEndCoordinates;

    
    [SerializeField] private float fillDelay;
    [SerializeField] private bool debugGrid;
    public static GridManager Instance
    {
        get
        {
            if(instance == null)
                instance = FindObjectOfType(typeof(GridManager)) as GridManager;
            return instance;
        }
        set
        {
            instance = value;
        }
    }
    private static GridManager instance;
    private void Awake()
    {
        
        pathCoordinates = new List<Coordinates>();
        blueCoordinates = new List<Coordinates>();
        boundaryCoordinates = new List<Coordinates>();
        allCoordinates = new List<Coordinates>();
        capturedCoordinates = new List<Coordinates>();  
    }
    private void Start()
    {
        grid = new Grid<GridMapObject>(width, height, 2f, originObject.transform.position, (Grid<GridMapObject> g, int x, int y)=>new GridMapObject(g,x,y), debugGrid);

        DestroyGridForBoundaries();
        SetBoundaries();
        allCoordinates = grid.AllGrids();
    }
    private void Update()
    {
        /*if(Input.GetMouseButtonDown(0))
        {
            Vector3 position = Utils.GetMouseWorldPosition();
            GridMapObject gridObject = grid.GetGridObject(position);
            if(gridObject != null)
            {
                //gridObject.SetType(GridType.PathGrid);
                SetGridAsPath(gridObject,position);
                int x,y;
                grid.GetXY(position, out x, out y);
                grid.InstantiatePathSprite(x, y, sprite);
                
            }
        }*/
    }
    public void SetGridAsPath(GridMapObject gridMapObject, Vector2 position)
    {
        gridMapObject.SetType(GridType.PathGrid);
        int x, y;
        grid.GetXY(position, out x, out y);
        pathCoordinates.Add(new Coordinates(x, y));
        //Make the below line a coroutine so I can animate it.

        StartCoroutine(InstantiatePathSprite(x, y));
        
    }

    IEnumerator InstantiatePathSprite(int x, int y)
    {
        WaitForSeconds wait = new WaitForSeconds(fillDelay);
        yield return wait;
        grid.InstantiatePathSprite(x, y, sprite);

    }
    public void SetGridAsBlue(GridMapObject gridMapObject, int x, int y)
    {
        gridMapObject.SetType(GridType.BlueGrid);
        grid.InstantiateBlueSprite(x, y, sprite);
    }
    private void DestroyGridForBoundaries()
    {
        for (int x = 0; x < width; x++)
        {
            int y = 0;
            Destroy(grid.allGridArray[x,y]);
        }
        for (int y = 1; y < height; y++)
        {
            int x = 0;
            Destroy(grid.allGridArray[x, y]);
        }
        for (int x = 0; x < width; x++)
        {
            int y = height - 1;
            Destroy(grid.allGridArray[x, y]);
        }
        for (int y = 1; y < height; y++)
        {
            int x = width - 1;
            Destroy(grid.allGridArray[x, y]);

        }
    }
    public void SetGridAsExample(GridMapObject gridMapObject, int x, int y)
    {
        gridMapObject.SetType(GridType.Example);

    }
    public void SetAsGrid(GridMapObject gridMapObject)
    {
        gridMapObject.SetType(GridType.Grid);
    }

    public void Connect()
    {

        PathTraversal.Instance.FindVectorInts();


        PathTraversal.Instance.Flood();
        PathToBlue();
        /*GetBoundaries();
        Vector2 pointIn = PointInPolygon.Instance.SetPolygonVertices();
        grid.GetXY(pointIn, out int startPointX, out int startPointY);*/





        //Haveto find one grid/Coordinate which lies in the enclosed area of the path.
        /*int minX = int.MaxValue, minY = int.MaxValue, maxX = int.MinValue, maxY = int.MinValue;
        foreach (Coordinates coord in pathCoordinates)
        {
            //Debug.Log(coord.X + ";" + coord.Y);
            minX = Mathf.Min(minX, coord.X);
            minY = Mathf.Min(minY, coord.Y);
            maxX = Mathf.Max(maxX, coord.X);
            maxY = Mathf.Max(maxY, coord.Y);
        }
        int startPointX = (minX + maxX) / 2;
        int startPointY = (minY + maxY) / 2;*/

        //FloodFill.Instance.InitiateFlood(startPointX, startPointY);
        //PathToBlue();
        /*if (IsWithinArea(startPointX, startPointY))
        {
            FloodFill.Instance.InitiateFlood(startPointX, startPointY);
            PathToBlue();
        }
        else
        {
            Debug.LogError("Starting point is outside the desired area : "+startPointX+";"+startPointY);
        }*/

    }
    private void GetBoundaries()
    {
        Coordinates start = pathCoordinates[0];
        Coordinates end = pathCoordinates[pathCoordinates.Count - 1];

        GetStartBoundaryCoordinates(start);
        GetEndBoundaryCoordinates(end);
        Debug.LogWarning("Boundarystarrt : " + boundaryStartCoordinates.X + ","+ boundaryStartCoordinates.Y);
        Debug.LogWarning("Boundaryend : " + boundaryEndCoordinates.X + "," + boundaryEndCoordinates.Y);
    }
    private void GetEndBoundaryCoordinates(Coordinates endBoundary)
    {
        Coordinates end = endBoundary;
        if (end.X == 1 && end.Y != 1)
        {
            //Boundary is the left side. 
            boundaryEndCoordinates = new Coordinates(0, end.Y);
        }
        if (end.X == 1 && end.Y == 1)
        {
            //check the next coordinate to find the path
            GetEndBoundaryCoordinates(pathCoordinates[pathCoordinates.Count - 2]);
        }
        if (end.X == grid.GetWidth() - 1 && end.Y != 1)
        {
            //Boundary is on right side
            boundaryEndCoordinates = new Coordinates(grid.GetWidth() - 1, end.Y);
        }
        if (end.X == grid.GetWidth() - 1 && end.Y == 1)
        {
            //Check the next coordinate to find the path 
            GetEndBoundaryCoordinates(pathCoordinates[pathCoordinates.Count - 2]);
        }

        if (end.Y == 1 && end.X != 1)
        {
            boundaryEndCoordinates = new Coordinates(end.X, 0);
            //Boundary is the bottom side. 
        }
        if (end.Y == grid.GetHeight() - 1 && end.X != 1)
        {
            //Boundary is on Up side
            boundaryEndCoordinates = new Coordinates(end.X, grid.GetHeight() - 1);
        }
        if (end.Y == grid.GetHeight() - 1 && end.X == 1)
        {
            //Check the next coordinate to find the path 
            GetEndBoundaryCoordinates(pathCoordinates[pathCoordinates.Count - 2]);
        }
        if (end.Y == grid.GetHeight() - 1 && end.X == grid.GetWidth() - 1)
        {
            //check the next coordinate
            GetEndBoundaryCoordinates(pathCoordinates[pathCoordinates.Count-2]);
        }

    } 
    private void GetStartBoundaryCoordinates(Coordinates startBoundary)
    {
        Coordinates start = startBoundary;
        if (start.X == 1 && start.Y != 1)
        {
            //Boundary is the left side. 
            boundaryStartCoordinates = new Coordinates(0, start.Y);
        }
        if (start.X == 1 && start.Y == 1)
        {
            //check the next coordinate to find the path
            GetStartBoundaryCoordinates(pathCoordinates[1]);
        }
        if (start.X == grid.GetWidth() - 1 && start.Y != 1)
        {
            //Boundary is on right side
            boundaryStartCoordinates = new Coordinates(grid.GetWidth()-1, start.Y);
        }
        if (start.X == grid.GetWidth() - 1 && start.Y == 1)
        {
            //Check the next coordinate to find the path 
            GetStartBoundaryCoordinates(pathCoordinates[1]);
        }

        if (start.Y == 1 && start.X != 1)
        {
            boundaryStartCoordinates = new Coordinates(start.X, 0);
            //Boundary is the bottom side. 
        }
        if (start.Y == grid.GetHeight() - 1 && start.X != 1)
        {
            //Boundary is on Up side
            boundaryStartCoordinates = new Coordinates(start.X, grid.GetHeight()-1);
        }
        if (start.Y == grid.GetHeight() - 1 && start.X == 1)
        {
            //Check the next coordinate to find the path 
            GetStartBoundaryCoordinates(pathCoordinates[1]);
        }
        if (start.Y == grid.GetHeight() - 1 && start.X == grid.GetWidth() - 1)
        {
            //check the next coordinate
            GetStartBoundaryCoordinates(pathCoordinates[1]);
        }
    }
    private bool IsWithinArea(int x, int y)
    {
        foreach (Coordinates coord in pathCoordinates)
        {
            // Assuming Coordinates is a class or struct with X and Y properties
            if (coord.X == x && coord.Y == y)
            {
                // The point is within the area
                return true;
            }
        }
        return false;
    }
    private IEnumerator CheckMidGrid(int selectedGridX, int selectedGridY)
    {
        GridType selectedGridType= grid.GetGridObject(selectedGridX, selectedGridY).GetType();
       
        switch(selectedGridType)
        {
            case GridType.Grid:
                FloodFill.Instance.InitiateFlood(selectedGridX, selectedGridY); FloodFill.Instance.InitiateFlood(selectedGridX, selectedGridY);
                yield return null;
                break;
            case GridType.BlueGrid:
                
                break;
            case GridType.PathGrid:
                break;

            case GridType.Boundary:

                break; 
        }
        //Check the type of grid, because sometimes, it is already a Blue Grid maybe even path grid. So i have to find the grid with in the grid ppath and also it should be of Grid Type. 
        //Stop looking in one direction if the type of grid is of type PATHGRID and continue looking in other directions. If every side it returns PATH GRID then, that means its already occupied area. 

       
    }

    private void PathToBlue()
    {
        foreach(Coordinates coord in pathCoordinates)
        {
            Destroy(grid.allGridArray[coord.X, coord.Y]);
            grid.InstantiateBlueSprite(coord.X, coord.Y, sprite);
            for(int i=0; i<GameManager.Instance.numberOfEnemiesAlive; i++)
            {
                GameManager.Instance.enemies[i].GetComponent<Enemy>().pathfinding._grid.GetGridObject(coord.X, coord.Y).isWalkable = false;
            }
        }
        pathCoordinates.Clear();
    }

    public void SetBoundaries()
    {
        for (int x = 0; x < width; x++)
        {
            int y = 0;
            grid.allGridArray[x, y] = Utils.CreateWorldBoundaries(grid.gridArray[x, y].ToString(), sprite, grid.GetWorldPosition(x, y) + new Vector3(grid.GetCellSize(), grid.GetCellSize()) * 0.5f, new Vector2(1.6f, 1.6f), 10, UnityEngine.Color.white,Boundary.Down);
            grid.gridArray[x, y].SetType(GridType.Boundary);
        }
        for (int y = 1; y < height; y++)
        {
            int x = 0;
            grid.allGridArray[x, y] = Utils.CreateWorldBoundaries(grid.gridArray[x, y].ToString(), sprite, grid.GetWorldPosition(x, y) + new Vector3(grid.GetCellSize(), grid.GetCellSize()) * 0.5f, new Vector2(1.6f, 1.6f), 10, UnityEngine.Color.white,Boundary.Left);
            grid.gridArray[x, y].SetType(GridType.Boundary);
        }
        for (int x = 0; x < width; x++)
        {
            int y = height - 1;
            //Debug.LogWarning(x + "," + y);
            grid.allGridArray[x, y] = Utils.CreateWorldBoundaries(grid.gridArray[x, y].ToString(), sprite, grid.GetWorldPosition(x, y) + new Vector3(grid.GetCellSize(), grid.GetCellSize()) * 0.5f, new Vector2(1.6f, 1.6f), 10, UnityEngine.Color.white, Boundary.Up);
            grid.gridArray[x, y].SetType(GridType.Boundary);
        }
        for (int y = 1; y < height; y++)
        {
            int x = width - 1;
            //Debug.LogWarning(x+","+y);
            grid.allGridArray[x, y] = Utils.CreateWorldBoundaries(grid.gridArray[x, y].ToString(), sprite, grid.GetWorldPosition(x, y) + new Vector3(grid.GetCellSize(), grid.GetCellSize()) * 0.5f, new Vector2(1.6f, 1.6f), 10, UnityEngine.Color.white,Boundary.Right);
            grid.gridArray[x, y].SetType(GridType.Boundary);
        }
    }


}

public class Coordinates
{
    public int X
    {
        get
            ; set;
    }
    public int Y { get; set; }
    
    public Coordinates(int x, int y)
    {
        X = x;
        Y = y;  
    }
}
public class GridMapObject
{
    private const string PATHGRID = "PathGrid";
    private const string BLUEGRID = "BlueGrid";
    private const string GRID = "Grid";

    private Grid<GridMapObject> grid;
    private int x, y;
    private GridType type; 
    
    public GridMapObject(Grid<GridMapObject> grid, int x, int y)
    {
        this.grid = grid;
        this.x= x;
        this.y= y;
        
    }
    public void SetType(GridType type)
    {
        this.type = type;
        grid.TriggerGridObjectChanged(x, y);
    }
    public GridType GetType()
    {
        return this.type;
    }
    public override string ToString()
    {
        return type.ToString();
    }
}

public enum GridType
{
    Grid,
    PathGrid,
    BlueGrid,
    Boundary,
    Example
}
