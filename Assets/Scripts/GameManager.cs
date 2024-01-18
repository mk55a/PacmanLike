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

    [SerializeField] public int numberOfEnemies;
    public int numberOfEnemiesAlive; 

    [SerializeField] private Transform[] enemyAnchorPoints; 
    [HideInInspector]
    public GameObject player;
    public List<GameObject> enemies; 

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
        enemies = new List<GameObject>();
        numberOfEnemiesAlive = numberOfEnemies;
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
                for(int i = 0; i< numberOfEnemies; i++)
                {
                    GameObject enemy = Instantiate(enemyPrefab, enemyAnchorPoints[i].position, Quaternion.identity);
                    enemies.Add(enemy);
                    //enemies[i] = Instantiate(enemyPrefab, GridManager.Instance.grid.GetWorldPosition(UnityEngine.Random.Range(4, GridManager.Instance.grid.GetWidth() - 1), UnityEngine.Random.Range(5, GridManager.Instance.grid.GetHeight() - 1)), Quaternion.identity);
                }
                
                EnablePlayerInput(true);
                UIManager.Instance.HideMainMenu();
                EnableEnemy();
                break;
            case EventManager.GameState.PAUSE:
                Time.timeScale = 0;
                EnablePlayerInput(false);
                UIManager.Instance.ShowMainMenu();
                break;

            case EventManager.GameState.CONTINUE:
                Time.timeScale = 1;
                EnablePlayerInput(true);
                UIManager.Instance.HideMainMenu();
                break;

            case EventManager.GameState.GAMEOVER:
                Time.timeScale = 0;
                EnablePlayerInput(false);
                UIManager.Instance.ShowGameOver();
                break;
            case EventManager.GameState.CAPTURE:

                break;
            case EventManager.GameState.CAPTURECOMPLETE:
                break;

        
        }
    }

    public void PlayAgain()
    {

    }
    private void EnablePlayerInput(bool enable)
    {
        player.GetComponent<Player>().EnableControls(enable);
    }
    private void EnableEnemy()
    {
        for(int i=0; i< numberOfEnemies; i++)
        {
            enemies[i].GetComponent<Enemy>().ChangeEnemyState(EnemyState.ATTARGET);
        }
        
    }
}
