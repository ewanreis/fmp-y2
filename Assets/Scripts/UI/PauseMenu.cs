using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private bool paused = false;

    private void Start()
    {
        InputManager.OnPausePressed += TogglePause;
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
        paused = true;
        Time.timeScale = 0;
        PlayerAudio.PauseAllSounds(true);
        Debug.Log("Game Paused");
    }

    public void UnpauseGame()
    {
        paused = false;
        Time.timeScale = 1;
        PlayerAudio.PauseAllSounds(false);
        Debug.Log("Game Resumed");
    }
}
