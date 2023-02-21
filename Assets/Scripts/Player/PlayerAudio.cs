using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] private AudioSource[] audioSources = new AudioSource[6];

    [SerializeField] private AudioClip[] footstepSounds;
    [SerializeField] private AudioClip[] musicTracks;
    [SerializeField] private float footstepDelay;
    [SerializeField] private AudioMixer audioMixer;

    private bool isPlayingFootstep;
    private bool isPlayingMusic;

    private int currentTrackNumber = 0;

    private void Start()
    {
        // make ui sound play when game paused
        audioSources[(int)AudioChannel.UI].ignoreListenerPause = true;
        MultilegProcedural.OnThroneStep += PlayFootstepSound;
        PlaySong();
    }

    public void PlayFootstepSound()
    {
        if (!isPlayingFootstep)
            StartCoroutine(FootstepCoroutine());
    }

    public void PlaySong()
    {
        if(!isPlayingMusic)
            StartCoroutine(MusicTrackCoroutine());
    }

    private IEnumerator FootstepCoroutine()
    {
        // flag to prevent multiple footstep sounds from playing at once
        isPlayingFootstep = true;

        // play random footstep sound on footstep channel
        audioSources[(int)AudioChannel.Footsteps].PlayOneShot(footstepSounds[Random.Range(0, footstepSounds.Length)]);

        yield return new WaitForSeconds(footstepDelay);
        isPlayingFootstep = false;
    }

    private IEnumerator MusicTrackCoroutine()
    {
        // flag to prevent multiple songs playing at once
        isPlayingMusic = true;

        // play the current track
        audioSources[(int)AudioChannel.Music].PlayOneShot(musicTracks[currentTrackNumber]);
        Debug.Log($"Song playing: {musicTracks[currentTrackNumber].ToString()}\nSong Length: {musicTracks[currentTrackNumber].length}");

        // wait until track is done playing
        yield return new WaitForSeconds(musicTracks[currentTrackNumber].length);

        // go to next song
        currentTrackNumber++;
        isPlayingMusic = false;
        PlaySong();
    }

    public void SetVolume(string groupName, float volume)
    {
        // set audio mixer volume converting to decibels
        audioMixer.SetFloat(groupName, Mathf.Log10(volume) * 20);
        //SaveData.SaveVolume()
    }

    public static void PauseAllSounds(bool pause)
    {
        // pause or resume all sounds in the game (except ui)
        AudioListener.pause = pause;
    }

    // editor-only function to label audio sources in inspector
    #if UNITY_EDITOR
    private void OnValidate()
    {
        // get number of channels
        var enumCount = System.Enum.GetNames(typeof(AudioChannel)).Length;
        
        // display error if sources dont match channel number
        if (audioSources.Length != enumCount)
            Debug.LogError("Number of sources do not match");
        else
        {
            // label source with corresponding enum name
            for (int i = 0; i < enumCount; i++)
                audioSources[i].name = System.Enum.GetName(typeof(AudioChannel), i);
        }
    }
    #endif
}

public enum AudioChannel
{
    Music,
    Ambiance,
    Environment,
    Footsteps,
    EnemySFX,
    UI
}