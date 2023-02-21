using System.Collections.Generic;
using UnityEngine;
using System;

public class Bestiary : MonoBehaviour
{
    [SerializeField] private List<Creature> _creatures;

    public event Action<Creature> OnCreatureEncountered;
    public event Action<Creature> OnCreatureKilled;

    private bool isBestiaryOpen;

    private void Start()
    {
        InputManager.OnBestiaryPressed += ToggleBestiary;
    }

    public void ToggleBestiary()
    {
        isBestiaryOpen = !isBestiaryOpen;

        if(isBestiaryOpen)
            OpenBestiary();

        else
            CloseBestiary();
    }

    public void OpenBestiary()
    {
        Debug.Log("Open Bestiary");
    }

    public void CloseBestiary()
    {
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