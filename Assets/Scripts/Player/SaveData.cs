using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
    // * scriptable objects such as the achievements have to be converted to a class or a struct before being saved as a json file

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

        //Debug.Log($"Saved Data \nAudio: {audioJson}\nStatistics: {statisticsJson}\nAchievements: {achievementsJson}\nGraphics{graphicsJson}");
        //Debug.Log($"{achievementsJson}, {achievements.saveableAchievementList.Length}");

        // save changes to player prefs
        PlayerPrefs.Save();
    }

    public static void SaveAchievements(List<Achievement> achievementData)
    {
        achievements.saveableAchievementList = ConvertAchievementToSaveable(achievementData);
        Debug.Log("Saved");
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
        {
            achievements = JsonUtility.FromJson<AchievementsData>(PlayerPrefs.GetString(achievementsKey));
        }

        else
        {
            Debug.Log("Created New Achievement Data");
            achievements = new AchievementsData(); // initialize default statistics data
            List<Achievement> achList = AchievementsMenu.achievementList; // get all known achievements
            //Debug.Log($"achList {achList}");
            achievements.saveableAchievementList = ConvertAchievementToSaveable(achList);
            for (int i = 0; i < achievements.saveableAchievementList.Count(); i++)
            {
                achievements.saveableAchievementList[i]._isUnlocked = false;
            }
            
            foreach(AchievementsData.SavableAchievement savableAchievement in achievements.saveableAchievementList)
            {
                Debug.Log($"Saveable Achievement {savableAchievement}");
            }
            Save();
            //Debug.Log(achievements.achievementList);
        }
    }

    public static AchievementsData.SavableAchievement[] ConvertAchievementToSaveable(List<Achievement> achList)
    {
        AchievementsData.SavableAchievement[] sAchList = new AchievementsData.SavableAchievement[achList.Count()];
        for(int i = 0; i < achList.Count(); i++)
        {
            AchievementsData.SavableAchievement savableAchievement = new AchievementsData.SavableAchievement
            (
                achList[i]._id,
                achList[i]._name,
                achList[i]._description,
                achList[i]._isUnlocked
            );
            sAchList[i] = savableAchievement;
        }
        Debug.Log(sAchList.Length);
        return sAchList;
    }

    public static List<Achievement> ConvertSaveableToAchievement(AchievementsData.SavableAchievement[] sAchList)
    {
        List<Achievement> achList = new List<Achievement>();
        
        for(int i = 0; i < sAchList.Length; i++)
        {
            Achievement achievement = (Achievement)ScriptableObject.CreateInstance("Achievement");
            achievement._id = sAchList[i]._id;
            achievement._name = sAchList[i]._name;
            achievement._description= sAchList[i]._description;
            achievement._isUnlocked = sAchList[i]._isUnlocked;

            achList.Add(achievement);
        }
        return achList;
    }

    private static void LoadGraphicsData()
    {
        if (PlayerPrefs.HasKey(graphicsKey))
            graphics = JsonUtility.FromJson<GraphicsData>(PlayerPrefs.GetString(graphicsKey));

        else
            graphics = new GraphicsData(); // initialize default graphics data
    }

    public static float[] GetSavedAudioVolumes() => audio.volumes;
    public static AchievementsData.SavableAchievement[] GetAchievements() 
    {
        return achievements.saveableAchievementList;
    }
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
    //public Dictionary<Creature, bool> creatures;
}

public struct AchievementsData
{
    public SavableAchievement[] saveableAchievementList;

    [System.Serializable]
    public struct SavableAchievement
    {
        public int _id;
        public string _name;
        public string _description;
        public bool _isUnlocked;
        public SavableAchievement(int id, string name, string description, bool isUnlocked)
        {
            _id = id;
            _name = name;
            _description = description;
            _isUnlocked = isUnlocked;
        }
    }
}

public struct GraphicsData
{
    public bool isFullscreen;
    public bool isVsync;
}