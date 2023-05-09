using System.Collections;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color flashColor = Color.red;
    [SerializeField] private float flashDuration = 0.1f;
    
    private Color originalColor;
    private bool isFlashing = false;
    
    private void Start()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    public void Flash()
    {
        StartCoroutine(FlashColorCoroutine());
    }

    IEnumerator FlashColorCoroutine()
    {
        isFlashing = true;
        float timeElapsed = 0f;
        while (timeElapsed < flashDuration)
        {
            spriteRenderer.color = Color.Lerp(originalColor, flashColor, timeElapsed / flashDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        spriteRenderer.color = originalColor;
        isFlashing = false;
    }
}
