using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Level")]
public class LevelStats : Level
{
    public int gridWidth, gridHeight;
    public float gridCellSize;

    public int noOfEnemies;

    public int levelNo;
    public int levelDuration;

    public float enemySpeed;
    public int playerSpeed; 
}
