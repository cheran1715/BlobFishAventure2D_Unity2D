using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    // When a 2D collider enters this platform's collider...
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // ...check if the object that collided has the "Player" tag.
        if (collision.gameObject.CompareTag("Player"))
        {
            // If it is the player, make the player a child of this platform.
            // This makes the player's movement relative to the platform.
            collision.transform.SetParent(this.transform);
        }
    }

    // When the player's collider exits this platform's collider...
    private void OnCollisionExit2D(Collision2D collision)
    {
        // ...check if it's the player leaving.
        if (collision.gameObject.CompareTag("Player"))
        {
            // If it is, un-parent the player so they can move freely again.
            collision.transform.SetParent(null);
        }
    }
}
