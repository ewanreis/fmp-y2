using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ParticleSystem))]
public class SnowScript : MonoBehaviour
{
    //* Manages the snow and makes the amount of snow increase over time
    private ParticleSystem ps;
    [SerializeField] private float hSliderValue = 5.0f;
    [SerializeField] private float incrementRate = .01f;
    private float counter = 0;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    void FixedUpdate()
    {
        var emission = ps.emission;
        counter += incrementRate * Time.deltaTime;
        emission.rateOverTime = hSliderValue + counter;
    }
}