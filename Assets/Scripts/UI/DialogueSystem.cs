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

    private bool isShowingDialogue;

    private string currentDialogue;


    void Start()
    {
        playerAudio = GetComponent<PlayerAudio>();
    }

    private void OnEnable() 
    {
        InputManager.OnSecondaryPressed += StartDialogue;
    }

    private void OnDisable() 
    {
        InputManager.OnSecondaryPressed -= StartDialogue;
    }

    public void StartDialogue()
    {
        currentDialogue = "Test Dialogue Speech.\nSecond line of dialogue.\nThird Line.";
        if(!isShowingDialogue && !PauseMenu.paused)
            StartCoroutine(TypeSentence());
    }

    IEnumerator TypeSentence()
    {
        isShowingDialogue = true;
        dialogueTextBox.text = "";
        foreach (char letter in currentDialogue.ToCharArray())
        {
            dialogueTextBox.text += letter;
            playerAudio.PlaySound(AudioChannel.Dialogue, letterSounds[Random.Range(0, letterSounds.Length)]);
            yield return new WaitForSeconds(letterDelay);
        }
        yield return new WaitForSeconds(sentenceDelay);
        EndDialogue();
        isShowingDialogue = false;
    }

    void EndDialogue()
    {
        dialogueTextBox.text = "";
    }
}
