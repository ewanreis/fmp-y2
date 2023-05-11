using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisonLight : MonoBehaviour
{
    //* This script sets the vision light cone to the sight range of the entity
    private UnityEngine.Rendering.Universal.Light2D visionLight;
    [SerializeField] private float visionRange;
    [SerializeField] private Transform target;
    [SerializeField] private Transform lightParent;

    private void Start()
    {
        visionLight = GetComponent<UnityEngine.Rendering.Universal.Light2D>();
        visionLight.pointLightOuterRadius = visionRange;
    }
}
