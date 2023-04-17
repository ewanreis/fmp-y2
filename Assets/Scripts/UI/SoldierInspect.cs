using System;
using System.Collections;
using UnityEngine;

public class SoldierInspect : MonoBehaviour
{
    public static event Action<Soldier> OnSoldierInspect;
    public static event Action<Soldier> OnSoldierHover;
    public static event Action OnSoldierStopHovering;

    [SerializeField] private Color hoverColor = Color.white;
    [SerializeField] private float transitionDuration = 0.3f;
    [SerializeField] private Soldier soldier;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private Color targetColor;
    private Coroutine colorTransitionCoroutine;

    private bool canInspect = true;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        soldier = GetComponent<Soldier>();
        originalColor = spriteRenderer.color;
        targetColor = originalColor;
        MountableObject.OnMount += DisableInspection;
        MountableObject.OnUnmount += EnableInspection;
    }

    private void DisableInspection()
    {
        canInspect = false;
        spriteRenderer.color = originalColor;
        OnSoldierStopHovering?.Invoke();
    }

    private void EnableInspection()
    {
        canInspect = true;
    }

    private void OnMouseEnter()
    {
        if(!canInspect)
            return;

        if (colorTransitionCoroutine != null && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            StopCoroutine(colorTransitionCoroutine);
        }
        Debug.Log(soldier);
        OnSoldierHover?.Invoke(soldier);
        colorTransitionCoroutine = StartCoroutine(TransitionColor(hoverColor));
    }

    private void OnMouseExit()
    {
        

        if (colorTransitionCoroutine != null)
        {
            StopCoroutine(colorTransitionCoroutine);
        }
        OnSoldierStopHovering?.Invoke();
        colorTransitionCoroutine = StartCoroutine(TransitionColor(originalColor));
    }

    private void OnMouseDown()
    {
        if(!canInspect)
            return;

        if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            OnSoldierInspect?.Invoke(soldier);
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
