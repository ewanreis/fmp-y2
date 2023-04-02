using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ScoreManager : MonoBehaviour
{
    private int currentPoints;
    [SerializeField] private int startingPoints;

    public static event Action<int> OnPointsGained;

    private void Start()
    {
        currentPoints = startingPoints;
    }

    public void AddPoints(int pointsToAdd)
    {
        currentPoints += pointsToAdd;
        OnPointsGained.Invoke(pointsToAdd);
    }

    public void SubtractPoints(int pointsToSubtract)
    {
        if((currentPoints - pointsToSubtract) >= 0)
            currentPoints -= pointsToSubtract;
        else
            Debug.Log("Tried to subtract negative point value");
    }

    public int GetCurrentPoints() => currentPoints;
}