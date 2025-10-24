using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script should be attached to the object that the player hits.
// It MUST have a Collider2D with 'Is Trigger' checked!
public class NextLevelTrigger : MonoBehaviour
{
    // The name/tag of the player object
    [SerializeField] private string playerTag = "Player";

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object that entered the trigger is the Player
        if (other.CompareTag(playerTag))
        {
            // Call the LevelCompleted method on the GameManager
            if (GameManager.Instance != null)
            {
                GameManager.Instance.LevelCompleted();
            }
            Debug.Log("The Player Finish the Level");
            // Optional: Disable the collider/gameObject to prevent multiple triggers
            GetComponent<Collider2D>().enabled = false;
        }
    }
}
