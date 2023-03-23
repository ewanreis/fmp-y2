using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour
{
    public int currentHealth;
    public SoldierType soldierType;
    public Statistics soldierStats;
}

public enum SoldierType
{
    Infantry,
    Scout,
    Commander
}

[System.Serializable]
public struct Statistics
{
    public int strength;
    public int defence;
    public int speed;
    public int vision;
    public int stealth;
}