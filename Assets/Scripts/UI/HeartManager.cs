using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
public class HeartManager : MonoBehaviour
{
    [SerializeField] private Sprite[] heartSprites;
    [SerializeField] private Image[] hearts;
    [SerializeField] private int healthPerHeart = 10;
    [SerializeField] private int numHearts = 3;
    [SerializeField] private TMP_Text healthText;

    private int currentHealth;

    private void Start()
    {
        currentHealth = healthPerHeart * numHearts;
        UpdateHeartDisplay();
    }

    void OnEnable()
    {
        PlayerHealth.OnUpdateHealth += UpdateHealth;
    }

    void OnDisable()
    {
        PlayerHealth.OnUpdateHealth -= UpdateHealth;
    }

    // safely update health to avoid weird values
    public void UpdateHealth(int health)
    {
        currentHealth = health;

        if (currentHealth < 0)
            currentHealth = 0;

        if (currentHealth > healthPerHeart * numHearts)
            currentHealth = healthPerHeart * numHearts;

        UpdateHeartDisplay();
    }

    // update heart sprite based on 10 points per heart
    private void UpdateHeartDisplay()
    {
        healthText.text = $"{currentHealth}";
        for (int i = 0; i < numHearts; i++)
        {
            int healthIndexMin = i * healthPerHeart;
            int healthIndexMax = (i + 1) * healthPerHeart;
            int indexStage = Mathf.Clamp(currentHealth, healthIndexMin, healthIndexMax) - healthIndexMin;
            hearts[i].sprite = heartSprites[indexStage];
        }
    }
}