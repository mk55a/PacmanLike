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
        });
        playButton.onClick.AddListener(HideMainMenu);

    }

    public void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);
    }
    public void HideMainMenu()
    {
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
        HideScore();
        playAgainButton.onClick.AddListener(() => GameManager.Instance.PlayAgain());
        playAgainButton.onClick.AddListener(() =>
        {
            HideGameOver();
            ShowScore();
        });
        mainMenuButton.onClick.AddListener(() =>
        {
            HideGameOver();
            ShowMainMenu();

        });
        gameOverPanel.SetActive(true);
    }
    public void HideGameOver()
    {
        playAgainButton.onClick.RemoveAllListeners();
        mainMenuButton.onClick.RemoveAllListeners();
        gameOverPanel.SetActive(false);
    }
}
