using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class DeathScreen : MonoBehaviour
{
    [SerializeField] private GameObject deathScreen;
    [SerializeField] private Image panel1;
    [SerializeField] private Image panel2;
    [SerializeField] private TMP_Text deathText;
    [SerializeField] private float fadeSpeed = 1f;
    [SerializeField] private SceneLoader sceneLoader;

    private void OnEnable()
    {
        SetAlpha(deathScreen, 0f);
        PlayerHealth.OnDeath += PlayerDied;
    }

    void OnDisable()
    {
        PlayerHealth.OnDeath -= PlayerDied;
    }

    public void PlayerDied()
    {
        deathScreen.SetActive(true);
        StartCoroutine(FadeInDeathScreen());
    }

    private System.Collections.IEnumerator FadeInDeathScreen()
    {
        while (GetAlpha(panel1) < 1f || GetAlpha(panel2) < 1f || GetAlpha(deathText) < 1f)
        {
            float newAlpha = GetAlpha(panel1) + fadeSpeed * Time.deltaTime;
            SetAlpha(panel1, newAlpha);

            newAlpha = GetAlpha(panel2) + fadeSpeed * Time.deltaTime;
            SetAlpha(panel2, newAlpha);

            newAlpha = GetAlpha(deathText) + fadeSpeed * Time.deltaTime;
            SetAlpha(deathText, newAlpha);

            yield return null;
        }
        yield return new WaitForSeconds(3);
        sceneLoader.LoadScene("mainmenu");
    }

    private void SetAlpha(TMP_Text deathText, float newAlpha)
    {
        SetAlpha(deathText.gameObject, newAlpha);
    }

    private void SetAlpha(Image image, float newAlpha)
    {
        SetAlpha(image.gameObject, newAlpha);
    }

    private float GetAlpha(Graphic graphic)
    {
        return graphic.color.a;
    }

    private void SetAlpha(GameObject obj, float alpha)
    {
        Graphic[] graphics = obj.GetComponentsInChildren<Graphic>();
        foreach (Graphic graphic in graphics)
        {
            Color color = graphic.color;
            color.a = alpha;
            graphic.color = color;
        }
    }
}
