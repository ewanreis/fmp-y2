using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PointGainText : MonoBehaviour
{
    public GameObject textBoxPrefab;
    public float fadeInTime = 0.5f;
    public float displayTime = 2.0f;
    public float fadeOutTime = 0.5f;
    public float maxDistance = 1.5f;

    private GameObject textBox;
    private RectTransform canvasRect;
    //private bool isActive;

    private void Start()
    {
        ScoreManager.OnPointsGained += SpawnText;
    }

    private void SpawnText(int points)
    {
        StartCoroutine(SpawnTextCoroutine(points));
    }

    private IEnumerator SpawnTextCoroutine(int pointsGained)
    {
        //isActive = true;
        canvasRect = FindObjectOfType<Canvas>().GetComponent<RectTransform>();

        textBox = Instantiate(textBoxPrefab, canvasRect.transform);
        textBox.SetActive(false);
        textBox.GetComponent<TMP_Text>().text = $"+{pointsGained}";

        float angle = Random.Range(0f, 2f * Mathf.PI);
        float distance = Random.Range(0f, maxDistance);
        Vector3 offset = new Vector3(Mathf.Cos(angle) * distance, Mathf.Sin(angle) * distance, 0f);
        Vector3 position = transform.position + offset;
        Vector3 screenPos = Camera.main.WorldToScreenPoint(position);

        textBox.GetComponent<RectTransform>().position = screenPos;
        Destroy(textBox, fadeOutTime + fadeInTime + displayTime + 1f);

        yield return StartCoroutine(FadeIn());
        yield return new WaitForSeconds(displayTime);
        yield return StartCoroutine(FadeOut());

    }

    IEnumerator FadeIn()
    {
        float timer = 0f;
        while (timer <= fadeInTime)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, timer / fadeInTime);
            Color color = textBox.GetComponent<TMP_Text>().color;
            color.a = alpha;
            textBox.GetComponent<TMP_Text>().color = color;
            textBox.SetActive(true);
            yield return null;
        }
    }

    IEnumerator FadeOut()
    {
        float timer = 0f;
        
        while (timer < fadeOutTime)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, timer / fadeOutTime);
            Color color = textBox.GetComponent<TMP_Text>().color;
            color.a = alpha;
            textBox.GetComponent<TMP_Text>().color = color;
            yield return null;
        }
        
        //isActive = false;
    }
}
