using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; // Required for UI check

public class DragShot : MonoBehaviour
{
    [Header("Mechanics")]
    public float AddToforce = 10f;
    public float dragLimit = 3f;
    public bool canDrag = true;

    [Header("References")]
    public Rigidbody2D rb;
    public LineRenderer line;

    // Private variables
    private bool isDragging;
    private Camera cam;
    private CameraZoomController cameraZoomController;

    private void Start()
    {
        cam = Camera.main;
        if (cam != null)
        {
            cameraZoomController = cam.GetComponent<CameraZoomController>();
        }

        if (rb != null)
        {
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        }

        if (line != null)
        {
            line.positionCount = 2;
            line.SetPosition(0, Vector3.zero);
            line.SetPosition(1, Vector3.zero);
            line.enabled = false;
            line.useWorldSpace = false;
        }
    }

    private void Update()
    {
        if (!canDrag || rb == null || line == null)
            return;

        // --- UNIFIED INPUT HANDLING (MOUSE & TOUCH) ---
        bool inputBegan = false, inputHeld = false, inputEnded = false;
        Vector2 screenPosition = Vector2.zero;

        // --- THIS LOGIC IS THE SAME ---
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            screenPosition = touch.position;
            if (touch.phase == TouchPhase.Began) inputBegan = true;
            if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary) inputHeld = true;
            if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) inputEnded = true;
        }
        else
        {
            screenPosition = Input.mousePosition;
            if (Input.GetMouseButtonDown(0)) inputBegan = true;
            if (Input.GetMouseButton(0)) inputHeld = true;
            if (Input.GetMouseButtonUp(0)) inputEnded = true;
        }
        // --- END OF INPUT HANDLING ---


        // --- LOGIC CHANGE IS HERE ---

        // 1. Check if input just began
        if (inputBegan)
        {
            // 2. ONLY check for UI on the first frame of input
            bool isOverUI = (Input.touchCount > 0)
                ? EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)
                : EventSystem.current.IsPointerOverGameObject();

            if (isOverUI)
            {
                // If the click started on the UI, do nothing.
                return;
            }
            else
            {
                // If the click started on the game world, start the drag.
                StartDrag();
            }
        }

        // We no longer need to check for UI here, because the drag is "locked in"
        if (isDragging && inputHeld)
        {
            ContinueDrag(screenPosition);
        }

        if (isDragging && inputEnded)
        {
            EndDrag();
        }
    }

    private void StartDrag()
    {
        if (cameraZoomController != null)
        {
            cameraZoomController.SetZoom(true);
        }
        isDragging = true;
        line.enabled = true;
        line.SetPosition(0, Vector3.zero);
    }

    private void ContinueDrag(Vector2 screenPosition)
    {
        Vector3 worldInputPos = cam.ScreenToWorldPoint(screenPosition);
        worldInputPos.z = 0f;
        Vector3 localInputPos = transform.InverseTransformPoint(worldInputPos);
        Vector3 dragVector = localInputPos;

        if (dragVector.magnitude > dragLimit)
        {
            dragVector = dragVector.normalized * dragLimit;
        }

        line.SetPosition(1, dragVector);
    }

    private void EndDrag()
    {
        if (cameraZoomController != null)
        {
            cameraZoomController.SetZoom(false);
        }

        Vector3 endPos = line.GetPosition(1);
        Vector3 localForceVector = -endPos;
        Vector3 worldForceVector = transform.TransformDirection(localForceVector);
        Vector3 force = worldForceVector * AddToforce;

        isDragging = false;
        line.enabled = false;

        rb.AddForce(force, ForceMode2D.Impulse);
        canDrag = false;
    }

    // CancelDrag is no longer needed by the Update loop, but can be kept
    // if other scripts need to cancel a drag externally.
    public void CancelDrag()
    {
        if (cameraZoomController != null)
        {
            cameraZoomController.SetZoom(false);
        }
        isDragging = false;
        line.enabled = false;
    }

    // --- Collision and Trigger Logic (No Changes Needed Here) ---
    // ... (OnCollisionEnter2D, OnTriggerEnter2D, etc. remain the same) ...
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            canDrag = true;
        }
        else if (collision.gameObject.CompareTag("OnPlatform"))
        {
            canDrag = false;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            canDrag = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("WaterIN")) // Recommended to use one "Water" tag
        {
            canDrag = true;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("WaterIN"))
        {
            canDrag = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("WaterIN"))
        {
            canDrag = false;
        }
    }
}

