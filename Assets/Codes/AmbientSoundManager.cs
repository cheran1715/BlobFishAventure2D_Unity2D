using System.Collections;
using UnityEngine;
using System.Collections.Generic; // Required for using Lists

public class AmbientSoundManager : MonoBehaviour
{
    public static AmbientSoundManager Instance;

    private AudioSource audioSource;

    // --- NEW ---
    // A static list to keep track of all other ambient sound sources in the scene
    public static List<AudioSource> positionalAmbientSources = new List<AudioSource>();
    // --- END NEW ---

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayAmbientLoop(AudioClip clip)
    {
        if (audioSource == null || clip == null) return;
        if (audioSource.clip == clip && audioSource.isPlaying) return;

        audioSource.clip = clip;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void SetVolume(float volume)
    {
        // Set the volume for the main, global ambient source
        if (audioSource != null)
        {
            audioSource.volume = volume;
        }

        // --- NEW ---
        // Also loop through all registered positional sources and update their volume
        // We use a try-catch block as a safeguard in case a source was destroyed improperly.
        try
        {
            foreach (AudioSource source in positionalAmbientSources)
            {
                if (source != null)
                {
                    source.volume = volume;
                }
            }
        }
        catch (System.Exception)
        {
            // If an error occurs (e.g., list was modified during loop), clean up nulls.
            positionalAmbientSources.RemoveAll(item => item == null);
        }
        // --- END NEW ---
    }
}



