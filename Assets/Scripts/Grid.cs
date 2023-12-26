using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EmptyCharacter.Utils;
public class Grid 
{
    private int width, height;
    private float cellSize;
    private Vector2 originPosition;
    private int[,] gridArray;
    private TextMesh[,] debugTextArray;
    public GameObject[,] blueGridArray,pathGridArray;
    public GameObject[,] boundaryGridArray;
    public GameObject[,] allGridArray;
    public Grid(int width, int height, float cellSize, Vector2 originPosition)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;   

        gridArray = new int[width, height];
        //debugTextArray = new TextMesh[width, height];
        blueGridArray = new GameObject[width, height];
        pathGridArray = new GameObject[width, height];
        boundaryGridArray = new GameObject[width, height];
        allGridArray = new GameObject[width, height];
        Debug.Log(width + ", " + height);

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                allGridArray[x, y] = Utils.CreateGridColliders(gridArray[x, y].ToString(), GetWorldPosition(x, y) + new Vector2(cellSize, cellSize) * 0.5f, new Vector2(1.6f,1.6f));
                //Debug.Log(x + "," + y);
                //debugTextArray[x,y]= Utils.CreateWorldText(gridArray[x, y].ToString(), null, GetWorldPosition(x, y) + new Vector2(cellSize, cellSize)* 0.5f, 12, Color.white, TextAnchor.MiddleCenter);
                //Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                //Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
            }
        }
        //Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);

        //Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);

        
    }
    public List<Coordinates> AllGrids()
    {
        var list = new List<Coordinates>();
        for(int x = 0;x < gridArray.GetLength(0); x++)
        {
            for(int y=0; y< gridArray.GetLength(1); y++)
            {
                list.Add(new Coordinates(x,y));
            }
        }
        return list;
    }
    public void SetBoundaries(Sprite sprite)
    {
        for (int x=0; x<width; x++)
        {
            int y = 0;
            boundaryGridArray[x, y] = Utils.CreateWorldBoundaries(gridArray[x, y].ToString(), sprite, GetWorldPosition(x, y) + new Vector2(cellSize, cellSize) * 0.5f, new Vector2(1.6f, 1.6f), 10, Color.white);
        }
        for(int y = 1; y<height; y++)
        {
            int x = 0;
            boundaryGridArray[x, y] = Utils.CreateWorldBoundaries(gridArray[x, y].ToString(), sprite, GetWorldPosition(x, y) + new Vector2(cellSize, cellSize) * 0.5f, new Vector2(1.6f, 1.6f), 10, Color.white);
        }
        for (int x = 0; x < width; x++)
        {
            int y = height-1;
            //Debug.LogWarning(x + "," + y);
            boundaryGridArray[x, y] = Utils.CreateWorldBoundaries(gridArray[x, y].ToString(), sprite, GetWorldPosition(x, y) + new Vector2(cellSize, cellSize) * 0.5f, new Vector2(1.6f, 1.6f), 10, Color.white);
        }
        for (int y = 1; y < height; y++)
        {
            int x = width-1;
            //Debug.LogWarning(x+","+y);
            boundaryGridArray[x, y] = Utils.CreateWorldBoundaries(gridArray[x, y].ToString(), sprite, GetWorldPosition(x, y) + new Vector2(cellSize, cellSize) * 0.5f, new Vector2(1.6f, 1.6f), 10, Color.white);
        }
    }
    /*public void OccupyGrid(Vector2 worldPosition, Sprite sprite)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);

        blueGridArray[x, y] = Utils.CreateWorldSprite(gridArray[x, y].ToString(), sprite, GetWorldPosition(x,y) + new Vector2(cellSize, cellSize)*0.5f,new Vector2(1.6f,1.6f), 10, Color.white);
    }*/
    public void PlayerOccupyPathGrid(int x, int y, Sprite sprite)
    {
        
        pathGridArray[x, y] =  Utils.CreatePathSprite(gridArray[x, y].ToString(), sprite, GetWorldPosition(x, y) + new Vector2(cellSize, cellSize) * 0.5f, new Vector2(1.6f, 1.6f), 10, Color.white); //Utils.CreateWorldSprite(gridArray[x,y].ToString(), sprite, GetWorldPosition(x,y)+new Vector2(cellSize, cellSize)*0.5f, new Vector2(1.6f,1.6f), 10,Color.white);

    }
    public void PlayerOccupyBlueGrid(int x, int y, Sprite sprite)
    {
        blueGridArray[x,y] = Utils.CreateWorldSprite(gridArray[x,y].ToString(), sprite, GetWorldPosition(x,y)+ new Vector2(cellSize,cellSize) *0.5f, new Vector2(1.6f,1.6f), 10, Color.white) ;

    }
    private Vector2 GetWorldPosition(int x, int y)
    {
        
        return new Vector2(x, y) * cellSize + originPosition;
    }
    public void GetXY(Vector2 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition-originPosition).x / cellSize);
        y= Mathf.FloorToInt((worldPosition-originPosition).y / cellSize);
    }
    public void SetValue(int x, int y, int value)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x, y] = value;
            debugTextArray[x, y].text = gridArray[x,y].ToString();
        }
        
    }
    public void SetValue(Vector2 worlPosition, int value)
    {
        int x, y;
        GetXY(worlPosition,out x,out y);
        SetValue(x, y, value);
    }

    public int GetValue(int x, int y)
    {
        if(x>=0 && y>=0 && x<width && y < height)
        {
            return gridArray[x, y]; 
        }
        else
        {
            return 0;
        }
    }
    public int GetValue(Vector2 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetValue(x, y);
    }
    

    
}
