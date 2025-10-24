using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; // Required for Array.Find

// This class holds a sound name and its corresponding audio clip.
// The [System.Serializable] attribute lets us see and edit it in the Inspector.
[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    // Add a volume slider for each sound, defaulting to full volume (1).
    [Range(0f, 3f)] // A range from 0 (silent) to 3 (triple volume)
    public float volume = 1f;
}

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;

    // Create an array to hold all of your sound effects
    public Sound[] sfxSounds;

    // This will be our "speaker"
    private AudioSource audioSource;

    private void Awake()
    {
        // Singleton pattern: Ensure only one instance of the SFXManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Get the AudioSource component attached to this GameObject
        audioSource = GetComponent<AudioSource>();
    }

    // This is the public function that other scripts will call to play a sound
    public void PlaySFX(string name)
    {
        // Use Array.Find to search our sfxSounds array for a sound with the matching name
        Sound soundToPlay = Array.Find(sfxSounds, s => s.name == name);

        // If a sound with that name was found...
        if (soundToPlay != null)
        {
            // ...play it using PlayOneShot, which is great for overlapping sounds.
            // We use the individual sound's volume multiplied by the master volume of the AudioSource.
            audioSource.PlayOneShot(soundToPlay.clip, soundToPlay.volume);
        }
        else
        {
            // If no sound with that name was found, log a warning to the console.
            Debug.LogWarning("SFXManager: Sound not found in list: " + name);
        }
    }

    // This function will be called by the slider to set the master SFX volume.
    public void SetVolume(float volume)
    {
        if (audioSource != null)
        {
            audioSource.volume = volume;
        }
    }
}



