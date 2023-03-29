using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SoldierInspect : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private Color hoverColor = Color.white;

    [SerializeField]
    private float transitionDuration = 0.3f;

    [SerializeField]
    private Soldier soldier;

    private Color originalColor;
    private Color targetColor;

    private Coroutine colorTransitionCoroutine;

    public static event Action<Soldier> OnSoldierInspect;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        soldier = GetComponent<Soldier>();
        originalColor = spriteRenderer.color;
        targetColor = originalColor;
    }

    private void OnMouseEnter()
    {
        if (colorTransitionCoroutine != null && !EventSystem.current.IsPointerOverGameObject())
        {
            StopCoroutine(colorTransitionCoroutine);
        }
        colorTransitionCoroutine = StartCoroutine(TransitionColor(hoverColor));
    }

    private void OnMouseExit()
    {
        if (colorTransitionCoroutine != null)
        {
            StopCoroutine(colorTransitionCoroutine);
        }
        colorTransitionCoroutine = StartCoroutine(TransitionColor(originalColor));
    }

    private void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
            OnSoldierInspect.Invoke(soldier);
    }

    private IEnumerator TransitionColor(Color newTargetColor)
    {
        float elapsedTime = 0f;
        Color startColor = spriteRenderer.color;

        while (elapsedTime < transitionDuration)
        {
            spriteRenderer.color = Color.Lerp(startColor, newTargetColor, elapsedTime / transitionDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        spriteRenderer.color = newTargetColor;
        targetColor = newTargetColor;
    }
}
