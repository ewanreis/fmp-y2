using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    #region Audio Sources
    [System.Serializable]
    private struct SourceList
    {
        public AudioSource musicAudioSource;
        public AudioSource ambianceAudioSource;
        public AudioSource environmentAudioSource;
        public AudioSource footstepsAudioSource;
        public AudioSource enemySFXAudioSource;
    }

    [System.Serializable]
    private struct VolumeList
    {
        [Range(0, 100)] public int musicVolume;
        [Range(0, 100)] public int ambianceVolume;
        [Range(0, 100)] public int environmentVolume;
        [Range(0, 100)] public int footstepsVolume;
        [Range(0, 100)] public int enemySFXVolume;
    }

    [SerializeField] private SourceList audioSources;
    [SerializeField] private VolumeList volume;
    #endregion

    #region Footstep Variables
    [SerializeField, Header("Footsteps")] private float stepDelay;

    [SerializeField] public AudioClip[] footstepSounds;


    private bool isPlayingStepSound;
    #endregion

    #region Footsteps
    public void PlayFootstepSound()
    {
        if(!isPlayingStepSound)
            StartCoroutine(FootstepCoroutine());
    }

    private IEnumerator FootstepCoroutine()
    {
        isPlayingStepSound = true;
        audioSources.footstepsAudioSource.PlayOneShot(footstepSounds[Random.Range(0, footstepSounds.Length)]);
        yield return new WaitForSeconds(stepDelay);
        isPlayingStepSound = false;
    }
    #endregion
}