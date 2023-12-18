using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Testing : MonoBehaviour
{
    [SerializeField] private int width, height;
    [SerializeField] private float gridCellSize;
    private void Start()
    {
        Grid grid = new Grid(width, height, gridCellSize);
       
    }
}
