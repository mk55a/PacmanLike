using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PointInPolygon : MonoBehaviour
{

    public Transform[] polygonVertices;
    public int vertexCount; 

    public static PointInPolygon Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType(typeof(PointInPolygon)) as PointInPolygon;
            return instance;
        }
        set
        {
            instance = value;
        }
    }
    private static PointInPolygon instance;
    public Vector2 SetPolygonVertices()
    {
        /*if(GridManager.Instance.pathCoordinates == null)
        {
            Debug.LogError("No Path ");
            return;
        }*/

        vertexCount = GridManager.Instance.pathCoordinates.Count;
        polygonVertices = new Transform[vertexCount];
        /*if (GridManager.Instance.boundaryStartCoordinates.X == GridManager.Instance.boundaryEndCoordinates.X || GridManager.Instance.boundaryEndCoordinates.X == GridManager.Instance.boundaryEndCoordinates.Y)
        {
            vertexCount = GridManager.Instance.pathCoordinates.Count+4;
            //if either of them are on same axis at start and end then only extra vertices will be addedd.
            polygonVertices = new Transform[vertexCount];
        }
        else
        {
            vertexCount = GridManager.Instance.pathCoordinates.Count+3;
            polygonVertices = new Transform[vertexCount];

        }*/

        for (int i = 0; i < vertexCount; i++)
        {
            Debug.LogError(GridManager.Instance.pathCoordinates[i].X + ","+ GridManager.Instance.pathCoordinates[i].Y);
            GameObject vertex = new GameObject("Vertex");
            vertex.transform.position = GridManager.Instance.grid.GetWorldPosition(GridManager.Instance.pathCoordinates[i].X, GridManager.Instance.pathCoordinates[i].Y);
            polygonVertices[i] = vertex.transform;

        }
        AddNewVertex(GridManager.Instance.boundaryStartCoordinates);
        AddNewVertex(GridManager.Instance.boundaryEndCoordinates);
        CheckForBoundaries();
        return FindPointInsidePolygon();
    }
    void CheckForBoundaries()
    {
        if(GridManager.Instance.boundaryStartCoordinates.X==0 && GridManager.Instance.boundaryEndCoordinates.X == 0)
        {
            if(GridManager.Instance.boundaryStartCoordinates.Y< GridManager.Instance.boundaryEndCoordinates.Y)
            {
                for (int i = GridManager.Instance.boundaryStartCoordinates.Y+1; i < GridManager.Instance.boundaryEndCoordinates.Y; i++)
                {
                    Debug.Log("0" + "," + i);
                    AddNewVertex(new Coordinates(0, i));
                }
            }
            else
            {
                for(int i= GridManager.Instance.boundaryEndCoordinates.Y+1; i< GridManager.Instance.boundaryStartCoordinates.Y; i++)
                {
                    Debug.Log("0" + "," + i);
                    AddNewVertex(new Coordinates(0,i));
                }
            }
        }
        if (GridManager.Instance.boundaryStartCoordinates.Y == 0 && GridManager.Instance.boundaryEndCoordinates.Y == 0)
        {
            if (GridManager.Instance.boundaryStartCoordinates.X < GridManager.Instance.boundaryEndCoordinates.X)
            {
                for (int i = GridManager.Instance.boundaryStartCoordinates.X+1; i < GridManager.Instance.boundaryEndCoordinates.X; i++)
                {
                    Debug.Log(i + "," + "0");
                    AddNewVertex(new Coordinates(i, 0));
                }
            }
            else
            {
                for (int i = GridManager.Instance.boundaryEndCoordinates.X+1; i < GridManager.Instance.boundaryStartCoordinates.X; i++)
                {
                    Debug.Log(i + "," + "0");
                    AddNewVertex(new Coordinates(i, 0));
                }
            }
        }
    }
    //To check if point is in the polygon
    bool PointInPolygonCheck(Vector2 point)
    {
        int vertexCount = polygonVertices.Length;
        bool isInside = false;

        for (int i = 0, j = vertexCount - 1; i < vertexCount; j = i++)
        {
            if (((polygonVertices[i].position.y > point.y) != (polygonVertices[j].position.y > point.y)) &&
                (point.x < (polygonVertices[j].position.x - polygonVertices[i].position.x) * (point.y - polygonVertices[i].position.y) / (polygonVertices[j].position.y - polygonVertices[i].position.y) + polygonVertices[i].position.x))
            {
                isInside = !isInside;
            }
        }

        return isInside;
    }

    Vector2 FindPointInsidePolygon()
    {
        Vector2 insidePoint = Vector2.zero;
        foreach (Transform vertex in polygonVertices)
        {
            insidePoint += (Vector2)vertex.position;
        }
        Debug.Log(polygonVertices.Length);
        insidePoint /= polygonVertices.Length;




        return insidePoint;
    }

    void AddNewVertex(Coordinates newCoordinate)
    {
        // Create a new GameObject for the new vertex
        GameObject newVertex = new GameObject("NewVertex");
        newVertex.transform.position = new Vector3(newCoordinate.X, newCoordinate.Y);

        // Add the new vertex to the polygonVertices array
        vertexCount++;
        System.Array.Resize(ref polygonVertices, polygonVertices.Length + 1);
        polygonVertices[polygonVertices.Length - 1] = newVertex.transform;
    }
}
