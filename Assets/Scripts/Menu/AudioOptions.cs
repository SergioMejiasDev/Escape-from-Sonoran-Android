using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

/// <summary>
/// Class in charge of controlling sound settings.
/// </summary>
public class AudioOptions : MonoBehaviour
{
    public static AudioOptions audioOptions;

    [SerializeField] Slider musicSlider = null, sfxSlider = null;
    [SerializeField] AudioMixerGroup musicMixer = null, sfxMixer = null;
    float musicVolume, sfxVolume;

    private void Start()
    {
        audioOptions = this;

        LoadOptions();
    }

    private void Update()
    {
        musicVolume = musicSlider.value;
        sfxVolume = sfxSlider.value;

        musicMixer.audioMixer.SetFloat("MusicVolume", Mathf.Log10(musicVolume) * 20);
        sfxMixer.audioMixer.SetFloat("SFXVolume", Mathf.Log10(sfxVolume) * 20);
    }

    /// <summary>
    /// Function that saves sound settings.
    /// </summary>
    public void SaveOptions()
    {
        SaveManager.saveManager.musicVolume = musicVolume;
        SaveManager.saveManager.sfxVolume = sfxVolume;
        SaveManager.saveManager.SaveOptions();
    }

    /// <summary>
    /// Function that loads the sound settings.
    /// </summary>
    void LoadOptions()
    {
        float musicVolumeLoaded = SaveManager.saveManager.musicVolume;
        musicSlider.value = musicVolumeLoaded;

        float sfxVolumeLoaded = SaveManager.saveManager.sfxVolume;
        sfxSlider.value = sfxVolumeLoaded;
    }
}