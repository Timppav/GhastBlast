using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] AudioMixer _mixer;

    AudioMixerGroup _musicGroup;
    AudioMixerGroup _sfxGroup;
    AudioSource _currentMusicSource;

    const string MUSIC_GROUP_NAME = "Music";
    const string SFX_GROUP_NAME = "SFX";

    const string MASTER_VOLUME_NAME = "MasterVolume";
    const string MUSIC_VOLUME_NAME = "MusicVolume";
    const string SFX_VOLUME_NAME = "SFXVolume";

    public enum SoundType
    {
        SFX,
        Music
    }

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        Init();
    }

    void Init()
    {
        _musicGroup = _mixer.FindMatchingGroups(MUSIC_GROUP_NAME)[0];
        _sfxGroup = _mixer.FindMatchingGroups(SFX_GROUP_NAME)[0];
    }

    public void PlaySceneMusic(AudioClip clip)
    {
        if (_currentMusicSource != null && _currentMusicSource.clip == clip && _currentMusicSource.isPlaying)
        {
            return;
        }

        if (_currentMusicSource != null)
        {
            Destroy(_currentMusicSource.gameObject);
            _currentMusicSource = null;
        }

        if (clip == null) return;

        GameObject musicObject = new GameObject(clip.name + " Music Source");
        DontDestroyOnLoad(musicObject);

        _currentMusicSource = musicObject.AddComponent<AudioSource>();
        _currentMusicSource.clip = clip;
        _currentMusicSource.volume = 1.0f;
        _currentMusicSource.loop = true;
        _currentMusicSource.outputAudioMixerGroup = _musicGroup;
        _currentMusicSource.Play();
    }

    public void ChangeMasterVolume(float volume)
    {
        _mixer.SetFloat(MASTER_VOLUME_NAME, Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("Settings.MasterVolume", volume);
        PlayerPrefs.Save();
    }

    public void ChangeMusicVolume(float volume)
    {
        _mixer.SetFloat(MUSIC_VOLUME_NAME, Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("Settings.MusicVolume", volume);
        PlayerPrefs.Save();
    }

    public void ChangeSFXVolume(float volume)
    {
        _mixer.SetFloat(SFX_VOLUME_NAME, Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("Settings.SFXVolume", volume);
        PlayerPrefs.Save();
    }

    public void PlayAudio(AudioClip audioClip, SoundType soundType, float volume, bool loop)
    {
        GameObject newAudioSource = new GameObject(audioClip.name + " Source");
        AudioSource audioSource = newAudioSource.AddComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.loop = loop;

        switch (soundType)
        {
            case SoundType.SFX:
                audioSource.outputAudioMixerGroup = _sfxGroup;
                break;
            case SoundType.Music:
                audioSource.outputAudioMixerGroup = _musicGroup;
                break;
        }

        audioSource.Play();

        if (!loop)
        {
            // Use a coroutine with WaitForSecondsRealtime instead
            StartCoroutine(DestroyAudioSourceAfterPlay(audioSource.gameObject, audioClip.length));
        }
    }

    System.Collections.IEnumerator DestroyAudioSourceAfterPlay(GameObject audioSourceObject, float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        Destroy(audioSourceObject);
    }
}
