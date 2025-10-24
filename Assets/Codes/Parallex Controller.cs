using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallexController : MonoBehaviour
{
    private float startPos, lengthSprite;
    public GameObject cam;
    public float parallexSpeed;

    void Start()
    {
        startPos = transform.position.x;
        lengthSprite = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // --- THIS IS THE FIX ---
    // We move the logic from FixedUpdate to LateUpdate.
    // LateUpdate runs after all other game logic, including the Cinemachine camera update.
    void LateUpdate()
    {
        // Check if the camera reference is valid
        if (cam == null) return;

        float distance = cam.transform.position.x * parallexSpeed;
        float movement = cam.transform.position.x * (1 - parallexSpeed);

        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);

        if (movement > startPos + lengthSprite)
        {
            startPos += lengthSprite;
        }
        else if (movement < startPos - lengthSprite)
        {
            startPos -= lengthSprite;
        }
    }
}
