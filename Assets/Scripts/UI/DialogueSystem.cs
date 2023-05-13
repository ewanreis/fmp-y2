using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueSystem : MonoBehaviour
{
    //* Used for managing the sound and subtitles of dialogue
    [SerializeField] private AudioClip[] letterSounds;
    [SerializeField] private AudioClip[] nonLetterSounds;
    [SerializeField] private TMP_Text dialogueTextBox;
    
    [SerializeField] private float letterDelay = 0.1f; // delay between each letter
    [SerializeField] private float sentenceDelay = 1f; // delay between each sentence
    [SerializeField] private float wordDelay = 0.2f; // delay between each word

    [SerializeField] private PlayerAudio playerAudio;

    private bool isShowingDialogue;
    private bool canTalk;

    private string currentDialogue;


    void Start()
    {
        playerAudio = GetComponent<PlayerAudio>();
    }

    private void OnEnable() 
    {
        InputManager.OnSecondaryPressed += StartDialogue;
        MountableObject.OnMount += DisableDialogue;
        MountableObject.OnUnmount += EnableDialogue;
    }

    private void OnDisable() 
    {
        InputManager.OnSecondaryPressed -= StartDialogue;
        MountableObject.OnMount -= DisableDialogue;
        MountableObject.OnUnmount -= EnableDialogue;
    }

    public void StartDialogue()
    {
        if(!canTalk)
            return;

        //currentDialogue = "Test Dialogue Speech.\nSecond line of dialogue.\nThird Line.";
        //currentDialogue = "Test Test Test Test Test Speech Speech Speech Speech ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        currentDialogue = "Well hello there, I see you've defeated everyone... \nThe Queen is on her way, so you should run\n Ha Ha Ha";
        if(!isShowingDialogue && !PauseMenu.paused)
            StartCoroutine(TypeSentence());
    }

    IEnumerator TypeSentence()
    {
        isShowingDialogue = true;
        dialogueTextBox.text = "";
        foreach (char letter in currentDialogue)
        {
            if (char.IsLetter(letter))
            {
                char upperCaseLetter = char.ToUpper(letter);
                int soundIndex = (int)upperCaseLetter - 65;
                playerAudio.PlaySound(AudioChannel.UI, letterSounds[soundIndex]);
                dialogueTextBox.text += letter;
                yield return new WaitForSeconds(letterDelay);
            }
            else
            {
                if (char.IsPunctuation(letter))
                    playerAudio.PlaySound(AudioChannel.UI, nonLetterSounds[Random.Range(0, nonLetterSounds.Length)]);

                else
                    yield return new WaitForSeconds(wordDelay);

                dialogueTextBox.text += letter;
            }
            if(letter == '.')
                yield return new WaitForSeconds(sentenceDelay);
        }
        
        EndDialogue();
        isShowingDialogue = false;
    }

    void EndDialogue()
    {
        dialogueTextBox.text = "";
    }

    private void EnableDialogue() => canTalk = true;
    private void DisableDialogue() => canTalk = false;
}
