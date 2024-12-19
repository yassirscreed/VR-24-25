using UnityEngine;
using UnityEngine.UI;

public class ToggleController : MonoBehaviour
{
    [SerializeField] private Toggle controllerToggle;
    [SerializeField] private Toggle handsToggle;

    [Header("Controller Objects")]
    [SerializeField] private GameObject leftController;
    [SerializeField] private GameObject rightController;
    [SerializeField] private GameObject leftControllerRay;
    [SerializeField] private GameObject rightControllerRay;

    [Header("Hand Objects")]
    [SerializeField] private GameObject leftHand;
    [SerializeField] private GameObject rightHand;
    [SerializeField] private GameObject handRayInteractor;

    private void Start()
    {
        controllerToggle.onValueChanged.AddListener(OnControllerToggleChanged);
        handsToggle.onValueChanged.AddListener(OnHandsToggleChanged);

        // Initialize based on starting toggle state
        UpdateObjectsState(controllerToggle.isOn);
    }

    private void OnControllerToggleChanged(bool isOn)
    {
        if (isOn)
        {
            handsToggle.isOn = false;
            UpdateObjectsState(true);
        }
    }

    private void OnHandsToggleChanged(bool isOn)
    {
        if (isOn)
        {
            controllerToggle.isOn = false;
            UpdateObjectsState(false);
        }
    }

    private void UpdateObjectsState(bool useControllers)
    {
        // Controller objects
        leftController.SetActive(useControllers);
        rightController.SetActive(useControllers);
        leftControllerRay.SetActive(useControllers);
        rightControllerRay.SetActive(useControllers);

        // Hand objects
        leftHand.SetActive(!useControllers);
        rightHand.SetActive(!useControllers);
        handRayInteractor.SetActive(!useControllers);
    }

    private void OnDestroy()
    {
        if (controllerToggle != null) controllerToggle.onValueChanged.RemoveListener(OnControllerToggleChanged);
        if (handsToggle != null) handsToggle.onValueChanged.RemoveListener(OnHandsToggleChanged);
    }
} 