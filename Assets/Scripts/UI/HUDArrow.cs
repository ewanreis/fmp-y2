using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class HUDArrow : MonoBehaviour
{
    [SerializeField] private RectTransform circleCenter;
    [SerializeField] private GameObject centerFollow;
    [SerializeField] private float circleRadius = 100f;
    [SerializeField] private float rotateSpeed = 5f;
    [SerializeField] private Image arrowImage;

    private Vector3 targetPosition;
    private Vector3 directionVector;
    private bool showingArrow;

    private void Start()
    {
        HideArrow();
        MountableObject.OnMount += ShowArrow;
        MountableObject.OnUnmount += HideArrow;
    }

    private void ShowArrow()
    {
        arrowImage.gameObject.SetActive(true);
        showingArrow = true;
    }

    private void HideArrow()
    {
        arrowImage.gameObject.SetActive(false);
        showingArrow = false;
    }

    private void LateUpdate()
    {
        if(!showingArrow)
            return;

        Vector2 pos = new Vector2();
        if(!InputManager.usingController)
        {
            // get current mouse position in screen space
            pos = Mouse.current.position.ReadValue();

            targetPosition = pos;

            // convert target position to ui coordinates
            Vector2 viewportPoint = Camera.main.ScreenToViewportPoint(targetPosition);
            targetPosition = new Vector2(viewportPoint.x * Screen.width, viewportPoint.y * Screen.height);
            // get direction vector from arrow to target position
            directionVector = targetPosition - circleCenter.position;
        }
        else
        {
            pos = InputManager.GetRightStickDirection() * 10;
            directionVector = pos;
        }

        // limit magnitude to the circle radius
        if (directionVector.magnitude > circleRadius)
        {
            directionVector = directionVector.normalized * circleRadius;
            targetPosition = circleCenter.position + directionVector;
        }

        // calculate angle of rotation
        float angle = Mathf.Atan2(directionVector.y, directionVector.x) * Mathf.Rad2Deg - 90f;

        // calculate position of arrow on the circle using the angle and radius
        Vector2 circlePosition = circleCenter.position + Quaternion.AngleAxis(angle, Vector3.forward) * Vector2.up * circleRadius;

        // set position of the arrow to the circle position
        transform.position = circlePosition;

        // rotate the arrow
        arrowImage.transform.rotation = Quaternion.RotateTowards(arrowImage.transform.rotation, Quaternion.LookRotation(Vector3.forward, directionVector), rotateSpeed * Time.deltaTime);
    }
}
