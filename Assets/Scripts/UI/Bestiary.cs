using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class Bestiary : MonoBehaviour
{
    //* Used for managing displayed creatures in the bestiary
    public static event Action<Creature> OnCreatureEncountered;
    public static event Action<Creature> OnCreatureKilled;
    public static event Action OnBestiaryOpen;

    public UnityEvent OnClose;
    public UnityEvent OnOpen;

    public List<GameObject> displayedCreatures = new List<GameObject>();

    [SerializeField] private List<Creature> _creatures;
    [SerializeField] private GameObject bestiaryUI;
    [SerializeField] private TMP_Text description;
    [SerializeField] private GameObject displayPrefab;
    [SerializeField] private Transform displayParent;
    [SerializeField] private Image displayImage;

    private bool isBestiaryOpen;

    private void Start()
    {
        UpdateBestiaryDisplay();
    }

    private void OnEnable() 
    {
        InputManager.OnBestiaryPressed += ToggleBestiary;
        AchievementsMenu.OnAchievementsMenuOpen += CloseBestiary;
        Health.OnDeath += ManageEnemyDeath;
    }

    private void OnDisable() 
    {
        InputManager.OnBestiaryPressed -= ToggleBestiary;
        AchievementsMenu.OnAchievementsMenuOpen -= CloseBestiary;
        Health.OnDeath -= ManageEnemyDeath;
    }

    private void ManageEnemyDeath(Creature creature, bool isEnemy)
    {
        if(!isEnemy)
            return;
        
        RegisterCreatureKill(creature);
    }

    public void UpdateBestiaryDisplay()
    {
        // clear old objects
        foreach(GameObject gameObject in displayedCreatures)
        {
            Destroy(gameObject);
        }
        displayedCreatures.Clear();


        for (int i = 0; i < _creatures.Count; i++)
        {
            int index = i; // prevent passing the reference i into the parameter of select

            displayedCreatures.Add(Instantiate(displayPrefab, displayParent));

            TMP_Text displayText = displayedCreatures[i].GetComponentInChildren<TMP_Text>();
            Image creatureImage = displayedCreatures[i].GetComponent<Image>();
            Button button = displayedCreatures[i].GetComponent<Button>();

            button.onClick.AddListener(() => ListButtonSelect(index));

            displayText.text = _creatures[i].Name;
            description.text = 
                $"Creature Name: {_creatures[i].Name}\nTimes Slain: {_creatures[i].CreatureKills}";
        }
    }

    public void ListButtonSelect(int index)
    {
        description.text = 
            $"Creature Name: {_creatures[index].Name}\nTimes Slain: {_creatures[index].CreatureKills}";
        displayImage.sprite = _creatures[index].Sprite;
    }

    public void ToggleBestiary()
    {
        isBestiaryOpen = !isBestiaryOpen;
        if(PauseMenu.paused)
            return;
        UpdateBestiaryDisplay();

        if(isBestiaryOpen)
            OpenBestiary();

        else
            CloseBestiary();
    }

    public void OpenBestiary()
    {
        isBestiaryOpen = true;
        bestiaryUI.SetActive(true);
        OnOpen.Invoke();
        OnBestiaryOpen.Invoke();
        Debug.Log("Open Bestiary");
    }

    public void CloseBestiary()
    {
        OnClose.Invoke();
        isBestiaryOpen = false;
        bestiaryUI.SetActive(false);
        Debug.Log("Close Bestiary");
    }

    public void RegisterCreatureEncounter(Creature creature)
    {
        creature.CreatureEncounters++;
        OnCreatureEncountered?.Invoke(creature);
    }

    public void RegisterCreatureKill(Creature creature)
    {
        creature.CreatureKills++;
        OnCreatureKilled?.Invoke(creature);
    }

    public bool HasEncountered(Creature creature)
    {
        // check if the player has encountered the given creature
        return (creature.CreatureEncounters > 0) ? true : false;
    }

    public bool HasSlain(Creature creature)
    {
        // check if the player has slain the given creature
        return (creature.CreatureKills > 0) ? true : false;
    }

    public int GetEncounterCount(Creature creature)
    {
        // get the number of times the player has encountered the given creature
        return creature.CreatureEncounters;
    }

    public int GetKillCount(Creature creature)
    {
        // get the number of times the player has slain the given creature
        return creature.CreatureKills;
    }

    public bool CanUnlockKnowledge(Creature creature)
    {
        // check if the player has slain enough of the given creature to unlock knowledge
        return (creature.CreatureKills >= creature.KillsToUnlock) ? true : false;
    }

    public void UnlockKnowledge(Creature creature)
    {
        // unlock knowledge for the given creature
        creature.IsUnlocked = true;
    }
}