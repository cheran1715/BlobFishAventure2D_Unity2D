using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This [System.Serializable] attribute lets us edit the properties of this class
// for each platform right in the Unity Inspector.
[System.Serializable]
public class ManagedPlatform
{
    [Tooltip("The Transform of the platform GameObject you want to move.")]
    public Transform platformTransform;
    [Tooltip("The starting point of the patrol.")]
    public Transform pointA;
    [Tooltip("The ending point of the patrol.")]
    public Transform pointB;
    [Tooltip("How fast the platform moves.")]
    public float speed = 2f;
}

public class PlatformManager : MonoBehaviour
{
    [Header("Platforms to Control")]
    // This array will hold all the platforms you want this manager to control.
    public ManagedPlatform[] platforms;

    // We need to store the current direction for each platform separately.
    private bool[] _movingTowardsB;

    void Start()
    {
        // Initialize the direction array to match the number of platforms
        _movingTowardsB = new bool[platforms.Length];

        // Set the initial state for each platform
        for (int i = 0; i < platforms.Length; i++)
        {
            if (platforms[i].platformTransform != null && platforms[i].pointA != null)
            {
                // Start each platform at Point A
                platforms[i].platformTransform.position = platforms[i].pointA.position;
                // Initially, all platforms will move towards Point B
                _movingTowardsB[i] = true;
            }
        }
    }

    void Update()
    {
        // This loop runs every frame for every platform in our list
        for (int i = 0; i < platforms.Length; i++)
        {
            // Skip this platform if any of its parts are not assigned
            if (platforms[i].platformTransform == null || platforms[i].pointA == null || platforms[i].pointB == null)
                continue;

            // Determine the current target point based on the direction
            Transform targetPoint = _movingTowardsB[i] ? platforms[i].pointB : platforms[i].pointA;

            // Move the platform towards the target point
            platforms[i].platformTransform.position = Vector3.MoveTowards(
                platforms[i].platformTransform.position,
                targetPoint.position,
                platforms[i].speed * Time.deltaTime
            );

            // Check if the platform has reached the target point
            if (Vector3.Distance(platforms[i].platformTransform.position, targetPoint.position) < 0.01f)
            {
                // If it has, flip the direction for the next frame
                _movingTowardsB[i] = !_movingTowardsB[i];
            }
        }
    }

    // This is a helper function to draw lines in the Scene view, making setup easier.
    private void OnDrawGizmos()
    {
        if (platforms == null) return;

        Gizmos.color = Color.green;
        foreach (var platform in platforms)
        {
            if (platform.pointA != null && platform.pointB != null)
            {
                Gizmos.DrawLine(platform.pointA.position, platform.pointB.position);
                Gizmos.DrawWireSphere(platform.pointA.position, 0.3f);
                Gizmos.DrawWireSphere(platform.pointB.position, 0.3f);
            }
        }
    }
}

