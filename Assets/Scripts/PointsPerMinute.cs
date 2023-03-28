using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PointsPerMinute : MonoBehaviour
{
    public int pointsToAdd = 10;
    public float timeBetweenPoints = 60f; 
    [SerializeField] private ScoreManager scoreManager;
    public static event Action OnGainPoints;

    void Start()
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
