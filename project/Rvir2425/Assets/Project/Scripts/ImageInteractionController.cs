using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;

public class ImageInteractionController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject imageContainer; // The object holding the image
    [SerializeField] private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;
    
    [Header("Zoom Settings")]
    [SerializeField] private float minZoomDistance = 0.2f;
    [SerializeField] private float maxZoomDistance = 2f;
    [SerializeField] private float zoomSpeed = 0.5f;

    [Header("Rotation Settings")]
    [SerializeField] private float rotationSpeed = 100f;

    private bool isZoomModeActive = false;
    private bool isRotateModeActive = false;
    private bool isGrabbed = false;
    private Transform grabberTransform;

    private void Start()
    {
        if (grabInteractable == null)
        {
            grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
         }

        // Setup grab events
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        isGrabbed = true;
        grabberTransform = args.interactorObject.transform;
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        isGrabbed = false;
        grabberTransform = null;
    }

    private void Update()
    {
        // Check for button inputs (A and B buttons)
        if (OVRInput.GetDown(OVRInput.Button.One)) // A button
        {
            ToggleZoomMode();
        }
        if (OVRInput.GetDown(OVRInput.Button.Two)) // B button
        {
            ToggleRotateMode();
        }

        if (isGrabbed && grabberTransform != null)
        {
            // Get thumbstick input
            Vector2 thumbstick = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);

            if (isZoomModeActive)
            {
                HandleZoom(thumbstick.y);
            }
            else if (isRotateModeActive)
            {
                HandleRotation(thumbstick.x);
            }
        }
    }

    private void ToggleZoomMode()
    {
        isZoomModeActive = !isZoomModeActive;
        isRotateModeActive = false; // Disable rotate mode when zoom is enabled
        Debug.Log($"Zoom Mode: {(isZoomModeActive ? "Enabled" : "Disabled")}");
    }

    private void ToggleRotateMode()
    {
        isRotateModeActive = !isRotateModeActive;
        isZoomModeActive = false; // Disable zoom mode when rotate is enabled
        Debug.Log($"Rotate Mode: {(isRotateModeActive ? "Enabled" : "Disabled")}");
    }

    private void HandleZoom(float thumbstickY)
    {
        if (Mathf.Abs(thumbstickY) < 0.1f) return; // Dead zone

        Vector3 directionToGrabber = grabberTransform.position - imageContainer.transform.position;
        float currentDistance = directionToGrabber.magnitude;
        float newDistance = currentDistance - (thumbstickY * zoomSpeed * Time.deltaTime);
        newDistance = Mathf.Clamp(newDistance, minZoomDistance, maxZoomDistance);

        // Update position
        Vector3 newPosition = grabberTransform.position - directionToGrabber.normalized * newDistance;
        imageContainer.transform.position = newPosition;
    }

    private void HandleRotation(float thumbstickX)
    {
        if (Mathf.Abs(thumbstickX) < 0.1f) return; // Dead zone

        // Rotate around the Y-axis
        imageContainer.transform.Rotate(Vector3.up, thumbstickX * rotationSpeed * Time.deltaTime);
    }

    private void OnDestroy()
    {
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(OnGrab);
            grabInteractable.selectExited.RemoveListener(OnRelease);
        }
    }
} 