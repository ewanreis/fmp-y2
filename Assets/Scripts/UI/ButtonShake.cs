using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ButtonShake : MonoBehaviour, UnityEngine.EventSystems.ISelectHandler, UnityEngine.EventSystems.IDeselectHandler
{
    //* Used for quit menu buttons to shake when hovered over
    [SerializeField] private Button button; 
    [SerializeField] private float shakeMagnitude = 0.7f;
    [SerializeField] private float shakeDuration = 0.5f;
    [SerializeField] private float rotationSpeed = 50f;

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Coroutine shakeCoroutine;

    void Start()
    {
        // save original position and rotation of the button
        originalPosition = button.transform.position;
        originalRotation = button.transform.rotation;
    }

    public void OnSelect(UnityEngine.EventSystems.BaseEventData eventData)
    {
        shakeCoroutine = StartCoroutine(ShakeButtonCoroutine());
    }

    public void OnDeselect(UnityEngine.EventSystems.BaseEventData eventData)
    {
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
            shakeCoroutine = null;
        }
        button.transform.position = originalPosition;
        button.transform.rotation = originalRotation;
    }

    private IEnumerator ShakeButtonCoroutine()
    {
        while (true)
        {
            float shakeIntensity = Mathf.Lerp(shakeMagnitude, 0f, shakeDuration / 2f);
            Vector3 shakeOffset = new Vector3(Random.Range(-shakeIntensity, shakeIntensity), Random.Range(-shakeIntensity, shakeIntensity), 0f);
            button.transform.position = originalPosition + shakeOffset;
            button.transform.rotation = originalRotation * Quaternion.Euler(0f, 0f, Mathf.Sin(Time.time * rotationSpeed) * shakeIntensity);
            yield return null;
        }
    }
}
