using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using UnityEngine.UI;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] private AudioSource[] audioSources = new AudioSource[6];

    [SerializeField] private AudioClip[] footstepSounds;
    [SerializeField] private AudioClip[] musicTracks;
    [SerializeField] private float footstepDelay;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider[] sliders;

    private bool isPlayingFootstep;
    private bool isPlayingMusic;

    private int currentTrackNumber = 0;

    private void Start()
    {
        // make ui sound play when game paused
        audioSources[(int)AudioChannel.UI].ignoreListenerPause = true;

        MultilegProcedural.OnThroneStep += PlayFootstepSound;
        Invoke("GetSavedVolume", 0.01f);
        Invoke("SetSlidersToSavedVolume", 0.5f);
        PlaySong();
        InputManager.OnSkipSongPressed += SkipSong;
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

    public void SkipSong()
    {
        StopCoroutine(MusicTrackCoroutine());
        isPlayingMusic = false;
        IncreaseMusicIndex();
        audioSources[(int)AudioChannel.Music].Stop();
        PlaySong();
    }

    private void IncreaseMusicIndex()
    {
        currentTrackNumber++;
        if(currentTrackNumber >= musicTracks.Length)
            currentTrackNumber = 0;
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
        IncreaseMusicIndex();
        isPlayingMusic = false;
        PlaySong();
    }

    // ensures the slider handle position is equal to the volume
    private void SetSlidersToSavedVolume()
    {
        for(int i = 0; i < sliders.Length; i++)
        {
            sliders[i].value = GetVolume((AudioChannel)i);
        }
    }

    #region Volume Methods
    public void SetVolume(AudioChannel audioChannel, float volume)
    {
        string groupName = audioChannel.ToString();

        // clamp volume to slider min & max
        volume = Mathf.Clamp(volume, -0.01f, 1);

        // set audio mixer volume converting to decibels
        audioMixer.SetFloat(groupName, Mathf.Log10(volume) * 20);

        // save the current volume
        SaveData.SaveVolume(audioChannel, volume);

        // update the sliders (for use when using button to increment)
        sliders[(int)audioChannel].value = GetVolume(audioChannel);
    }

    public void IncrementVolume(AudioChannel audioChannel, float increment)
    {
        float currentVolume = GetVolume(audioChannel);
        float newVolume = currentVolume + increment;
        SetVolume(audioChannel, newVolume);
    }


    public void GetSavedVolume()
    {
        float[] channelVolumes = SaveData.GetSavedAudioVolumes();

        foreach (AudioChannel channel in (AudioChannel[]) System.Enum.GetValues(typeof(AudioChannel)))
            SetVolume(channel, channelVolumes[(int)channel]);
    }

    public void PlaySound(AudioChannel channel, AudioClip sound)
    {
        audioSources[(int)channel].PlayOneShot(sound);
    }

    public float GetVolume(AudioChannel audioChannel) => SaveData.GetSavedAudioVolumes()[(int)audioChannel];

    public void SetVolumeMaster(float volume) => SetVolume(AudioChannel.Master, volume);
    public void SetVolumeMusic(float volume) => SetVolume(AudioChannel.Music, volume);
    public void SetVolumeAmbiance(float volume) => SetVolume(AudioChannel.Ambiance, volume);
    public void SetVolumeEnvironment(float volume) => SetVolume(AudioChannel.Environment, volume);
    public void SetVolumeFootsteps(float volume) => SetVolume(AudioChannel.Footsteps, volume);
    public void SetVolumeEnemySFX(float volume) => SetVolume(AudioChannel.EnemySFX, volume);
    public void SetVolumeUI(float volume) => SetVolume(AudioChannel.UI, volume);
    public void SetVolumeDialogue(float volume) => SetVolume(AudioChannel.Dialogue, volume);

    public void IncrementVolumeMaster(float increment) => IncrementVolume(AudioChannel.Master, increment);
    public void IncrementVolumeMusic(float increment) => IncrementVolume(AudioChannel.Music, increment);
    public void IncrementVolumeAmbiance(float increment) => IncrementVolume(AudioChannel.Ambiance, increment);
    public void IncrementVolumeEnvironment(float increment) => IncrementVolume(AudioChannel.Environment, increment);
    public void IncrementVolumeFootsteps(float increment) => IncrementVolume(AudioChannel.Footsteps, increment);
    public void IncrementVolumeEnemySFX(float increment) => IncrementVolume(AudioChannel.EnemySFX, increment);
    public void IncrementVolumeUI(float increment) => IncrementVolume(AudioChannel.UI, increment);
    public void IncrementVolumeDialogue(float increment) => IncrementVolume(AudioChannel.Dialogue, increment);
    #endregion

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
    Master,
    Music,
    Ambiance,
    Environment,
    Footsteps,
    EnemySFX,
    UI,
    Dialogue
}