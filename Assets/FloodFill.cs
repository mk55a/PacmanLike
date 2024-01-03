using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloodFill : MonoBehaviour
{
    public static FloodFill Instance { get; private set; }  
    [SerializeField] private float fillDelay = 0.2f;
    private GridManager gridManager;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;

        gridManager = GetComponent<GridManager>();  
    }
    
    private IEnumerator Flood(int x, int y)
    {
        WaitForSeconds wait = new WaitForSeconds(fillDelay);
        if(x>=0 && x<gridManager.width && y>=0 && y < gridManager.height)
        {
            Debug.LogWarning("Checking if  flood is possible");
            //yield return wait;
            //Now check if the tile exists in gridArray and blueArray.If not in blue array turn to blue? 
            Coordinates checkCoord = new Coordinates(x, y);
            if(!gridManager.blueCoordinates.Contains(checkCoord) && !gridManager.boundaryCoordinates.Contains(checkCoord)) //&& !gridManager.blueCoordinates.Contains(new Coordinates(x,y))
            {
                //Change grid to blue as it does not exist 
                Debug.LogWarning(x + "," + y);
                StartCoroutine(gridManager.ConvertToBlueGrid(x, y));

                StartCoroutine(Flood(x+1, y));
                /*StartCoroutine(Flood(x - 1, y));
                StartCoroutine(Flood(x, y+1));
                StartCoroutine(Flood(x, y-1));*/
            }
            yield return wait;
        }
    }
    public void InitiateFlood(int x, int y)
    {
        StartCoroutine(Flood(x, y));
    }
    
}
