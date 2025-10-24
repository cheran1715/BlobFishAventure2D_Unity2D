using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class holds all the data for a single trap we want to manage.
// The [System.Serializable] attribute lets us edit it in the Inspector.
[System.Serializable]
public class ManagedTrap
{
    [Tooltip("The Transform of the saw trap GameObject.")]
    public Transform trapTransform;
    [Tooltip("The starting point of the patrol.")]
    public Transform pointA;
    [Tooltip("The ending point of the patrol.")]
    public Transform pointB;
    [Tooltip("How fast the trap moves.")]
    public float speed = 3f;
}

public class TrapManager : MonoBehaviour
{
    [Header("Traps to Control")]
    public ManagedTrap[] traps;

    // We store the current direction for each trap
    private bool[] _movingTowardsB;

    void Start()
    {
        // Initialize the direction array to match the number of traps
        _movingTowardsB = new bool[traps.Length];

        // Set the initial state for each trap
        for (int i = 0; i < traps.Length; i++)
        {
            if (traps[i].trapTransform != null && traps[i].pointA != null)
            {
                // Start each trap at Point A
                traps[i].trapTransform.position = traps[i].pointA.position;
                // Initially, all traps will move towards Point B
                _movingTowardsB[i] = true;
            }
        }
    }

    void Update()
    {
        // This loop runs every frame for every trap in our list
        for (int i = 0; i < traps.Length; i++)
        {
            // Skip this trap if any of its parts are not assigned
            if (traps[i].trapTransform == null || traps[i].pointA == null || traps[i].pointB == null)
                continue;

            // Determine the current target point based on the direction
            Transform targetPoint = _movingTowardsB[i] ? traps[i].pointB : traps[i].pointA;

            // Move the trap towards the target point
            traps[i].trapTransform.position = Vector3.MoveTowards(
                traps[i].trapTransform.position,
                targetPoint.position,
                traps[i].speed * Time.deltaTime
            );

            // Check if the trap has reached the target point
            if (Vector3.Distance(traps[i].trapTransform.position, targetPoint.position) < 0.01f)
            {
                // If it has, flip the direction for the next frame
                _movingTowardsB[i] = !_movingTowardsB[i];
            }
        }
    }

    // Helper to draw lines in the Scene view to visualize the paths
    private void OnDrawGizmos()
    {
        if (traps == null) return;

        Gizmos.color = Color.red; // Use a different color to distinguish from platforms
        foreach (var trap in traps)
        {
            if (trap.pointA != null && trap.pointB != null)
            {
                Gizmos.DrawLine(trap.pointA.position, trap.pointB.position);
                Gizmos.DrawWireSphere(trap.pointA.position, 0.3f);
                Gizmos.DrawWireSphere(trap.pointB.position, 0.3f);
            }
        }
    }
}


