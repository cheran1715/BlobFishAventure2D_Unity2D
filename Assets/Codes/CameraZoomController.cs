using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine; // Very important!

public class CameraZoomController : MonoBehaviour
{
    [Header("References")]
    [Tooltip("The Cinemachine Virtual Camera that follows the player.")]
    public CinemachineVirtualCamera virtualCamera;

    [Header("Zoom Settings")]
    public float defaultZoom = 5f;
    public float zoomedOutZoom = 7f;
    public float zoomSpeed = 5f;

    private float targetZoom;

    private void Start()
    {
        // Set the initial zoom state
        targetZoom = defaultZoom;
    }

    void Update()
    {
        // If the virtual camera isn't set, do nothing
        if (virtualCamera == null) return;

        // Smoothly interpolate the camera's orthographic size towards the target
        virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(
            virtualCamera.m_Lens.OrthographicSize,
            targetZoom,
            zoomSpeed * Time.deltaTime
        );
    }

    // This is the public function that the DragShot script will call
    public void SetZoom(bool isZoomedOut)
    {
        targetZoom = isZoomedOut ? zoomedOutZoom : defaultZoom;
    }
}

