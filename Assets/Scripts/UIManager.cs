using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Button playButton;
    [SerializeField]
    private Button exitButton;
    [SerializeField]
    private GameObject mainMenuPanel;

    [SerializeField]
    private GameObject gameOverPanel;
    [SerializeField]
    private Button playAgainButton;
    [SerializeField]
    private Button mainMenuButton;

    [SerializeField]
    private GameObject scorePanel;
    [SerializeField]
    private TextMeshProUGUI levelText;
    [SerializeField]
    private TextMeshProUGUI scoreText;
    [SerializeField]
    private TextMeshProUGUI clockText;
    [SerializeField]
    private GameObject powerUpImage;
    [SerializeField]
    private GameObject enemyCapturedPrefab;
    [SerializeField]
    private GameObject enemyCapturedContainer;

    [SerializeField]
    private GameObject victoryPanel;


    
    public static UIManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType(typeof(UIManager)) as UIManager;
            return instance;
        }
        set
        {
            instance = value;
        }
    }
    private static UIManager instance;
    private void Awake()
    {
        ShowMainMenu();
        HideScore();
        HideGameOver();
    }
    private void Start()
    {
        playButton.onClick.AddListener(() => {
            GameManager.Instance.OnPlay();
            ShowScore();
            SoundManager.Instance.ButtonClickSound();
        });
        playButton.onClick.AddListener(HideMainMenu);

        SetCurrentTime();


    }
    public void SetCurrentTime()
    {
        levelTime = GameManager.Instance.levelDuration;
        currentTime = levelTime;
    }
    private void Update()
    {
        scoreText.text = GameManager.Instance.Score().ToString() + "%";
        if (EventManager.GetGameState != EventManager.GameState.MAINMENU && EventManager.GetGameState != EventManager.GameState.GAMEOVER && EventManager.GetGameState != EventManager.GameState.VICTORY)
        {
            //Debug.LogError("NOT IN MAIN MENY");
            UpdateTimer();
            
        }
    }
    public void UpdateScore()
    {
        while(EventManager.GetGameState != EventManager.GameState.MAINMENU)
        {
            scoreText.text = GameManager.Instance.Score().ToString() + "%";
        }
    }
    public void ShowMainMenu()
    {
        SoundManager.Instance.StartBgMusic();
        mainMenuPanel.SetActive(true);
    }
    public void HideMainMenu()
    {
        SoundManager.Instance.StopBgMusic();
        mainMenuPanel.SetActive(false);
    }

    public void ShowScore()
    {
        scorePanel.SetActive(true);
    }
    public void HideScore() { 
        ResetScore();
        scorePanel.SetActive(false);
    }
    private void ResetScore()
    {
        clockText.text = "00:00:00";
        levelText.text = "Level 0";
        scoreText.text = "0%";
        foreach(Transform child in enemyCapturedContainer.transform)
        {
            Destroy(child.gameObject);
        }
    }
    public void ShowPowerUp()
    {
        powerUpImage.SetActive(true);
    }
    public void HidePowerUp()
    {
        powerUpImage.SetActive(false);
    }
    public void AddEnemiesCaptured()
    {
        Instantiate(enemyCapturedPrefab, enemyCapturedContainer.transform.position, Quaternion.identity);
    }

    public void ShowGameOver()
    {
        SoundManager.Instance.GameOverSound();
        HideScore();

        playAgainButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.ButtonClickSound();
            PlayAgain();
            HideGameOver();
            ShowScore();
            
        });
        mainMenuButton.onClick.AddListener(() =>
        {
            HideGameOver();
            ShowMainMenu();
            SoundManager.Instance.ButtonClickSound();

        });
        gameOverPanel.SetActive(true);
    }
    public void HideGameOver()
    {
        playAgainButton.onClick.RemoveAllListeners();
        mainMenuButton.onClick.RemoveAllListeners();
        gameOverPanel.SetActive(false);
    }

    public float currentTime;
    public float levelTime; 
    private void UpdateTimer()
    {
        //Debug.Log(currentTime);
        UpdateClockDisplay();
        if (currentTime > 0f)
        {
            currentTime -= Time.deltaTime;
            
        }
        else
        {
            EventManager.SetGameState(EventManager.GameState.GAMEOVER);
        }
    }
    public void UpdateClockDisplay()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);   
        clockText.text = string.Format("{00:00}:{01:00}", minutes, seconds);
    }

    public void PlayAgain()
    {
        //Reset Grid. 
        //Reinstantiate enemies.
        EventManager.SetGameState(EventManager.GameState.GAME);
    }
}
