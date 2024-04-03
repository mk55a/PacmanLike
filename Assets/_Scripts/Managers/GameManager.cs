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
            if (_instance == null)
                _instance = FindObjectOfType(typeof(GameManager)) as GameManager;
            return _instance;
        }
        set
        {
            _instance = value;
        }
    }
    private static GameManager _instance;

    public int numberOfEnemiesAlive;
    [HideInInspector]
    public GameObject player;
    public List<GameObject> enemies;

    public int numberOfEnemies;
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _gridManager;
    [SerializeField] private GameObject _powerUpPrefab;

    [SerializeField] private Transform[] _enemyAnchorPoints;

    private EventManager.GameState _currentGameState;


    private void OnEnable()
    {
        EventManager.OnGameStateChange += HandleNewGameState;
        LevelManager.onLevelSelected += OnPlay;
    }
    private void OnDisable()
    {
        EventManager.OnGameStateChange -= HandleNewGameState;
        LevelManager.onLevelSelected -= OnPlay;
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
        _gridManager.SetActive(true);
        EventManager.SetGameState(EventManager.GameState.BEGIN);
    }
    void HandleNewGameState(EventManager.GameState newState)
    {
        _currentGameState = newState;

        switch(_currentGameState) { 
            case EventManager.GameState.BEGIN:

                SoundManager.Instance.GameStartSound();
                Time.timeScale = 1;
                player = Instantiate(_playerPrefab, GridManager.Instance.grid.GetWorldPosition(1,1) + new Vector3(GridManager.Instance.gridCellSize, GridManager.Instance.gridCellSize)*0.5f, Quaternion.identity);
                for (int i = 0; i < numberOfEnemies; i++)
                {
                    GameObject enemy = Instantiate(_enemyPrefab, _enemyAnchorPoints[i].position, Quaternion.identity);
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
