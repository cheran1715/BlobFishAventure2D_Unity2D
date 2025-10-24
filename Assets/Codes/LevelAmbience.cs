using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelAmbience : MonoBehaviour
{
    [Header("Sound")]
    [Tooltip("The ambient sound loop for this specific level (e.g., forest sounds, cave drips).")]
    public AudioClip ambientClip;

    void Start()
    {
        // When the level starts, find the AmbientSoundManager and tell it to play our clip.
        if (AmbientSoundManager.Instance != null && ambientClip != null)
        {
            AmbientSoundManager.Instance.PlayAmbientLoop(ambientClip);
        }
    }
}
