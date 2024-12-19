using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class ToggleController : MonoBehaviour
{
    [SerializeField] private Toggle handToggle;
    [SerializeField] private Toggle controllerToggle;
    
    [Header("XR Components")]
    [SerializeField] private GameObject handInteractors;     // Child objects containing hand interactions
    [SerializeField] private GameObject controllerInteractors; // Child objects containing controller interactions
    [SerializeField] private GameObject xrOrigin;              // Reference to the XR Origin

    private void Start()
    {
        handToggle.onValueChanged.AddListener(OnHandToggleChanged);
        controllerToggle.onValueChanged.AddListener(OnControllerToggleChanged);

        // Set controller toggle as default
        handToggle.isOn = false;
        controllerToggle.isOn = true;

        // Initially set hand interactors to inactive regardless of toggle state
        if (handInteractors != null)
        {
            handInteractors.SetActive(false);
        }
        if (controllerInteractors != null)
        {
            controllerInteractors.SetActive(true);
        }
    }

    private void UpdateRigs()
    {
        // Instead of activating/deactivating entire rigs,
        // we only toggle the interactors
        handInteractors.SetActive(handToggle.isOn);
        controllerInteractors.SetActive(controllerToggle.isOn);

        // Ensure XR Origin and camera remain active
        if (xrOrigin != null)
        {
            xrOrigin.gameObject.SetActive(true);
            if (xrOrigin.GetComponent<Camera>() != null)
            {  
                xrOrigin.GetComponent<Camera>().gameObject.SetActive(true);
            }
        }
    }

    private void OnHandToggleChanged(bool isOn)
    {
        if (isOn)
        {
            controllerToggle.isOn = false;
        }
        else if (!controllerToggle.isOn)
        {
            // If trying to turn off hands and controller is also off,
            // force hands to stay on
            handToggle.isOn = true;
        }
    }

    private void OnControllerToggleChanged(bool isOn)
    {
        if (isOn)
        {
            handToggle.isOn = false;
        }
        else if (!handToggle.isOn)
        {
            // If trying to turn off controller and hands is also off,
            // force controller to stay on
            controllerToggle.isOn = true;
        }
    }

    // Call this method when an image is selected
    public void OnImageSelected()
    {
        // Now we only update the rigs based on toggle state when an image is selected
        UpdateRigs();
    }
}
