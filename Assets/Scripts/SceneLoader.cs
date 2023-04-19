using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class SceneLoader : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Slider loadSlider;
    [SerializeField] private TMP_Text loadPercentText;
    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadGameAsync(sceneName));
    }
    private IEnumerator LoadGameAsync(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        loadingScreen.SetActive(true);
        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            loadSlider.value = progress;
            loadPercentText.text = $"{progress * 100}%";
            yield return null;
        }
        loadingScreen.SetActive(false);
    }
}
