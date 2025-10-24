using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioController : MonoBehaviour
{
    // You can add more sound triggers here in the future!
    // For example: public string jumpSoundName = "PlayerJump";

    [Header("Sound Effect Names")]
    [Tooltip("The name of the water splash sound as defined in the SFXManager.")]
    public string waterSplashSoundName = "WaterSplash";

    // This function is called automatically by Unity whenever the player's collider enters a trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the trigger we entered is tagged as "Water"
        if (other.CompareTag("WaterIN"))
        {
            // If it is, and our SFXManager exists...
            if (SFXManager.Instance != null)
            {
                // ...tell the SFXManager to play the water splash sound.
                SFXManager.Instance.PlaySFX(waterSplashSoundName);
            }
        }

        // You could add more checks here for other triggers, like a "Coin" tag
        // if (other.CompareTag("Coin"))
        // {
        //     SFXManager.Instance.PlaySFX("CoinCollect");
        // }
    }
}
