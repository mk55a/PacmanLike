using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip buttonClickSound;
    [SerializeField] private AudioClip bgMusic;
    [SerializeField] private AudioClip gameOverSound;
    [SerializeField] private AudioClip gameStartSound;
    [SerializeField] private AudioClip floodFillSound;
    [SerializeField] private AudioClip powerUpSound;
    
    [SerializeField]
    private AudioSource effectsAudioSource;
    [SerializeField]
    private AudioSource musicAudioSource;


    private AudioSource playerAudioSource; 
    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType(typeof(SoundManager)) as SoundManager;
            return instance;
        }
        set
        {
            instance = value;
        }
    }
    private static SoundManager instance;

    private void Update()
    {
        /*switch (EventManager.GetGameState)
        {
            case EventManager.GameState.BEGIN:

                playerAudioSource = FindObjectOfType<Player>().GetComponent<AudioSource>();
                break;
            default:
                return; 
        };*/
    }

    public void PlayerOcuupyGridSound()
    {
         /*if(playerAudioSource != null) {
            playerAudioSource.clip = powerUpSound;
            playerAudioSource.Play();
        } */
    }

    public void ButtonClickSound()
    {
        effectsAudioSource.clip = buttonClickSound;
        effectsAudioSource.Play();
    }
    public void StartBgMusic()
    {
        musicAudioSource.clip = bgMusic;
        musicAudioSource.Play();   
    }
    public void StopBgMusic()
    {
        musicAudioSource.Stop();
    }
    public void FloodFillSound()
    {
        effectsAudioSource.clip = floodFillSound;
        effectsAudioSource.Play();
    }
    public void PowerUpSound()
    {

    }
    public void GameOverSound()
    {
        effectsAudioSource.clip = gameOverSound;
        effectsAudioSource.Play();
    }
    public void GameStartSound()
    {
        effectsAudioSource.clip = gameStartSound;
        effectsAudioSource.Play();
    }
}
