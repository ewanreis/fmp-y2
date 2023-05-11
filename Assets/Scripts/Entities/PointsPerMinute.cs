using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PointsPerMinute : MonoBehaviour
{
    //* This script handles the points given to the player every minute (or any other interval)
    public static event Action OnGainPoints;

    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private float timeBetweenPoints = 60f; 
    [SerializeField] private int pointsToAdd = 10;

    void OnEnable()
    {
        Invoke("IncremementPoints", timeBetweenPoints);
    }

    private void IncremementPoints()
    {
        scoreManager.AddPoints(pointsToAdd);
        OnGainPoints.Invoke();
        Invoke("IncremementPoints", timeBetweenPoints);
    }
}
