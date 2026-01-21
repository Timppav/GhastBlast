using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] AudioMixer _mixer;
    [SerializeField] AudioClip _music;
    AudioMixerGroup _musicGroup;
    AudioMixerGroup _sfxGroup;

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

    void Init()
    {
        _musicGroup = _mixer.FindMatchingGroups(MUSIC_GROUP_NAME)[0];
        _sfxGroup = _mixer.FindMatchingGroups(SFX_GROUP_NAME)[0];

        PlayAudio(_music, SoundType.Music, 1.0f, true);
    }

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        Instance = this;

        Init();
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
        GameObject newAudioSource = new(audioClip.name + " Source");
        AudioSource audioSource = newAudioSource.AddComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.loop = loop;
        audioSource.Play();

        // Set which AudioMixerGroup the AudioSource is going to play through
        switch (soundType)
        {
            case SoundType.SFX:
                audioSource.outputAudioMixerGroup = Instance._sfxGroup;
                break;
            case SoundType.Music:
                audioSource.outputAudioMixerGroup = Instance._musicGroup;
                break;
            default:
                break;
        }

        audioSource.Play();

        // Destroy the AudioSource if loop is set to false
        if (!loop)
        {
            Destroy(audioSource.gameObject, audioClip.length);
        }
    }
}
