using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public Bestiary bestiary;
    public AchievementsMenu achievementsMenu;

    private void Start()
    {
        Bestiary.OnBestiaryOpen += ManageBestiaryOpen;
    }

    private void ManageBestiaryOpen()
    {

    }
}
