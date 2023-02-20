using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class SaveData
{
    public static AudioData audio;
    public static StatisticData statistics;

    private static string audioKey = "audio";
    private static string statisticsKey = "statistics";

    // This converts the stored structs into JSON files, which can then be saved to player prefs.
    // The JSON file can be turned back into a struct, if one is found from player prefs.

    public static void Setup()
    {
        // Ensure volume array is correct size.
        Array.Resize(ref audio.volumes, System.Enum.GetNames(typeof(AudioChannel)).Length);

        // Load saved data from PlayerPrefs.
        LoadAudioData();
        LoadStatisticsData();
    }

    public static void Save()
    {
        // Save audio data.
        string audioJson = JsonUtility.ToJson(audio);
        PlayerPrefs.SetString(audioKey, audioJson);

        // Save statistics data.
        string statisticsJson = JsonUtility.ToJson(statistics);
        PlayerPrefs.SetString(statisticsKey, statisticsJson);

        // Save changes to PlayerPrefs.
        PlayerPrefs.Save();
    }

    public static void SaveVolume(AudioChannel channel, int volume)
    {
        audio.volumes[(int)channel] = volume;
        Save();
    }

    private static void LoadAudioData()
    {
        if (PlayerPrefs.HasKey(audioKey))
            audio = JsonUtility.FromJson<AudioData>(PlayerPrefs.GetString(audioKey));

        else
            audio = new AudioData(); // Initialize default audio data.
    }

    private static void LoadStatisticsData()
    {
        if (PlayerPrefs.HasKey(statisticsKey))
            statistics = JsonUtility.FromJson<StatisticData>(PlayerPrefs.GetString(statisticsKey));

        else
            statistics = new StatisticData(); // Initialize default statistics data.
    }
}

public struct AudioData
{
    public int[] volumes;
}

public struct StatisticData
{
    public int totalEnemiesKilled;
    public int highestYearReached;
}