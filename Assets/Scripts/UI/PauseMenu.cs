using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public class PauseMenu : MonoBehaviour
{
    //* Manages the pause menu and pausing of the game
    // keep track of whether the game is paused
    public static bool paused = false;

    // events for each button
    public UnityEvent onResumeClicked;
    public UnityEvent onSettingsClicked;
    public UnityEvent onExitClicked;
    public UnityEvent onPauseClicked;

    // c# events for each button
    public static event Action OnPause;
    public static event Action OnResume;

    // button references
    public Button resumeButton;
    public Button settingsButton;
    public Button exitButton;

    void Start()
    {
        // add listeners to the buttons
        resumeButton.onClick.AddListener(OnResumeClicked);
        settingsButton.onClick.AddListener(OnSettingsClicked);
        exitButton.onClick.AddListener(OnExitClicked);
    }

    private void OnEnable()
    {
        InputManager.OnPausePressed += TogglePause;
        paused = false;
        Time.timeScale = 1;
        PlayerAudio.PauseAllSounds(false);
    }

    private void OnDisable() 
    {
        InputManager.OnPausePressed -= TogglePause;
    }

    private void Awake()
    {
        Time.timeScale = 1;
    }

    void OnResumeClicked()
    {
        UnpauseGame();
        onResumeClicked.Invoke();
        
    }

    void OnSettingsClicked()
    {
        onSettingsClicked.Invoke();
    }

    void OnExitClicked()
    {
        onExitClicked.Invoke();
        
    }

    public void TogglePause()
    {
        if(paused)
            UnpauseGame();
        else
            PauseGame();
    }

    public void PauseGame()
    {
        // pause the game by setting timescale to 0 and pausing all sounds
        paused = true;
        Time.timeScale = 0;
        PlayerAudio.PauseAllSounds(true);
        resumeButton.Select();
        onPauseClicked.Invoke();
        OnPause.Invoke();
        Debug.Log("Game Paused");
    }

    public void UnpauseGame()
    {
        // unpause the game by setting timescale to 1 and resuming all sounds
        paused = false;
        Time.timeScale = 1;
        PlayerAudio.PauseAllSounds(false);
        onResumeClicked.Invoke();
        OnResume.Invoke();
        Debug.Log("Game Resumed");
    }
}
