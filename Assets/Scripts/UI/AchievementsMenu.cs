using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

public class AchievementsMenu : MonoBehaviour
{
    [SerializeField] private List<Achievement> _achievements;
    [SerializeField] private GameObject achievementsUI;
    [SerializeField] private TMP_Text description;

    private Dictionary<Achievement, bool> achievementStatuses;


    public static event Action OnAchievementsMenuOpen;
    public static event Action<Achievement> OnAchievementUnlock;

    private bool isAchievementsMenuOpen;

    public GameObject displayPrefab;
    public Transform displayParent;

    public List<GameObject> displayedAchievements = new List<GameObject>();

    private void Start()
    {
        InputManager.OnAchievementsPressed += ToggleAchievementsMenu;
        Bestiary.OnBestiaryOpen += CloseAchievementsMenu;

        achievementStatuses = new Dictionary<Achievement, bool>();

        GetSavedAchievements();
        UpdateAchievementsMenuDisplay();
    }

    public void UpdateAchievementsMenuDisplay()
    {
        // clear old objects
        foreach(GameObject gameObject in displayedAchievements)
        {
            Destroy(gameObject);
        }
        displayedAchievements.Clear();


        for(int i = 0; i < _achievements.Count; i++)
        {
            int index = i; // prevent passing the reference i into the parameter of select

            displayedAchievements.Add(Instantiate(displayPrefab, displayParent));

            TMP_Text displayText = displayedAchievements[i].GetComponentInChildren<TMP_Text>();
            Image creatureImage = displayedAchievements[i].GetComponent<Image>();
            Button button = displayedAchievements[i].GetComponent<Button>();

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
        achievementStatuses = SaveData.GetAchievements();
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
        isAchievementsMenuOpen = false;
        achievementsUI.SetActive(false);
        Debug.Log("Close Achievements Menu");
    }

    public void RegisterAchievementUnlock(Achievement achievement)
    {
        achievement.IsUnlocked = true;
        achievementStatuses[achievement] = true;
        SaveAchievementStatus();
        UpdateAchievementsMenuDisplay();
        OnAchievementUnlock?.Invoke(achievement);
    }
}
