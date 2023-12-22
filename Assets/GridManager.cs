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

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
    private void Start()
    {
        grid = new Grid(width, height, 2f, originObject.transform.position);
        grid.SetBoundaries(sprite);
       
    }

    public void PlayerOccupyGrid(int x, int y)
    {
        grid.PlayerOccupyGrid(x, y, sprite);
    }
}
