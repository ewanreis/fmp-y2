using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PauseMenu : MonoBehaviour
{
    // keep track of whether the game is paused
    private bool paused = false;

    // events for each button
    public UnityEvent onResumeClicked;
    public UnityEvent onSettingsClicked;
    public UnityEvent onExitClicked;
    public UnityEvent onPauseClicked;

    // button references
    public Button resumeButton;
    public Button settingsButton;
    public Button exitButton;

    void Start()
    {
        // add a listener for the pause button event
        InputManager.OnPausePressed += TogglePause;

        // add listeners to the buttons
        resumeButton.onClick.AddListener(OnResumeClicked);
        settingsButton.onClick.AddListener(OnSettingsClicked);
        exitButton.onClick.AddListener(OnExitClicked);
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
        onPauseClicked.Invoke();
        Debug.Log("Game Paused");
    }

    public void UnpauseGame()
    {
        // unpause the game by setting timescale to 1 and resuming all sounds
        paused = false;
        Time.timeScale = 1;
        PlayerAudio.PauseAllSounds(false);
        onResumeClicked.Invoke();
        Debug.Log("Game Resumed");
    }
}
