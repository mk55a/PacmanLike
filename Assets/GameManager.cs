using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType(typeof(GameManager)) as GameManager;
            return instance;
        }
        set
        {
            instance = value;
        }
    }
    private static GameManager instance;
    public static event EventHandler GameBegins; 
    
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject gridManager; 

    [HideInInspector]
    public GameObject player;
    private GameObject enemies; 

    private EventManager.GameState currentGameState;
    private void OnEnable()
    {
        EventManager.OnGameStateChange += HandleNewGameState;
    }
    private void OnDisable()
    {
        EventManager.OnGameStateChange -= HandleNewGameState;   
    }
    private void Awake()
    {
        if(Instance != null) { 
            Instance=this;  
        }
        
    }
    private void Start()
    {

        
    }
    public void OnPlay()
    {
        gridManager.SetActive(true);
        EventManager.SetGameState(EventManager.GameState.BEGIN);
    }
    void HandleNewGameState(EventManager.GameState newState)
    {
        currentGameState = newState;

        switch(currentGameState) { 
            case EventManager.GameState.BEGIN:
                Time.timeScale = 1;
                player = Instantiate(playerPrefab, GridManager.Instance.grid.GetWorldPosition(1,1) + new Vector3(GridManager.Instance.gridCellSize, GridManager.Instance.gridCellSize)*0.5f, Quaternion.identity); //+new Vector3(GridManager.Instance.gridCellSize,GridManager.Instance.gridCellSize)
                //enemies = Instantiate(enemyPrefab, GridManager.Instance.grid.GetWorldPosition(20,20), Quaternion.identity);
                //EnablePlayerInput(true);
                UIManager.Instance.Hide();
                //EnableEnemy(true);
                break;
            case EventManager.GameState.PAUSE:
                Time.timeScale = 0;
                EnablePlayerInput(false);
                UIManager.Instance.Show();
                break;

            case EventManager.GameState.CONTINUE:
                Time.timeScale = 1;
                EnablePlayerInput(true);
                UIManager.Instance.Hide();
                break;

            case EventManager.GameState.GAMEOVER:
                Time.timeScale = 0;
                EnablePlayerInput(false);
                UIManager.Instance.Show();
                break;
        
        }
    }

    private void EnablePlayerInput(bool enable)
    {
        player.GetComponent<Player>().EnableControls(enable);
    }
    private void EnableEnemy(bool enable)
    {
        enemies.GetComponent<Enemy>().EnableEnemy(enable);
    }
}
