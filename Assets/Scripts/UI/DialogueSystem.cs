using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueSystem : MonoBehaviour
{
    [SerializeField] private AudioClip[] letterSounds;
    [SerializeField] private TMP_Text dialogueTextBox;
    
    [SerializeField] private float letterDelay = 0.1f; // delay between each letter
    [SerializeField] private float sentenceDelay = 1f; // delay between each sentence

    [SerializeField] private PlayerAudio playerAudio;

    private string currentDialogue;


    void Start()
    {
        playerAudio = GetComponent<PlayerAudio>();
        InputManager.OnPrimaryPressed += StartDialogue;
    }

    public void StartDialogue()
    {
        currentDialogue = "Test Dialogue Speech";
        StartCoroutine(TypeSentence());
    }

    IEnumerator TypeSentence()
    {
        dialogueTextBox.text = "";
        foreach (char letter in currentDialogue.ToCharArray())
        {
            dialogueTextBox.text += letter;
            playerAudio.PlaySound(AudioChannel.Dialogue, letterSounds[Random.Range(0, letterSounds.Length)]);
            yield return new WaitForSeconds(letterDelay);
        }
        yield return new WaitForSeconds(sentenceDelay);
        EndDialogue();
    }

    void EndDialogue()
    {
        dialogueTextBox.text = "";
    }
}
