using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class YearStartText : MonoBehaviour
{
    [SerializeField] private float fadeTime = 2f;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private string textToDisplay;

    private TMP_Text displayText;
    private int currentWave;

    void Start()
    {
        displayText = GetComponent<TMP_Text>();
        displayText.text = "";
        DisplayText(1);
        EnemySpawner.OnWaveStart += DisplayText;
    }

    private void DisplayText(int waveNumber)
    {
        Debug.Log("Display");
        currentWave = waveNumber;
        StartCoroutine(FadeOutText());
    }

    IEnumerator FadeOutText()
    {
        Vector3 textPos = new Vector3(playerTransform.position.x, playerTransform.position.y + 10, 20);
        displayText.color = Color.white;
        transform.position = textPos;
        displayText.text = $"Year {currentWave}";
        yield return new WaitForSeconds(2f);

        float t = 0f;
        while (t < fadeTime)
        {
            t += Time.deltaTime;
            displayText.color = new Color(displayText.color.r, displayText.color.g, displayText.color.b, Mathf.Lerp(1f, 0f, t / fadeTime));
            yield return null;
        }

        displayText.text = "";
    }
}
