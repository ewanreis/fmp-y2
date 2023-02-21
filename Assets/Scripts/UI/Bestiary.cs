using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class Bestiary : MonoBehaviour
{
    [SerializeField] private List<Creature> _creatures;
    [SerializeField] private GameObject bestiaryUI;

    public event Action<Creature> OnCreatureEncountered;
    public event Action<Creature> OnCreatureKilled;

    private bool isBestiaryOpen;

    public GameObject displayPrefab;
    public Transform displayParent;

    public List<GameObject> displayedCreatures = new List<GameObject>();

    private void Start()
    {
        InputManager.OnBestiaryPressed += ToggleBestiary;
        UpdateBestiaryDisplay();
    }

    public void UpdateBestiaryDisplay()
    {
        // clear old objects
        foreach(GameObject gameObject in displayedCreatures)
        {
            Destroy(gameObject);
        }
        displayedCreatures.Clear();


        for(int i = 0; i < _creatures.Count; i++)
        {
            displayedCreatures.Add(Instantiate(displayPrefab, displayParent));
            TMP_Text displayText = displayedCreatures[i].GetComponentInChildren<TMP_Text>();
            Image creatureImage = displayedCreatures[i].GetComponent<Image>();
            displayText.text = _creatures[i].Name;
        }
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
        Debug.Log("Open Bestiary");
    }

    public void CloseBestiary()
    {
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