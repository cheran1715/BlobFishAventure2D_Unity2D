using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance;

    [Header("All Checkpoints in Scene")]
    public Transform[] checkpoints; // Assign all checkpoints in Inspector

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Returns the nearest checkpoint to the player at start.
    /// </summary>
    public Transform GetNearestCheckpoint(Vector3 playerPosition)
    {
        if (checkpoints == null || checkpoints.Length == 0)
            return null;

        Transform nearest = checkpoints[0];
        float minDistance = Vector3.Distance(playerPosition, nearest.position);

        for (int i = 1; i < checkpoints.Length; i++)
        {
            float dist = Vector3.Distance(playerPosition, checkpoints[i].position);
            if (dist < minDistance)
            {
                minDistance = dist;
                nearest = checkpoints[i];
            }
        }

        return nearest;
    }

    /// <summary>
    /// Returns the first checkpoint (used when player dies).
    /// </summary>
    public Transform GetFirstCheckpoint()
    {
        if (checkpoints != null && checkpoints.Length > 0)
            return checkpoints[0];
        return null;
    }
}
