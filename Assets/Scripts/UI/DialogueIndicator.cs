using UnityEngine;

public class DialogueIndicator : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private GameObject spriteObject;
    [SerializeField] private float detectionRange = 3f;

    private bool isInRange = false;

    private void FixedUpdate()
    {
        float distance = Vector2.Distance(transform.position, player.position);

        if(distance > detectionRange)
        {
            ChangeIndicatorStatus(false);
            return;
        }

        ChangeIndicatorStatus(true);
    }

    private void ChangeIndicatorStatus(bool status)
    {
        spriteObject.SetActive(status);
        isInRange = status;
        //Debug.Log(status);
    }
}
