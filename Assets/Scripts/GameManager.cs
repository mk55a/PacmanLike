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
    [SerializeField] private GameObject PowerUpPrefab;

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
        EventManager.SetGameState(EventManager.GameState.MAINMENU);
        
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

                SoundManager.Instance.GameStartSound();
                Time.timeScale = 1;
                player = Instantiate(playerPrefab, GridManager.Instance.grid.GetWorldPosition(1,1) + new Vector3(GridManager.Instance.gridCellSize, GridManager.Instance.gridCellSize)*0.5f, Quaternion.identity);
                for (int i = 0; i < numberOfEnemies; i++)
                {
                    GameObject enemy = Instantiate(enemyPrefab, enemyAnchorPoints[i].position, Quaternion.identity);
                    enemies.Add(enemy);

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
            case EventManager.GameState.MAINMENU:

                break;


        
        }
    }

    public float Score()
    {
        float score = (GridManager.Instance.capturedNumberOfGrids / GridManager.Instance.totalNumberOfGrids) * 100f;
        //Debug.LogError(GridManager.Instance.capturedNumberOfGrids + ", "+ GridManager.Instance.totalNumberOfGrids + " "+ "Score :" + score);
        return score;
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
