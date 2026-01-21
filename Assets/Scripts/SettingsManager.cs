using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] Slider _mainSlider;
    [SerializeField] Slider _musicSlider;
    [SerializeField] Slider _sfxSlider;

    const float MAIN_DEFAULT_VOLUME = 0.5f;
    const float MUSIC_DEFAULT_VOLUME = 1f;
    const float SFX_DEFAULT_VOLUME = 1f;

    void Start()
    {
        _mainSlider.value = PlayerPrefs.GetFloat("Settings.MasterVolume", MAIN_DEFAULT_VOLUME);
        _musicSlider.value = PlayerPrefs.GetFloat("Settings.MusicVolume", MUSIC_DEFAULT_VOLUME);
        _sfxSlider.value = PlayerPrefs.GetFloat("Settings.SFXVolume", SFX_DEFAULT_VOLUME);
    }

    public void ChangeMainVolume(float newVol)
    {
        AudioManager.Instance.ChangeMasterVolume(newVol);
    }

    public void ChangeMusicVolume(float newVol)
    {
        AudioManager.Instance.ChangeMusicVolume(newVol);
    }

    public void ChangeSFXVolume(float newVol)
    {
        AudioManager.Instance.ChangeSFXVolume(newVol);
    }
}
