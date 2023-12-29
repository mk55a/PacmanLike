using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using EmptyCharacter.Utils;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }
    [SerializeField] private int width, height;
    [SerializeField] public float gridCellSize;
    [SerializeField] public Sprite sprite;
    [SerializeField] public GameObject originObject;
    public Grid grid;


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
        grid = new Grid(width, height, 2f, originObject.transform.position);
        grid.SetBoundaries(sprite);
        
        allCoordinates = grid.AllGrids();
        //PrintAllCoordinates();
    }
    private void PrintAllCoordinates()
    {
        foreach(Coordinates coord in allCoordinates)
        {
            Debug.LogWarning(coord.X+","+coord.Y);
        }
    }
    public void Connect()
    {
        //Debug.Log("Connect : " + new Vector2(pathCoordinates[0].X, pathCoordinates[0].Y) + " :: " + new Vector2(pathCoordinates[pathCoordinates.Count -1].X, pathCoordinates[pathCoordinates.Count - 1].Y));
        //all path grid become blue grid. 
        CalculateCapturedGrid();



        foreach (Coordinates pathCoord in pathCoordinates)
        {
            blueCoordinates.Add(pathCoord);
            grid.PlayerOccupyBlueGrid(pathCoord.X, pathCoord.Y, sprite);
            Destroy(grid.pathGridArray[pathCoord.X,pathCoord.Y]);
        }


        

        //sCheckAround(x, y);  
    }
    public void CheckAround(int x, int y)
    {
       
    }
    public void CalculateCapturedGrid()
    {
        int minX= int.MaxValue ,minY = int.MaxValue, maxX=int.MinValue, maxY=int.MinValue;
        //Currently this is calculating all the coordinates. Instead get the startPath and reduce it from the list of path coordinates. So we get smaller blocks. Also it is crashing. 
        foreach (Coordinates coord in pathCoordinates)
        {
            minX = Mathf.Min(minX, coord.X);
            minY= Mathf.Min(minY, coord.Y);
            maxX = Mathf.Max(maxX, coord.X);
            maxY = Mathf.Max(maxY, coord.Y);
        }

        for(int x = minX; x <= maxX; x++)
        {
            for(int y = minY; y <= maxY; y++)
            {
                capturedCoordinates.Add(new Coordinates(x,y));
            }
        }
        foreach(Coordinates coord in capturedCoordinates)
        {
            PlayerOccupyPathGrid(coord.X,coord.Y);
        }
    }

    
    public void PlayerOccupyPathGrid(int x, int y)
    {
        grid.PlayerOccupyPathGrid(x, y, sprite);
        Destroy(grid.allGridArray[x, y]);
        pathCoordinates.Add(new Coordinates(x, y));
        
        /*foreach(Coordinates coord in blueCoordinates) { 
            Debug.LogWarning(coord.X+","+coord.Y);

        }*/
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

