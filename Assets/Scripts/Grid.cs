using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EmptyCharacter.Utils;
using System;
using System.Diagnostics.Tracing;

public class Grid<TGridObject>
{
    public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged; 
    public class OnGridObjectChangedEventArgs : EventArgs
    {
        public int x;
        public int y; 
    }

    private int width, height;
    private float cellSize;
    private Vector3 originPosition;
    public TGridObject[,] gridArray; 

    private TextMesh[,] debugTextArray;
    public GameObject[,] allGridArray;

    public Grid(int width, int height, float cellSize, Vector2 originPosition, Func<Grid<TGridObject>, int, int, TGridObject> createGridObject, bool showDebug)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        gridArray = new TGridObject[width, height];

        allGridArray = new GameObject[width, height];
        //Debug.Log(width + ", " + height);

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {

                gridArray[x, y] = createGridObject(this, x, y);
                //allGridArray[x, y] = Utils.CreateGridColliders(gridArray[x, y].ToString(), GetWorldPosition(x, y) + new Vector2(cellSize, cellSize) * 0.5f, new Vector2(1.6f, 1.6f));
            }
        }

        
        if (showDebug)
        {
            debugTextArray = new TextMesh[width, height];

            for(int x = 0; x< gridArray.GetLength(0); x++)
            {
                for(int y=0; y< gridArray.GetLength(1); y++)
                {
                    debugTextArray[x,y] = Utils.CreateWorldText(gridArray[x,y]?.ToString(), null, GetWorldPosition(x, y)+new Vector3(cellSize,cellSize) * 0.5f,8);
                    
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
                }
            }
            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);
            OnGridObjectChanged += (object sender, OnGridObjectChangedEventArgs eventArgs) =>
            {
                debugTextArray[eventArgs.x, eventArgs.y].text = gridArray[eventArgs.x, eventArgs.y]?.ToString(); //ToString();
            };
        }
        
        }

    public int GetWidth()
    {
        return width;
    }
    public int GetHeight()
    {
        return height;
    }
    public float GetCellSize()
    {
        return cellSize; 
    }
    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize + originPosition;
    }
    public void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition-originPosition).x / cellSize);
        y= Mathf.FloorToInt((worldPosition-originPosition).y / cellSize);
    }
    
    public void SetGridObject(int x, int y, TGridObject value)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x, y] = value;
            if(OnGridObjectChanged != null)
            {
                OnGridObjectChanged(this, new OnGridObjectChangedEventArgs { x=x, y=y });
            }
        }
    }
    public void TriggerGridObjectChanged(int x, int y)
    {
        if (OnGridObjectChanged != null)
        {
            OnGridObjectChanged(this, new OnGridObjectChangedEventArgs { x=x, y=y });
        }
    }
    
    public void SetGridObject(Vector2 worldPosition, TGridObject value)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetGridObject(x, y, value);
    }

    public TGridObject GetGridObject(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];
        }
        else
        {
            return default(TGridObject);    
        }
    }
    public TGridObject GetGridObject(Vector3 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetGridObject(x, y);
    }
    

    public List<Coordinates> AllGrids()
    {
        var list = new List<Coordinates>();
        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                list.Add(new Coordinates(x, y));
            }
        }
        return list;
    }
    public void SetBoundaries(Sprite sprite)
    {
        for (int x = 0; x < width; x++)
        {
            int y = 0;
            allGridArray[x, y] = Utils.CreateWorldBoundaries(gridArray[x, y].ToString(), sprite, GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * 0.5f, new Vector2(1.6f, 1.6f), 10, Color.white,Boundary.Down);
        }
        for (int y = 1; y < height; y++)
        {
            int x = 0;
            allGridArray[x, y] = Utils.CreateWorldBoundaries(gridArray[x, y].ToString(), sprite, GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * 0.5f, new Vector2(1.6f, 1.6f), 10, Color.white,Boundary.Left);
        }
        for (int x = 0; x < width; x++)
        {
            int y = height - 1;
            //Debug.LogWarning(x + "," + y);
            allGridArray[x, y] = Utils.CreateWorldBoundaries(gridArray[x, y].ToString(), sprite, GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * 0.5f, new Vector2(1.6f, 1.6f), 10, Color.white, Boundary.Up);
        }
        for (int y = 1; y < height; y++)
        {
            int x = width - 1;
            //Debug.LogWarning(x+","+y);
            allGridArray[x, y] = Utils.CreateWorldBoundaries(gridArray[x, y].ToString(), sprite, GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * 0.5f, new Vector2(1.6f, 1.6f), 10, Color.white,Boundary.Right);
        }
    }
    public void InstantiatePathSprite(int x, int y, Sprite sprite)
    {
        allGridArray[x, y] = Utils.CreatePathSprite(gridArray[x, y].ToString(), sprite, GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * 0.5f, new Vector2(1.6f, 1.6f),5,Color.white);
    }
    public void InstantiateBlueSprite(int x, int y , Sprite sprite)
    {
        allGridArray[x, y] = Utils.CreateWorldSprite(gridArray[x, y].ToString(), sprite, GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * 0.5f, new Vector2(1.6f, 1.6f), 5, Color.white);
    }

    public void InstantiateSelectedSprite(int x, int y , Sprite sprite) {
        allGridArray[x,y] = Utils.CreateTestSprite(gridArray[x, y].ToString(), sprite, GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * 0.5f, new Vector2(1.6f, 1.6f), 5, Color.red);
    }

}
