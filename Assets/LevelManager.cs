using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private GameObject levelButtonPrefab;
    [SerializeField]
    private GameObject levelButtonContainer; 

    public List<LevelStats> levels;

    public delegate void LevelEventChangeHandler();
    public static event LevelEventChangeHandler onLevelSelected;
    private void Start()
    {
        InitializeLevelButtons();
    }

    public void InitializeLevelButtons()
    {
        foreach (LevelStats level in levels)
        {
            GameObject levelButtonObject = Instantiate(levelButtonPrefab, levelButtonContainer.transform);
            levelButtonObject.GetComponent<LevelButtonHandler>().levelStats = level;
            levelButtonObject.GetComponent<Button>().onClick.AddListener(() =>
            {
                onLevelSelected?.Invoke();
            });
            /*Button levelButton = levelButtonObject.GetComponent<Button>();
            levelButton.onClick.AddListener(() => {
                InitializeLevel(level);
            });*/
        }
    }

    

}
