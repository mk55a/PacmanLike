using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class LevelButtonHandler : MonoBehaviour
{
    [HideInInspector]
    public LevelStats levelStats;

    [SerializeField]
    private TextMeshProUGUI levelText;
    [SerializeField]
    private Button levelButton;
    private void Start()
    {
        levelText.text = levelStats.levelNo.ToString();
        Debug.Log(levelStats.levelNo);
        levelButton.onClick.AddListener(() =>
        {
            InitializeLevel(levelStats);
        });
    }

    public void InitializeLevel(LevelStats level)
    {
        GridManager.Instance.width = level.gridWidth;
        GridManager.Instance.height = level.gridHeight;
        GridManager.Instance.gridCellSize = level.gridCellSize;

        GameManager.Instance.numberOfEnemies = level.noOfEnemies;
        GameManager.Instance.numberOfEnemiesAlive = level.noOfEnemies;

        UIManager.Instance.levelText.text = level.levelNo.ToString();

        
    }
}
