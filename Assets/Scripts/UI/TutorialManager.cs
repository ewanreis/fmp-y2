using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TutorialManager : MonoBehaviour
{
    //* Manages the opening tutorial on game start

    [SerializeField] private float sequenceDuration = 5f;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private TMP_Text tutorialText;

    private List<string> tutorialSequences = new List<string>();
    private int currentSequenceIndex = 0;
    private Coroutine sequenceCoroutine;

    private void Start()
    {
        tutorialSequences.Add("Mount the throne to shoot");
        tutorialSequences.Add("Buy soldiers through the shop");
        tutorialSequences.Add("Stop enemies from capturing the castle");
        tutorialSequences.Add("Kill the enemies to progress to the next year");

        StartTutorial();
    }

    private void StartTutorial()
    {
        currentSequenceIndex = 0;
        ShowTutorialText();
        sequenceCoroutine = StartCoroutine(PlayTutorialSequence());
    }

    private void ShowTutorialText()
    {
        tutorialText.text = tutorialSequences[currentSequenceIndex];
        StartCoroutine(FadeText(0f, 1f, fadeDuration));
    }

    private IEnumerator PlayTutorialSequence()
    {
        yield return new WaitForSeconds(sequenceDuration);

        StartCoroutine(FadeText(1f, 0f, fadeDuration));
        currentSequenceIndex++;

        if (currentSequenceIndex < tutorialSequences.Count)
        {
            yield return new WaitForSeconds(fadeDuration);
            ShowTutorialText();
            sequenceCoroutine = StartCoroutine(PlayTutorialSequence());
        }
        else
        {
            sequenceCoroutine = null;
            Debug.Log("Tutorial Complete");
        }
    }

    private IEnumerator FadeText(float startAlpha, float targetAlpha, float duration)
    {
        Color startColor = tutorialText.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, targetAlpha);
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            tutorialText.color = Color.Lerp(startColor, targetColor, elapsedTime / duration);
            yield return null;
        }

        tutorialText.color = targetColor;
    }
}
