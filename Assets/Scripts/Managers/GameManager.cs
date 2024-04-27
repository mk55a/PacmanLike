using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] public float levelDuration;
    [SerializeField] public float powerUpPointer;

    public int numberOfEnemiesAlive;
    public float powerUpCountdown;

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
        currentGameState = EventManager.GameState.MAINMENU ;
    }
    
    public void OnPlay()
    {
        gridManager.SetActive(true);
        EventManager.SetGameState(EventManager.GameState.GAME);
    }
    void HandleNewGameState(EventManager.GameState newState)
    {
        
        if(currentGameState != newState)
        {
            Debug.LogWarning("changed Current state");
            currentGameState = newState;
            Debug.LogWarning(currentGameState);
            switch (currentGameState)
            {
                case EventManager.GameState.GAME:
                    UIManager.Instance.SetCurrentTime();
                    SoundManager.Instance.GameStartSound();
                    Debug.LogWarning("GAME");
                    if (GridManager.Instance.grid == null)
                    {
                        GridManager.Instance.InitiateGrid();
                    }
                    enemies = new List<GameObject>();
                    numberOfEnemiesAlive = numberOfEnemies;
                    player = Instantiate(playerPrefab, GridManager.Instance.grid.GetWorldPosition(1, 1) + new Vector3(GridManager.Instance.gridCellSize, GridManager.Instance.gridCellSize) * 0.5f, Quaternion.identity);
                    for (int i = 0; i < numberOfEnemies; i++)
                    {
                        GameObject enemy = Instantiate(enemyPrefab, enemyAnchorPoints[i].position, Quaternion.identity);
                        enemies.Add(enemy);

                    }

                    UIManager.Instance.HideMainMenu();

                    Time.timeScale = 1;
                    EnablePlayerInput(true);
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

                    Debug.LogWarning("GAME OVER");
                    Time.timeScale = 0;

                    //UIManager.Instance.ShowGameOver();
                    //GridManager.Instance.CleanGridStats();
                    if (player != null)
                    {
                        EnablePlayerInput(false);
                        Destroy(player.gameObject);
                        player = null;
                    }
                    if (enemies != null)
                    {
                        for (int i = 0; i < numberOfEnemiesAlive; i++)
                        {
                            Destroy(enemies[i].gameObject);
                        }
                    }
                    break;
                case EventManager.GameState.MAINMENU:
                    UIManager.Instance.ShowMainMenu();
                    break;



            }
        }
        
    }

    public float Score()
    {
        float score = (GridManager.Instance.capturedNumberOfGrids / GridManager.Instance.totalNumberOfGrids) * 100f;
        if (score == powerUpPointer)
        {
            InstantiatePowerUp();
        }
        //Debug.LogError(GridManager.Instance.capturedNumberOfGrids + ", "+ GridManager.Instance.totalNumberOfGrids + " "+ "Score :" + score);
        return score;
    }
    private Coordinates RandomIndex()
    {
        
       List<Coordinates> selectables = GridManager.Instance.allCoordinates.Concat(GridManager.Instance.boundaryCoordinates).Concat(GridManager.Instance.blueCoordinates).Concat(GridManager.Instance.pathCoordinates).Distinct().ToList();
        int randomIndex;
        if (selectables.Count > 0)
        {
            randomIndex = UnityEngine.Random.Range(0, selectables.Count);
            return selectables[randomIndex];
        }
        else
        {
            return new Coordinates(0, 0);
        }
        
    }
    bool powerUpInGame = false;
    public void InstantiatePowerUp()
    {
        //produce random coordinates
        Coordinates selected = RandomIndex();
        if(!powerUpInGame)
        {
            Instantiate(PowerUpPrefab, GridManager.Instance.grid.GetWorldPosition(selected.X, selected.Y), Quaternion.identity);
            powerUpInGame = true;
        }
        
    }
    private void EnablePlayerInput(bool enable)
    {
        player.GetComponent<Player>().EnableControls(enable);
    }
    private void EnableEnemy()
    {
        for(int i=0; i< numberOfEnemies; i++)
        {
            /*enemies[i].GetComponent<Enemy>().ChangeEnemyState(EnemyState.ATTARGET);*/
        }
        
    }
}
