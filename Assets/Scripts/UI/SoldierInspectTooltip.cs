using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class SoldierInspectTooltip : MonoBehaviour
{
    public static event Action OnTooltipShow;
    public static event Action OnTooltipHide;

    [SerializeField] private InputActionReference keybindAction; // reference for the keybind
    [SerializeField] private TMP_Text tooltipText; // reference to the text box
    [SerializeField] private GameObject objectToFollow;
    [SerializeField] private RectTransform canvasRect;

    private bool showTooltip = false;

    void Start()
    {
        string keybind = keybindAction.action.GetBindingDisplayString();
        tooltipText.text = $"Press {keybind} to Inspect.";
        showTooltip = false;
        tooltipText.gameObject.SetActive(false);
        SoldierInspect.OnSoldierHover += ShowTooltip;
        SoldierInspect.OnSoldierStopHovering += HideTooltip;
    }

    private void ShowTooltip(Soldier soldier)
    {
        string keybind = keybindAction.action.GetBindingDisplayString();
        tooltipText.text = $"Press {keybind} to inspect {soldier.soldierType}.";
        objectToFollow = soldier.gameObject;
        showTooltip = true;
        tooltipText.gameObject.SetActive(true);
        OnTooltipShow?.Invoke();
    }

    private void HideTooltip()
    {
        string keybind = keybindAction.action.GetBindingDisplayString();
        tooltipText.text = $"Press {keybind} to Inspect.";
        showTooltip = false;
        tooltipText.gameObject.SetActive(false);
        OnTooltipHide?.Invoke();
    }

    private void LateUpdate()
    {
        if(!showTooltip)
            return;

        // convert position to screen space
        Vector2 screenPos = Camera.main.WorldToScreenPoint(objectToFollow.transform.position);

        tooltipText.transform.position = screenPos;

        // clamp position to the boundaries of the canvas
        Vector2 clampedPos = transform.position;
        clampedPos.x = Mathf.Clamp(clampedPos.x, canvasRect.rect.xMin + tooltipText.preferredWidth / 2, canvasRect.rect.xMax - tooltipText.preferredWidth / 2);
        clampedPos.y = Mathf.Clamp(clampedPos.y, canvasRect.rect.yMin + tooltipText.preferredHeight / 2, canvasRect.rect.yMax - tooltipText.preferredHeight / 2);
        transform.position = clampedPos;
    }
}
