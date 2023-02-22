using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AchievementsMenu : MonoBehaviour
{
    public static event Action OnAchievementsMenuOpen;
    private bool isAchievementsMenuOpen;

    private void Start()
    {
        InputManager.OnAchievementsPressed += ToggleAchievementsMenu;
        Bestiary.OnBestiaryOpen += CloseAchievementsMenu;
    }

    public void ToggleAchievementsMenu()
    {
        if(PauseMenu.paused)
            return;
        UpdateAchievementsDisplay();

        if(isAchievementsMenuOpen)
            CloseAchievementsMenu();

        else
            OpenAchievementsMenu();
    }

    public void OpenAchievementsMenu()
    {
        Debug.Log("Open Achievements Menu");
        OnAchievementsMenuOpen.Invoke();
    }

    public void CloseAchievementsMenu()
    {
        Debug.Log("Close Achievements Menu");
    }

    public void UpdateAchievementsDisplay()
    {

    }
}
