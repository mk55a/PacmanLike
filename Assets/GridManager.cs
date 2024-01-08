using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using EmptyCharacter.Utils;
using System.Drawing;
using System.Runtime.CompilerServices;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }
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


    [SerializeField] private float fillDelay;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        pathCoordinates = new List<Coordinates>();
        blueCoordinates = new List<Coordinates>();
        boundaryCoordinates = new List<Coordinates>();
        allCoordinates = new List<Coordinates>();
        capturedCoordinates = new List<Coordinates>();  
    }
    private void Start()
    {
        grid = new Grid<GridMapObject>(width, height, 2f, originObject.transform.position, (Grid<GridMapObject> g, int x, int y)=>new GridMapObject(g,x,y));

        DestroyGridForBoundaries();
        SetBoundaries();
        allCoordinates = grid.AllGrids();
    }
    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
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
        }
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

    public void Connect()
    {
        //Haveto find one grid/Coordinate which lies in the enclosed area of the path.
        int minX = int.MaxValue, minY = int.MaxValue, maxX = int.MinValue, maxY = int.MinValue;
        foreach (Coordinates coord in pathCoordinates)
        {
            //Debug.Log(coord.X + ";" + coord.Y);
            minX = Mathf.Min(minX, coord.X);
            minY = Mathf.Min(minY, coord.Y);
            maxX = Mathf.Max(maxX, coord.X);
            maxY = Mathf.Max(maxY, coord.Y);
        }
        int startPointX = (minX + maxX) / 2;
        int startPointY = (minY + maxY) / 2;
        //grid.InstantiateSelectedSprite(startPointX, startPointY,sprite);
        //Debug.LogWarning(startPointX + ";;" + startPointY);
        PathToBlue();

        
        FloodFill.Instance.InitiateFlood(startPointX, startPointY);

    }

    private void PathToBlue()
    {
        foreach(Coordinates coord in pathCoordinates)
        {
            Destroy(grid.allGridArray[coord.X, coord.Y]);
            grid.InstantiateBlueSprite(coord.X, coord.Y, sprite);
        }
        pathCoordinates.Clear();
    }

    public void SetBoundaries()
    {
        for (int x = 0; x < width; x++)
        {
            int y = 0;
            grid.allGridArray[x, y] = Utils.CreateWorldBoundaries(grid.gridArray[x, y].ToString(), sprite, grid.GetWorldPosition(x, y) + new Vector3(grid.GetCellSize(), grid.GetCellSize()) * 0.5f, new Vector2(1.6f, 1.6f), 10, UnityEngine.Color.white);
            grid.gridArray[x, y].SetType(GridType.Boundary);
        }
        for (int y = 1; y < height; y++)
        {
            int x = 0;
            grid.allGridArray[x, y] = Utils.CreateWorldBoundaries(grid.gridArray[x, y].ToString(), sprite, grid.GetWorldPosition(x, y) + new Vector3(grid.GetCellSize(), grid.GetCellSize()) * 0.5f, new Vector2(1.6f, 1.6f), 10, UnityEngine.Color.white);
            grid.gridArray[x, y].SetType(GridType.Boundary);
        }
        for (int x = 0; x < width; x++)
        {
            int y = height - 1;
            //Debug.LogWarning(x + "," + y);
            grid.allGridArray[x, y] = Utils.CreateWorldBoundaries(grid.gridArray[x, y].ToString(), sprite, grid.GetWorldPosition(x, y) + new Vector3(grid.GetCellSize(), grid.GetCellSize()) * 0.5f, new Vector2(1.6f, 1.6f), 10, UnityEngine.Color.white);
            grid.gridArray[x, y].SetType(GridType.Boundary);
        }
        for (int y = 1; y < height; y++)
        {
            int x = width - 1;
            //Debug.LogWarning(x+","+y);
            grid.allGridArray[x, y] = Utils.CreateWorldBoundaries(grid.gridArray[x, y].ToString(), sprite, grid.GetWorldPosition(x, y) + new Vector3(grid.GetCellSize(), grid.GetCellSize()) * 0.5f, new Vector2(1.6f, 1.6f), 10, UnityEngine.Color.white);
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
    Boundary
}
