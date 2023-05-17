using UnityEngine;
using TMPro;
using System.Collections;

public class PointGainText : MonoBehaviour
{
    //* Manages the text for whenever the player gains score
    [SerializeField] private GameObject textBoxPrefab;
    [SerializeField] private float fadeInTime = 0.5f;
    [SerializeField] private float displayTime = 2.0f;
    [SerializeField] private float fadeOutTime = 0.5f;
    [SerializeField] private float maxDistance = 1.5f;

    private ScoreManager scoreManager;
    private GameObject textBox;

    private void OnEnable()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
        ScoreManager.OnPointsGained += SpawnText;
    }

    private void OnDisable()
    {
        ScoreManager.OnPointsGained -= SpawnText;
    }

    private void SpawnText(int points)
    {
        StartCoroutine(SpawnTextCoroutine(points));
    }

    private IEnumerator SpawnTextCoroutine(int pointsGained)
    {
        var canvas = FindObjectOfType<Canvas>();
        var canvasRect = canvas.GetComponent<RectTransform>();

        textBox = Instantiate(textBoxPrefab, canvasRect.transform);
        textBox.transform.SetAsFirstSibling();
        textBox.SetActive(false);
        textBox.GetComponent<TMP_Text>().text = $"+{pointsGained}";

        Vector3 randomPos = Random.insideUnitCircle * maxDistance;
        Vector3 position = transform.position + new Vector3(randomPos.x, randomPos.y, 0f);
        var screenPos = Camera.main.WorldToScreenPoint(position);

        textBox.GetComponent<RectTransform>().position = screenPos;

        Destroy(textBox, fadeOutTime + fadeInTime + displayTime + 2f);
        yield return StartCoroutine(FadeIn(textBox));
        yield return new WaitForSeconds(displayTime);
        yield return StartCoroutine(FadeOut(textBox));

    }

    private IEnumerator FadeIn(GameObject fadeObject)
    {
        var textComponent = fadeObject.GetComponent<TMP_Text>();
        var color = textComponent.color;

        for (float timer = 0f; timer <= fadeInTime; timer += Time.deltaTime)
        {
            color.a = Mathf.Lerp(0f, 1f, timer / fadeInTime);
            textComponent.color = color;
            fadeObject.SetActive(true);
            yield return null;
        }
    }

    private IEnumerator FadeOut(GameObject fadeObject)
    {
        var textComponent = fadeObject.GetComponent<TMP_Text>();
        var color = textComponent.color;

        for (float timer = 0f; timer < fadeOutTime; timer += Time.deltaTime)
        {
            color.a = Mathf.Lerp(1f, 0f, timer / fadeOutTime);
            textComponent.color = color;
            yield return null;
        }
    }
}
