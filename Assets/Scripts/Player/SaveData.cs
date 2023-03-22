using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class SaveData
{
    public static AudioData audio;
    public static StatisticData statistics;
    public static AchievementsData achievements;
    public static GraphicsData graphics;

    private const string audioKey = "audio";
    private const string statisticsKey = "statistics";
    private const string achievementsKey = "achievements";
    private const string graphicsKey = "graphics";

    // * this converts the stored structs into JSON files, which can then be saved to player prefs
    // * the JSON file can be turned back into a struct, if one is found from player prefs

    public static void Setup()
    {
        // ensure volume array is correct size
        audio.volumes = new float[System.Enum.GetNames(typeof(AudioChannel)).Length];

        // load saved data
        LoadAudioData();
        LoadStatisticsData();
        LoadAchievementsData();
        LoadGraphicsData();
        Save();
    }

    public static void Save()
    {
        // save audio data
        string audioJson = JsonUtility.ToJson(audio);
        PlayerPrefs.SetString(audioKey, audioJson);

        // save statistics data
        string statisticsJson = JsonUtility.ToJson(statistics);
        PlayerPrefs.SetString(statisticsKey, statisticsJson);

        // save achievements data
        string achievementsJson = JsonUtility.ToJson(achievements);
        PlayerPrefs.SetString(achievementsKey, achievementsJson);

        // save graphics data
        string graphicsJson = JsonUtility.ToJson(graphics);
        PlayerPrefs.SetString(graphicsKey, graphicsJson);

        Debug.Log($"Saved Data \nAudio: {audioJson}\nStatistics: {statisticsJson}\nAchievements: {achievementsJson}\nGraphics{graphicsJson}");

        // save changes to player prefs
        PlayerPrefs.Save();
    }

    public static void SaveAchievements(Dictionary<Achievement, bool> achievementData)
    {
        achievements.achievementList = achievementData;
        Save();
    }

    public static void SaveVolume(AudioChannel channel, float volume)
    {
        audio.volumes[(int)channel] = volume;
        Save();
    }

    public static void SaveGraphics(GraphicsData data)
    {
        graphics = data;
        Save();
    }

    private static void LoadAudioData()
    {
        if (PlayerPrefs.HasKey(audioKey))
            audio = JsonUtility.FromJson<AudioData>(PlayerPrefs.GetString(audioKey));

        else
        {
            audio = new AudioData(); // initialize default audio data
            audio.volumes = new float[System.Enum.GetNames(typeof(AudioChannel)).Length];

            for(int i = 0; i < audio.volumes.Length; i++)
                audio.volumes[i] = 1f;
        }
    }

    private static void LoadStatisticsData()
    {
        if (PlayerPrefs.HasKey(statisticsKey))
            statistics = JsonUtility.FromJson<StatisticData>(PlayerPrefs.GetString(statisticsKey));

        else
            statistics = new StatisticData(); // initialize default statistics data
    }

    private static void LoadAchievementsData()
    {
        if (PlayerPrefs.HasKey(achievementsKey))
            achievements = JsonUtility.FromJson<AchievementsData>(PlayerPrefs.GetString(achievementsKey));

        else
            achievements = new AchievementsData(); // initialize default statistics data
    }

    private static void LoadGraphicsData()
    {
        if (PlayerPrefs.HasKey(graphicsKey))
            graphics = JsonUtility.FromJson<GraphicsData>(PlayerPrefs.GetString(graphicsKey));

        else
            graphics = new GraphicsData(); // initialize default graphics data
    }

    public static float[] GetSavedAudioVolumes() => audio.volumes;
    public static Dictionary<Achievement, bool> GetAchievements() => achievements.achievementList;
    public static GraphicsData GetGraphicsData() => graphics;
}

public struct AudioData
{
    public float[] volumes;
}

public struct StatisticData
{
    public int totalEnemiesKilled;
    public int highestYearReached;
    public Dictionary<Creature, bool> creatures;
}

public struct AchievementsData
{
    public Dictionary<Achievement, bool> achievementList;
}

public struct GraphicsData
{
    public bool isFullscreen;
    public bool isVsync;
}