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

    public void CalculateCapturedGrid()
    {

        //Once the area is captured, clear path coordinates list.
        pathCoordinates.Clear();
    }
    public void PlayerOccupyGrid(int x, int y)
    {
        grid.PlayerOccupyGrid(x, y, sprite);
        pathCoordinates.Add(new Coordinates(x, y));
        blueCoordinates.Add(new Coordinates(x, y));
        foreach(Coordinates coord in blueCoordinates) { 
            Debug.LogWarning(coord.X+","+coord.Y);

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

