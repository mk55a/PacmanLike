using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding 
{
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 10;
    public static Pathfinding Instance { get; private set; }

    public Grid<PathNode> _grid;
    private List<PathNode> openList; 
    private List<PathNode> closedList;
    public Pathfinding(int width, int height)
    {
        if (Instance == null)
        {
            Instance = this;
        }
        //_grid = new Grid<PathNode>(width, height,2f,Vector2.zero,(Grid<PathNode> g, int x, int y)=> new PathNode(g, x, y));
        _grid = new Grid<PathNode>(width, height,2f,GridManager.Instance.originObject.transform.position,(Grid<PathNode> g, int x, int y)=> new PathNode(g, x, y),false);
    }
    public Grid<PathNode> GetGrid()
    {
        return _grid;
    }
    public List<Vector3> FindPath(Vector3 startWorldPosition, Vector3 endWorldPosition)
    {
        _grid.GetXY(startWorldPosition, out int startX, out int startY);
        _grid.GetXY(endWorldPosition, out int endX, out int endY);

        Debug.Log("Path is  " + startX + "," + startY + "," + endX + "," + endY);
        List<PathNode> path = FindPath(startX, startY, endX, endY);
        
        if (path == null)
        {
            Debug.Log("Path is nul "+ startX+","+startY+","+endX+","+endY);
            return null; 
        }
        else
        {
            List<Vector3> vectorPath = new List<Vector3>();
            foreach(PathNode pathNode in path)
            {
                Debug.Log("Path : " + pathNode.x + ", "+ pathNode.y);
                //vectorPath.Add(new Vector3(pathNode.x, pathNode.y) * _grid.GetCellSize() + Vector3.one * _grid.GetCellSize()*0.5f);
                vectorPath.Add(GetGrid().GetWorldPosition(pathNode.x, pathNode.y));
            }
            //Debug.Log("VectorPath: " + vectorPath.Count);
           
            return vectorPath;
        }
    }
    public List<PathNode> FindPath(int startX, int startY, int endX, int endY)
    {
        PathNode startNode =  _grid.GetGridObject(startX, startY);
        PathNode endNode = _grid.GetGridObject(endX+1, endY+1); 
        if(startNode==null || endNode == null)
        {
            Debug.Log("Nodes are null");
            return null;
        }

        openList = new List<PathNode> { startNode};
        closedList = new List<PathNode>();

        for(int x=0; x<_grid.GetWidth(); x++)
        {
            for(int y=0; y<_grid.GetHeight(); y++)
            {
                PathNode pathNode= _grid.GetGridObject(x, y);
                pathNode.gCost = 999999999;
                pathNode.CalculateFCost();
                pathNode.cameFromNode = null;
            }
        }

        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();

        while(openList.Count > 0)
        {
            PathNode currentNode = GetLowestFCostNode(openList);
            if(currentNode == endNode)
            {
                return CalculatedPath(endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach(PathNode neighbourNode in GetNeighbourList(currentNode))
            {
                if (closedList.Contains(neighbourNode)) continue;
                //Debug.LogWarning("neig: "+neighbourNode.ToString()+" , " + neighbourNode.isWalkable);
                if (!neighbourNode.isWalkable)
                {
                    closedList.Add(neighbourNode);
                    continue;
                }
                int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode);
                if(tentativeGCost< neighbourNode.gCost)
                {
                    neighbourNode.cameFromNode = currentNode;
                    neighbourNode.gCost=tentativeGCost;
                    neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endNode);
                    neighbourNode.CalculateFCost();

                    if (!openList.Contains(neighbourNode))
                    {
                        openList.Add(neighbourNode);   
                    }
                }
            }
        }
        //Out of nodes on the open list
        return null;
    }

    private List<PathNode> GetNeighbourList(PathNode currentNode)
    {
        List<PathNode> neighbourList = new List<PathNode>();
        //Left
        if(currentNode.x-1 > 0) //>=
        {
            neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y));
            //LeftDown
            /*if (currentNode.y - 1 >= 0) neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y - 1));
            // Left Up
            if (currentNode.y + 1 < _grid.GetHeight()) neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y + 1));*/
        }
        //Right
        if (currentNode.x + 1 < _grid.GetWidth()-1)
        {
            neighbourList.Add(GetNode(currentNode.x+1, currentNode.y));
            // Right Down
            /*if (currentNode.y-1 >= 0){
                neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y - 1));
            }
            // Right Up
            if (currentNode.y + 1 < _grid.GetHeight()) neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y + 1));*/
        }
        //Down
        if (currentNode.y - 1 > 0)
        {
            neighbourList.Add(GetNode(currentNode.x, currentNode.y - 1));
        }
        //Up
        if (currentNode.y + 1 < _grid.GetHeight()-1)
        {
            neighbourList.Add(GetNode(currentNode.x, currentNode.y + 1));
        }

        return neighbourList;

    }
    private PathNode GetNode(int x, int y)
    {
        return _grid.GetGridObject(x,y);
    }
    
    private List<PathNode> CalculatedPath(PathNode endNode)
    {
        List<PathNode> path = new List<PathNode>();
        path.Add(endNode);
        PathNode currentNode = endNode; 
        while(currentNode.cameFromNode != null)
        {
            path.Add(currentNode.cameFromNode);
            currentNode = currentNode.cameFromNode;
        }
        path.Reverse();
        return path;
    }
    private int CalculateDistanceCost(PathNode a, PathNode b)
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int remaining = Mathf.Abs(xDistance - yDistance);

        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    private PathNode GetLowestFCostNode(List<PathNode> pathNodelist)
    {
        PathNode lowestFCostNode = pathNodelist[0];
        for(int i = 1; i<pathNodelist.Count; i++)
        {
            if (pathNodelist[i].fCost < lowestFCostNode.fCost)
            {
                lowestFCostNode = pathNodelist[i];

            }
        }
        return lowestFCostNode;
    }
}
