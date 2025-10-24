using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// These attributes automatically add the required components and ensure they are set up correctly.
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(BoxCollider2D))] // Or CircleCollider2D if you prefer
public class PositionalAmbientSource : MonoBehaviour
{
    private AudioSource audioSource;
    // This key MUST match the key used in your OptionsMenu script
    private const string AmbientSfxVolumeKey = "AmbientSfxVolume";

    private void Awake()
    {
        // Get the AudioSource component and configure it
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true; // This sound should loop
        audioSource.playOnAwake = false; // It should only play when triggered
        audioSource.spatialBlend = 0.0f; // Make it a 2D sound

        // Ensure the collider is a trigger
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void OnEnable()
    {
        // Add this sound source to the manager's list so its volume can be controlled
        AmbientSoundManager.positionalAmbientSources.Add(audioSource);
        // Load the saved volume setting immediately
        LoadVolume();
    }

    private void OnDisable()
    {
        // Remove this sound source from the list when it's disabled or destroyed
        AmbientSoundManager.positionalAmbientSources.Remove(audioSource);
    }

    // Loads the volume from PlayerPrefs and applies it
    private void LoadVolume()
    {
        float savedVolume = PlayerPrefs.GetFloat(AmbientSfxVolumeKey, 1f);
        audioSource.volume = savedVolume;
    }

    // When an object with a Rigidbody2D enters the trigger zone...
    private void OnTriggerEnter2D(Collider2D other)
    {
        // ...check if it's the player.
        if (other.CompareTag("Player"))
        {
            // If the sound isn't already playing, play it.
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
    }

    // When the object exits the trigger zone...
    private void OnTriggerExit2D(Collider2D other)
    {
        // ...check if it's the player.
        if (other.CompareTag("Player"))
        {
            // Stop the sound.
            audioSource.Stop();
        }
    }
}
    
