using System.Collections;
using UnityEngine;
using TMPro;

public class AchievementNotification : MonoBehaviour
{
    //* Manages the notification for gaining achievements
    [SerializeField] private float lerpSpeed = 2;
    [SerializeField] private float displayTime = 2;
    private float currentTime = 0;
    private float normalizedValue;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Vector3 startPosition;
    [SerializeField] private Vector3 endPosition;
    [SerializeField] private TMP_Text achievementText;
    [SerializeField] private GUIStyle testVariable;

    private void Start()
    {
        //rectTransform = GetComponent<RectTransform>();
        rectTransform.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        AchievementsMenu.OnAchievementUnlock += ShowAchievement;
    }

    private void OnDisable() 
    {
        AchievementsMenu.OnAchievementUnlock -= ShowAchievement;
    }

    public void ShowAchievement(Achievement achievement)
    {
        achievementText.text = achievement.Name;
        rectTransform.gameObject.SetActive(true);
        StopCoroutine(LerpObject(startPosition, endPosition));
        StartCoroutine(LerpObject(startPosition, endPosition));
    }
    IEnumerator LerpObject(Vector3 start, Vector3 end)
    {
        Debug.Log("Start Lerp");
        rectTransform.anchoredPosition = start;
        currentTime = 0;
        while (currentTime <= lerpSpeed) 
        { 
            currentTime += Time.deltaTime; 
            normalizedValue = currentTime / lerpSpeed; // normalize time 
        
            rectTransform.anchoredPosition = Vector3.Lerp(start, end, normalizedValue); 
            yield return null; 
        }
        currentTime = 0;
        yield return new WaitForSeconds(displayTime);
        while (currentTime <= lerpSpeed) 
        { 
            currentTime += Time.deltaTime; 
            normalizedValue = currentTime / lerpSpeed; // normalize time 
        
            rectTransform.anchoredPosition = Vector3.Lerp(end, start, normalizedValue); 
            yield return null;
        }
        rectTransform.gameObject.SetActive(false);
    }
}