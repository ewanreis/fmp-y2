using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class AchievementsMenu : MonoBehaviour
{
    public static event Action OnAchievementsMenuOpen;
    public static event Action<Achievement> OnAchievementUnlock;

    public static List<Achievement> achievementList;
    public UnityEvent OnClose;

    [SerializeField] private List<Achievement> _achievements;
    [SerializeField] private GameObject achievementsUI;
    [SerializeField] private TMP_Text description;

    private List<Achievement> achievementStatuses;
    private bool isAchievementsMenuOpen;

    [SerializeField] private GameObject displayPrefab;
    [SerializeField] private Transform displayParent;

    [SerializeField] private List<GameObject> displayedAchievements = new List<GameObject>();

    private IEnumerator Start()
    {
        achievementList = _achievements;

        achievementStatuses = new List<Achievement>();

        yield return new WaitForSeconds(0.5f);
        GetSavedAchievements();
        UpdateAchievementsMenuDisplay();
        RegisterAchievementUnlock(_achievements[0]);
        yield return null;
    }

    private void OnEnable() 
    {
        InputManager.OnAchievementsPressed += ToggleAchievementsMenu;
        Bestiary.OnBestiaryOpen += CloseAchievementsMenu;
    }

    private void OnDisable() 
    {
        InputManager.OnAchievementsPressed -= ToggleAchievementsMenu;
        Bestiary.OnBestiaryOpen -= CloseAchievementsMenu;
    }

    public void UpdateAchievementsMenuDisplay()
    {
        // clear old objects
        foreach(GameObject gameObject in displayedAchievements)
        {
            Destroy(gameObject);
        }
        displayedAchievements.Clear();
        ListButtonSelect(0);


        for(int i = 0; i < _achievements.Count; i++)
        {
            int index = i; // prevent passing the reference i into the parameter of select

            displayedAchievements.Add(Instantiate(displayPrefab, displayParent));

            TMP_Text displayText = displayedAchievements[i].GetComponentInChildren<TMP_Text>();
            Image creatureImage = displayedAchievements[i].GetComponent<Image>();
            Button button = displayedAchievements[i].GetComponent<Button>();
            button.interactable = _achievements[i].IsUnlocked;

            button.onClick.AddListener(() => ListButtonSelect(index));

            displayText.text = _achievements[i].Name;
            description.text = $"Achievement: {_achievements[index].Name}\nDescription: {_achievements[index].Description}";
        }
    }

    public void ListButtonSelect(int index)
    {
        description.text = $"Achievement: {_achievements[index].Name}\nDescription: {_achievements[index].Description}";
    }

    public void SaveAchievementStatus()
    {
        SaveData.SaveAchievements(achievementStatuses);
    }

    public void GetSavedAchievements()
    {
        AchievementsData.SavableAchievement[] savableAchievementsList = SaveData.GetAchievements();
        achievementStatuses = SaveData.ConvertSaveableToAchievement(savableAchievementsList);
        _achievements = achievementStatuses;
    }

    public static List<Achievement> GetAchievementList()
    {
        return achievementList;
    }


    public void ToggleAchievementsMenu()
    {
        isAchievementsMenuOpen = !isAchievementsMenuOpen;
        if(PauseMenu.paused)
            return;

        if(isAchievementsMenuOpen)
            OpenAchievementsMenu();

        else
            CloseAchievementsMenu();
    }

    public void OpenAchievementsMenu()
    {
        isAchievementsMenuOpen = true;
        UpdateAchievementsMenuDisplay();
        achievementsUI.SetActive(true);
        OnAchievementsMenuOpen.Invoke();
        Debug.Log("Open Achievements Menu");
    }

    public void CloseAchievementsMenu()
    {
        OnClose.Invoke();
        isAchievementsMenuOpen = false;
        achievementsUI.SetActive(false);
        Debug.Log("Close Achievements Menu");
    }

    public void RegisterAchievementUnlock(Achievement achievement)
    {
        if(achievement.IsUnlocked == true)
        {
            Debug.Log($"Achievement {achievement} is already unlocked");
            return;
        }
        achievement.IsUnlocked = true;
        SaveAchievementStatus();
        UpdateAchievementsMenuDisplay();
        Debug.Log($"Unlocked {achievement}");
        OnAchievementUnlock?.Invoke(achievement);
    }
}
