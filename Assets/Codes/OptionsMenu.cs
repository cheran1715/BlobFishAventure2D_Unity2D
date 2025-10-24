using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [Header("UI Elements")]
    public Slider musicSlider;
    public Slider sfxSlider;
    public Slider ambientSfxSlider; // The new slider for ambient sounds

    // Keys for saving settings
    private const string MusicVolumeKey = "MusicVolume";
    private const string SfxVolumeKey = "SfxVolume";
    private const string AmbientSfxVolumeKey = "AmbientSfxVolume"; // The new key

    private void Start()
    {
        // Load all saved settings when the menu opens
        LoadSettings();

        // Add listeners to each slider to call a function when their value changes
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSfxVolume);
        ambientSfxSlider.onValueChanged.AddListener(SetAmbientSfxVolume); // Add listener for the new slider
    }

    private void LoadSettings()
    {
        // Load Music Volume
        float musicVolume = PlayerPrefs.GetFloat(MusicVolumeKey, 1f);
        musicSlider.value = musicVolume;
        SetMusicVolume(musicVolume);

        // Load SFX Volume
        float sfxVolume = PlayerPrefs.GetFloat(SfxVolumeKey, 1f);
        sfxSlider.value = sfxVolume;
        SetSfxVolume(sfxVolume);

        // Load Ambient SFX Volume
        float ambientSfxVolume = PlayerPrefs.GetFloat(AmbientSfxVolumeKey, 1f);
        ambientSfxSlider.value = ambientSfxVolume;
        SetAmbientSfxVolume(ambientSfxVolume);
    }

    public void SetMusicVolume(float volume)
    {
        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.SetVolume(volume);
        }
        PlayerPrefs.SetFloat(MusicVolumeKey, volume);
    }

    public void SetSfxVolume(float volume)
    {
        if (SFXManager.Instance != null)
        {
            SFXManager.Instance.SetVolume(volume);
        }
        PlayerPrefs.SetFloat(SfxVolumeKey, volume);
    }

    // This new method handles the ambient SFX slider
    public void SetAmbientSfxVolume(float volume)
    {
        // If our AmbientSoundManager exists, tell it to update its volume
        if (AmbientSoundManager.Instance != null)
        {
            AmbientSoundManager.Instance.SetVolume(volume);
        }
        // Save the new volume setting
        PlayerPrefs.SetFloat(AmbientSfxVolumeKey, volume);
    }
}


