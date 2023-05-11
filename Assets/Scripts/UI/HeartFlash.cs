using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartFlash : MonoBehaviour
{
    //* Flashes the attached sprite when the player takes damage
    private Color originalColour;
    private Image heartSprite;
    [SerializeField] private Color flashColour;
    [SerializeField] private float flashTime;

    void Start()
    {
        heartSprite = GetComponent<Image>();
        originalColour = heartSprite.color;
    }

    private void OnEnable() 
    {
        PlayerHealth.OnUpdateHealth += FlashHeart;
    }

    private void OnDisable() 
    {
        PlayerHealth.OnUpdateHealth -= FlashHeart;
    }

    private void FlashHeart(int health)
    {
        StartCoroutine(FlashCoroutine(flashColour));
    }

    private IEnumerator FlashCoroutine(Color targetColour)
    {
        float elapsedTime = 0f;
        Color startColor = heartSprite.color;

        while (elapsedTime < flashTime)
        {
            heartSprite.color = Color.Lerp(startColor, targetColour, elapsedTime / flashTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        heartSprite.color = flashColour;
        StartCoroutine(ReturnToDefaultColour());
    }

    private IEnumerator ReturnToDefaultColour()
    {
        float elapsedTime = 0f;
        Color startColor = heartSprite.color;

        while (elapsedTime < flashTime)
        {
            heartSprite.color = Color.Lerp(startColor, originalColour, elapsedTime / flashTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        heartSprite.color = originalColour;
    }
}
