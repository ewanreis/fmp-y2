using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using TMPro;
using System;

public class TooltipThrone : MonoBehaviour
{
    public static event Action OnTooltipShow;
    public static event Action OnTooltipHide;

    [SerializeField] private InputActionReference keybindAction; // reference for the keybind
    [SerializeField] private TMP_Text tooltipText; // reference to the text box
    [SerializeField] private GameObject objectToFollow;
    [SerializeField] private RectTransform canvasRect;

    private bool showTooltip = false;
    private MountableObject throne;

    private void Start()
    {
        throne = GetComponent<MountableObject>();
        string keybind = keybindAction.action.GetBindingDisplayString();
        tooltipText.text = $"Press {keybind} to Mount.";
        showTooltip = false;
        tooltipText.gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        bool isMounted = throne.GetMountState();
        if(isMounted)
            return;

        if (other.gameObject.tag == "Self")
        {
            showTooltip = true;
            OnTooltipShow.Invoke();
            tooltipText.gameObject.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Self")
        {
            showTooltip = false;
            OnTooltipHide.Invoke();
            tooltipText.gameObject.SetActive(false);
        }
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