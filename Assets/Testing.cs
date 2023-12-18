using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using EmptyCharacter.Utils;

public class Testing : MonoBehaviour
{
    [SerializeField] private int width, height;
    [SerializeField] private float gridCellSize;

    private Grid grid; 
    private void Start()
    {
         grid = new Grid(width, height, 2f);
       
    }

    private void Update()
    {
        //Instead of taking the position of the mouse we later take the postion of the player character. 
        if (Input.GetMouseButtonDown(0))
        {
            grid.SetValue(Utils.GetMouseWorldPosition(),10 );
        }
        if(Input.GetMouseButtonDown(1))
        {
            Debug.Log(grid.GetValue(Utils.GetMouseWorldPosition()));
        }
    }
}
