using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AchievementManager
{
    public delegate void AchievementUnlocked(string achievementName);
    public static event AchievementUnlocked OnAchievementUnlocked;

    // dictionary of achievements and unlock status
    private static Dictionary<string, bool> achievements = new Dictionary<string, bool>();

    private static void Start()
    {
        // initialize achievements
        achievements.Add("First Game", false);
        achievements.Add("Test Achievement", false);

        OnAchievementUnlocked += AchievementUnlockedHandler;
    }

    // handler method
    private static void AchievementUnlockedHandler(string achievementName)
    {
        Debug.Log("Achievement Unlocked: " + achievementName);

        // update status of the achievement
        achievements[achievementName] = true;
    }

    // pass achievement name to unlock
    public static void UnlockAchievement(string achievementName)
    {
        // check if the achievement has already been unlocked
        if (achievements[achievementName])
        {
            Debug.Log("Achievement already unlocked: " + achievementName);
            return;
        }

        // invoke event
        OnAchievementUnlocked?.Invoke(achievementName);
    }
}
