using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
public class HeartManager : MonoBehaviour
{
    public Sprite[] heartSprites;
    public Image[] hearts;
    public int healthPerHeart = 10;
    public int numHearts = 3;
    public TMP_Text healthText;

    private int currentHealth;

    private void Start()
    {
        currentHealth = healthPerHeart * numHearts;
        PlayerHealth.OnUpdateHealth += UpdateHealth;
        UpdateHeartDisplay();
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