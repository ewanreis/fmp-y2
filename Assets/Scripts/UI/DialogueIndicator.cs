using UnityEngine;

public class DialogueIndicator : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private GameObject spriteObject;
    [SerializeField] private float detectionRange = 3f;
    [SerializeField] [Multiline] private string[] dialogueLines;
    [SerializeField] private DialogueSystem dialogueSystem;

    private bool isInRange = false;

    private void OnEnable()
    {
        InputManager.OnSecondaryPressed += TryTalk;
    }

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
    }

    private void TryTalk()
    {
        if(!isInRange)
            return;

        dialogueSystem.StartDialogue(dialogueLines[Random.Range(0, dialogueLines.Length)]);
    }
}
