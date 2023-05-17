using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using UnityEngine.UI;
public class PlayerAudio : MonoBehaviour
{
    //* Manages all of the audio for the game
    public static event System.Action OnMuteMusic;
    [SerializeField] private AudioSource[] audioSources = new AudioSource[6];
    [SerializeField] private float footstepDelay;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider[] sliders;
    [SerializeField] private GameObject sourceParent;

    #region AudioClips
    [SerializeField] private MonsterSounds monsterSounds;
    [SerializeField] private AudioClip[] playerDamageSounds;
    [SerializeField] private AudioClip[] waveStartSounds;
    [SerializeField] private AudioClip[] shopBuySounds;
    [SerializeField] private AudioClip[] shopOpenSounds;
    [SerializeField] private AudioClip[] scoreGainSounds;
    [SerializeField] private AudioClip[] footstepSounds;
    [SerializeField] private AudioClip[] musicTracks;
    [SerializeField] private AudioClip[] throneShootSounds;
    [SerializeField] private AudioClip[] throneHitSounds;
    [SerializeField] private AudioClip pauseSound;
    [SerializeField] private AudioClip resumeSound;
    [SerializeField] private AudioClip buttonDenySound;
    [SerializeField] private AudioClip buttonClickSound;
    [SerializeField] private AudioClip buttonHoverSound;
    [SerializeField] private AudioClip volumeChangeSound;
    [SerializeField] private AudioClip submenuOpenSound;
    [SerializeField] private AudioClip lowHealthSound;
    [SerializeField] private AudioClip criticalHealthSound;
    [SerializeField] private AudioClip deathSound;
    #endregion

    private bool isPlayingFootstep;
    private bool isPlayingMusic;
    private int currentTrackNumber = 0;

    private void OnEnable()
    {
        // make ui sound play when game paused
        audioSources[(int)AudioChannel.UI].ignoreListenerPause = true;

        MultilegProcedural.OnThroneStep += PlayFootstepSound;
        Invoke("GetSavedVolume", 0.01f);
        Invoke("SetSlidersToSavedVolume", 0.5f);
        PlaySong();
        InputManager.OnSkipSongPressed += SkipSong;
        ShopButton.OnLockedButtonClick += PlayButtonDenySound;
        ShopButton.OnShopButtonClick += PlayShopBuySound;
        ScoreManager.OnPointsGained += PlayScoreGainSound;
        PauseMenu.OnPause += PlayPauseSound;
        PauseMenu.OnResume += PlayResumeSound;
        Bestiary.OnBestiaryOpen += PlaySubmenuOpenSound;
        AchievementsMenu.OnAchievementsMenuOpen += PlaySubmenuOpenSound;
        EnemySpawner.OnWaveStart += PlayWaveStartSound;
        PlayerHealth.OnUpdateHealth += PlayDamageSound;
        TaskWanderEnemy.OnReachPoint += PlayMonsterAttackSound;
        PlayerHealth.OnLowHealth += PlayLowHealthSound;
        PlayerHealth.OnCriticalHealth += PlayCriticalHealthSound;
        PlayerHealth.OnDeath += PlayDeathSound;
        ThroneShoot.OnShoot += PlayThroneShootSound;
        ThroneProjectile.OnHit += PlayThroneHitSound;
        ExplodeOnCollision.OnExplode += PlayThroneHitSound;
    }

    private void OnDisable()
    {
        MultilegProcedural.OnThroneStep -= PlayFootstepSound;
        InputManager.OnSkipSongPressed -= SkipSong;
        ShopButton.OnLockedButtonClick -= PlayButtonDenySound;
        ShopButton.OnShopButtonClick -= PlayShopBuySound;
        ScoreManager.OnPointsGained -= PlayScoreGainSound;
        PauseMenu.OnPause -= PlayPauseSound;
        PauseMenu.OnResume -= PlayResumeSound;
        Bestiary.OnBestiaryOpen -= PlaySubmenuOpenSound;
        AchievementsMenu.OnAchievementsMenuOpen -= PlaySubmenuOpenSound;
        EnemySpawner.OnWaveStart -= PlayWaveStartSound;
        PlayerHealth.OnUpdateHealth -= PlayDamageSound;
        TaskWanderEnemy.OnReachPoint -= PlayMonsterAttackSound;
        PlayerHealth.OnLowHealth -= PlayLowHealthSound;
        PlayerHealth.OnCriticalHealth -= PlayCriticalHealthSound;
        PlayerHealth.OnDeath -= PlayDeathSound;
        ThroneShoot.OnShoot -= PlayThroneShootSound;
        ThroneProjectile.OnHit -= PlayThroneHitSound;
        ExplodeOnCollision.OnExplode -= PlayThroneHitSound;
    }

    public void PlayThroneShootSound() => PlayRandomSoundInArray(AudioChannel.Environment, throneShootSounds);
    public void PlayThroneHitSound(Vector3 pos) => PlayRandom3DClipInArray(pos, AudioChannel.Environment, throneHitSounds);

    public void PlayLowHealthSound()
    {
        if (!audioSources[(int)AudioChannel.Environment].isPlaying)
            PlaySound(AudioChannel.Environment, lowHealthSound);
    }

    public void PlayCriticalHealthSound()
    {
        if (!audioSources[(int)AudioChannel.Environment].isPlaying)
            PlaySound(AudioChannel.Environment, criticalHealthSound);
    }

    public void PlayDamageSound(int health) => PlayRandomSoundInArray(AudioChannel.Environment, playerDamageSounds);
    public void PlayDeathSound() => PlaySound(AudioChannel.Environment, deathSound);

    public void PlayMonsterAttackSound(EnemyPositionType positionType)
    {
        AudioClip[] clipArray = positionType.type switch
        {
            EnemyTypes.Fodder => monsterSounds.fodderSounds.attackSounds,
            EnemyTypes.Footsoldier => monsterSounds.footsoldierSounds.attackSounds,
            EnemyTypes.Commander => monsterSounds.commanderSounds.attackSounds,
            _ => monsterSounds.fodderSounds.attackSounds
        };
        PlayRandom3DClipInArray(positionType.enemyPosition.position, AudioChannel.EnemySFX, clipArray);
    }

    public void PlayWaveStartSound(int wave)
    {
        if(wave == 10)
        {
            PlaySound(AudioChannel.Ambiance, waveStartSounds[3]);
            return;
        }
        else if(wave == 15)
        {
            PlaySound(AudioChannel.Ambiance, waveStartSounds[4]);
            return;
        }
        else
            PlayRandomSoundInArray(AudioChannel.Ambiance, waveStartSounds, 3);
    }

    #region Music
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

    private IEnumerator MusicTrackCoroutine()
    {
        // flag to prevent multiple songs playing at once
        isPlayingMusic = true;

        // play the current track
        PlaySound(AudioChannel.Music, musicTracks[currentTrackNumber]);
        Debug.Log($"Song playing: {musicTracks[currentTrackNumber].ToString()}\nSong Length: {musicTracks[currentTrackNumber].length}");

        // wait until track is done playing
        yield return new WaitForSeconds(musicTracks[currentTrackNumber].length);

        // go to next song
        IncreaseMusicIndex();
        isPlayingMusic = false;
        PlaySong();
    }
    #endregion

    #region Footsteps
    public void PlayFootstepSound()
    {
        if (!isPlayingFootstep)
            StartCoroutine(FootstepCoroutine());
    }

    private IEnumerator FootstepCoroutine()
    {
        // flag to prevent multiple footstep sounds from playing at once
        isPlayingFootstep = true;

        // play random footstep sound on footstep channel
        PlayRandomSoundInArray(AudioChannel.Footsteps, footstepSounds);

        yield return new WaitForSeconds(footstepDelay);
        isPlayingFootstep = false;
    }
    #endregion

    #region UI Sounds
    public void PlayVolumeChangeSound() => PlaySound(AudioChannel.UI, volumeChangeSound);
    public void PlayButtonHoverSound() => PlaySound(AudioChannel.UI, buttonHoverSound);
    public void PlayButtonClickSound() => PlaySound(AudioChannel.UI, buttonClickSound);
    public void PlayPauseSound() => PlaySound(AudioChannel.UI, pauseSound);
    public void PlayResumeSound() => PlaySound(AudioChannel.UI, resumeSound);
    private void PlayButtonDenySound() => PlaySound(AudioChannel.UI, buttonDenySound);
    private void PlayScoreGainSound(int i) => PlayRandomSoundInArray(AudioChannel.UI, scoreGainSounds);
    private void PlayShopBuySound() => PlayRandomSoundInArray(AudioChannel.UI, shopBuySounds);
    public void PlayShopOpenSound() => PlayRandomSoundInArray(AudioChannel.UI, shopOpenSounds);
    public void PlaySubmenuOpenSound() => PlaySound(AudioChannel.UI, submenuOpenSound);
    #endregion

    #region Volume Methods
    // ensures the slider handle position is equal to the volume
    private void SetSlidersToSavedVolume()
    {
        for(int i = 0; i < sliders.Length; i++)
            sliders[i].value = GetVolume((AudioChannel)i);
    }

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
    => audioSources[(int)channel]?.PlayOneShot(sound);

    public void PlayRandomSoundInArray(AudioChannel channel, AudioClip[] soundArray)
    => audioSources[(int)channel]?.PlayOneShot(soundArray[Random.Range(0, soundArray.Length)]);

    public void PlayRandomSoundInArray(AudioChannel channel, AudioClip[] soundArray, int maxRange)
    => audioSources[(int)channel]?.PlayOneShot(soundArray[Random.Range(0, maxRange)]);

    public void PlayRandomSoundInArray(AudioChannel channel, AudioClip[] soundArray, int minRange, int maxRange)
    => audioSources[(int)channel]?.PlayOneShot(soundArray[Random.Range(minRange, maxRange)]);

    private AudioSource Play3DClip(Vector3 pos, AudioChannel channel, AudioClip clip)
    {
        float volume = GetVolume(channel);
        GameObject sourceGameObject = new GameObject("TempAudio");
        pos.z = Camera.main.transform.position.z;
        sourceGameObject.transform.position = pos;
        sourceGameObject.transform.parent = sourceParent.transform;

        AudioSource aSource = sourceGameObject.AddComponent<AudioSource>() as AudioSource;
        aSource.clip = clip;
        aSource.volume = volume;
        aSource.spatialBlend = 1;

        aSource.Play();
        Destroy(sourceGameObject, clip.length);
        return aSource;
    }

    public void PlayRandom3DClipInArray(Vector3 pos, AudioChannel channel, AudioClip[] soundArray)
    => Play3DClip(pos, channel, soundArray[Random.Range(0, soundArray.Length)]);

    public void PlayRandom3DClipInArray(Vector3 pos, AudioChannel channel, AudioClip[] soundArray, int maxRange)
    => Play3DClip(pos, channel, soundArray[Random.Range(0, maxRange)]);

    public void PlayRandom3DClipInArray(Vector3 pos, AudioChannel channel, AudioClip[] soundArray, int minRange, int maxRange)
    => Play3DClip(pos, channel, soundArray[Random.Range(minRange, maxRange)]);

    public float GetVolume(AudioChannel audioChannel) => SaveData.GetSavedAudioVolumes()[(int)audioChannel];

    public void SetVolumeMaster(float volume) => SetVolume(AudioChannel.Master, volume);
    public void SetVolumeMusic(float volume) 
    {
        if(volume == 0)
            OnMuteMusic?.Invoke();

        SetVolume(AudioChannel.Music, volume);
    } 
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

    public static void PauseAllSounds(bool pause)
    {
        // pause or resume all sounds in the game (except ui)
        AudioListener.pause = pause;
    }
    #endregion

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

[System.Serializable]
struct MonsterSounds
{
    public FodderSounds fodderSounds;
    public FootsoldierSounds footsoldierSounds;
    public CommanderSounds commanderSounds;
}

[System.Serializable]
struct FodderSounds
{
    public AudioClip[] attackSounds;
    public AudioClip[] damagedSounds;
    public AudioClip[] footstepSounds;
    public AudioClip deathSound;
}

[System.Serializable]
struct FootsoldierSounds
{
    public AudioClip[] attackSounds;
    public AudioClip[] damagedSounds;
    public AudioClip[] footstepSounds;
    public AudioClip deathSound;
}
[System.Serializable]
struct CommanderSounds
{
    public AudioClip[] attackSounds;
    public AudioClip[] damagedSounds;
    public AudioClip[] footstepSounds;
    public AudioClip deathSound;
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

public enum EnemyTypes
{
    Fodder,
    Footsoldier,
    Commander
}