using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Button playButton;
    [SerializeField]
    private Button exitButton;
    [SerializeField]
    private GameObject mainMenuPanel;
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
        Show();
    }
    private void Start()
    {
        playButton.onClick.AddListener(() => GameManager.Instance.OnPlay());
        playButton.onClick.AddListener(Hide);

    }

    public void Show()
    {
        mainMenuPanel.SetActive(true);
    }
    public void Hide()
    {
        mainMenuPanel.SetActive(false);
    }
}
