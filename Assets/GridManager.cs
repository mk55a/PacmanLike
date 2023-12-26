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
    public void Connect()
    {
        //all path grid become blue grid. 
        foreach(Coordinates pathCoord in pathCoordinates)
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
        Debug.Log("Calculating Area");
        int startingX, startingY, endingX, endingY;
        Vector2 startingVector = new Vector2(0,0);
        Vector2 endingVector = new Vector2(0,0);
        Debug.Log("Count : " + pathCoordinates.Count);
        for(int i=0; i< pathCoordinates.Count; i++)
        {
            if (i == 0)
            {
                startingX = pathCoordinates[i].X;   
                startingY = pathCoordinates[i].Y;  
                startingVector.x = pathCoordinates[i].X;    
                startingVector.y = pathCoordinates[i].Y;

            }
            if (i == pathCoordinates.Count - 1)
            {
                endingVector.y = pathCoordinates[i].Y;  
                endingVector.x = pathCoordinates[i].X;
                endingX = pathCoordinates[i].X; 
                endingY = pathCoordinates[i].Y;
            }
            Debug.Log(startingVector + ","+endingVector);

            /*float t =0.05f;

            Vector2 interpolatedPoint = Vector2.Lerp(Vector2.Lerp(startingVector, new Vector2(pathCoordinates[i].X, pathCoordinates[i].Y), t), Vector2.Lerp(new Vector2(pathCoordinates[i].X, pathCoordinates[i].Y), endingVector, t),t);
            int x, y;
            grid.GetXY(interpolatedPoint, out x,out y);
            PlayerOccupyGrid(x, y);*/

        }
        //Once the area is captured, clear path coordinates list.
        //pathCoordinates.Clear();
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

