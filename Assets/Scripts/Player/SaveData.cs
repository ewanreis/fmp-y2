using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class SaveData
{
    public static AudioData audio;
    public static StatisticData statistics;
    public static AchievementsData achievements;

    private const string audioKey = "audio";
    private const string statisticsKey = "statistics";
    private const string achievementsKey = "achievements";

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

        //Debug.Log($"Saved Data \nAudio: {audioJson}\nStatistics: {statisticsJson}\nAchievements: {achievementsJson}");

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

    public static float[] GetSavedAudioVolumes() => audio.volumes;
    public static Dictionary<Achievement, bool> GetAchievements() => achievements.achievementList;
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