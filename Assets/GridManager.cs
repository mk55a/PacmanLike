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
        grid = new Grid<GridMapObject>(width, height, 2f, originObject.transform.position, ()=>new GridMapObject());

        DestroyGridForBoundaries();
        grid.SetBoundaries(sprite);
        allCoordinates = grid.AllGrids();
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
        //Debug.LogWarning(startPointX + ";;" + startPointY);
        

        FloodFill.Instance.InitiateFlood(startPointX, startPointY);

    }
    public IEnumerator PlayerOccupyPathGrid(int x, int y)
    {
        Destroy(grid.allGridArray[x, y]);
        grid.PlayerOccupyPathGrid(x, y, sprite);
        
        pathCoordinates.Add(new Coordinates(x, y));
        yield return null;
        
    }
    public void ConvertPathToBlue()
    {
        foreach(Coordinates pathcoord in pathCoordinates)
        {
            Destroy(grid.allGridArray[pathcoord.X, pathcoord.Y]);
            grid.PlayerOccupyBlueGrid(pathcoord.X, pathcoord.Y, sprite);
            blueCoordinates.Add(pathcoord);
            


        }
        pathCoordinates.Clear();
    }
    public IEnumerator ConvertToBlueGrid(int x, int y)
    {
        Destroy(grid.allGridArray[x, y]);
        grid.PlayerOccupyBlueGrid(x, y, sprite);
        
        blueCoordinates.Add(new Coordinates(x, y));
        yield return null;
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

    public GridType type; 

    public GridMapObject()
    {
        this.type = GridType.Grid;
    }
    public void SetType(GridType type)
    {
        this.type = type;
    }
    public GridType GetType()
    {
        return this.type;
    }
}

public enum GridType
{
    Grid,
    PathGrid,
    BlueGrid
}
